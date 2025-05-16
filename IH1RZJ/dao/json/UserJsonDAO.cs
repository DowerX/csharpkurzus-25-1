using System.Text.Json;

using IH1RZJ.Model;
using IH1RZJ.Model.DTO.Json;
using IH1RZJ.Model.DTO;

namespace IH1RZJ.DAO.Json;

public class UserJsonDAO : IUserDAO, IAsyncDisposable
{
  private readonly List<User> users;
  private readonly string path;
  private readonly Lock lockObj = new();

  public UserJsonDAO(string path)
  {
    this.path = path;
    using FileStream stream = File.OpenRead(path);
    List<UserJsonDTO>? userDTOs = JsonSerializer.Deserialize<List<UserJsonDTO>>(stream, Config.JsonOptions);

    if (userDTOs == null)
    {
      throw new Exception("Failed to load users!");
    }

    users = userDTOs
      .AsParallel()
      .Select(dto => dto.ToDomain())
      .ToList();
  }

  public async Task Save()
  {
    string tempFile = Path.GetTempFileName();
    try
    {
      List<UserJsonDTO> dtoList;
      lock (lockObj)
      {
        dtoList = users
          .AsParallel()
          .Select(movie => movie.ToDTO())
          .ToList();
      }

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

  public async Task Create(User user)
  {
    lock (lockObj)
    {
      users.Add(user);
    }
    await Save();
  }

  public async Task Delete(User user)
  {
    lock (lockObj)
    {
      users.Remove(user);
    }
    await Save();
  }

  public Task<IEnumerable<User>> List(Guid? id, string? username, bool? isAdmin)
  {
    List<User> result;
    lock (lockObj)
    {
      result = users
        .AsParallel()
        .Where(user => id == null || user.ID == id)
        .Where(user => username == null || user.Username == username)
        .Where(user => isAdmin == null || user.IsAdmin == isAdmin)
        .ToList();
    }

    return Task.FromResult<IEnumerable<User>>(result);
  }

  public async Task Update(User user)
  {
    int index = -1;
    lock (lockObj)
    {
      index = users.FindIndex(u => user.ID == u.ID);
      if (index != -1)
      {
        users[index] = user;
      }
    }

    if (index != -1) await Save();
  }
}