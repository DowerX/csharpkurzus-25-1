using System.Text.Json;

using IH1RZJ.Model;
using IH1RZJ.Model.DTO.Json;
using IH1RZJ.Model.DTO;

namespace IH1RZJ.DAO.Json;

public class AppearanceJsonDAO : IAppearanceDAO, IAsyncDisposable
{
  private readonly List<Appearance> appearances;
  private readonly string path;

  public AppearanceJsonDAO(string path)
  {
    this.path = path;
    using FileStream stream = File.OpenRead(path);
    List<AppearanceJsonDTO>? appearancesDTOs = JsonSerializer.Deserialize<List<AppearanceJsonDTO>>(stream, Config.JsonOptions);

    if (appearancesDTOs == null)
    {
      throw new Exception("Failed to load apperances!");
    }

    appearances = appearancesDTOs
      .AsParallel()
      .Select(dto => dto.ToDomain())
      .ToList();
  }

  public async Task Save()
  {
    string tempFile = Path.GetTempFileName();
    try
    {
      var dtoList = appearances
        .AsParallel()
        .Select(review => review.ToDTO())
        .ToList();

      await using FileStream stream = File.Create(tempFile);
      await JsonSerializer.SerializeAsync(stream, dtoList, Config.JsonOptions);
      stream.Close();

      File.Move(tempFile, path, true);
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);
    }
  }

  public async ValueTask DisposeAsync()
  {
    await Save();
  }

  public async Task Create(Appearance appearance)
  {
    appearances.Add(appearance);
    await Save();
  }

  public async Task Delete(Appearance appearance)
  {
    appearances.Remove(appearance);
    await Save();
  }

  public Task<IEnumerable<Appearance>> List(Guid? id, Guid? movie, Guid? person, Role? role)
  {
    var result = appearances
      .AsParallel()
      .Where(appearance => id == null || appearance.ID == id)
      .Where(appearance => movie == null || appearance.MovieID == movie)
      .Where(appearance => person == null || appearance.PersonID == person)
      .Where(appearance => role == null || appearance.Role == role)
      .ToList();

    return Task.FromResult<IEnumerable<Appearance>>(result);
  }

  public async Task Update(Appearance appearance)
  {
    int index = appearances.FindIndex(a => appearance.ID == a.ID);
    if (index != -1)
    {
      appearances[index] = appearance;
      await Save();
    }
  }
}