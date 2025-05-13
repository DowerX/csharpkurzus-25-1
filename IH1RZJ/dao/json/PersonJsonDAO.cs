
using System.Text.Json;

using IH1RZJ.Model;
using IH1RZJ.Model.DTO.Json;
using IH1RZJ.Model.DTO;

namespace IH1RZJ.DAO.Json;

public class PersonJsonDAO : IPersonDAO, IAsyncDisposable
{
  private readonly List<Person> people;
  private readonly string path;

  public PersonJsonDAO(string path)
  {
    this.path = path;
    using FileStream stream = File.OpenRead(path);
    List<PersonJsonDTO>? peopleDTOs = JsonSerializer.Deserialize<List<PersonJsonDTO>>(stream, Config.JsonOptions);

    if (peopleDTOs == null)
    {
      throw new Exception("Failed to load people!");
    }

    people = peopleDTOs
      .AsParallel()
      .Select(dto => dto.ToDomain())
      .ToList();
  }

  public async Task Save()
  {
    string tempFile = Path.GetTempFileName();
    try
    {
      var dtoList = people
        .AsParallel()
        .Select(person => person.ToDTO())
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

  public async Task Create(Person person)
  {
    people.Add(person);
    await Save();
  }

  public async Task Delete(Person person)
  {
    people.Remove(person);
    await Save();
  }

  public Task<IEnumerable<Person>> List(Guid? id, string? name)
  {
    var result = people
      .AsParallel()
      .Where(person => id == null || person.ID == id)
      .Where(person => name == null || person.Name == name)
      .ToList();

    return Task.FromResult<IEnumerable<Person>>(result);
  }

  public async Task Update(Person person)
  {
    int index = people.FindIndex(p => person.ID == p.ID);
    if (index != -1)
    {
      people[index] = person;
      await Save();
    }
  }
}