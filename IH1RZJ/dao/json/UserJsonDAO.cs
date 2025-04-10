using System.Text.Json;

using IH1RZJ.Model;
using IH1RZJ.Model.DTO.Json;
using IH1RZJ.Model.DTO;

namespace IH1RZJ.DAO.Json;

public class UserJsonDAO : IUserDAO, IDisposable
{
  private List<User> users;
  private string path;

  public UserJsonDAO(string path)
  {
    this.path = path;
    using FileStream stream = File.OpenRead(path);
    List<UserJsonDTO>? userDTOs = JsonSerializer.Deserialize<List<UserJsonDTO>>(stream, Config.Instance.JsonOptions);

    if (userDTOs == null)
    {
      throw new Exception("Failed to load users!");
    }

    users = userDTOs
      .AsParallel()
      .Select(dto => dto.ToDomain())
      .ToList();
  }

  public void Save()
  {
    string tempFile = Path.GetTempFileName();
    try
    {
      using FileStream stream = File.Create(tempFile);

      stream.Write(System.Text.Encoding.UTF8.GetBytes(
        JsonSerializer.Serialize(
          users
            .AsParallel()
            .Select(user => user.ToDTO())
            .ToList(),
          Config.Instance.JsonOptions
      )));
      stream.Close();

      File.Move(tempFile, path, true);
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);
    }
  }

  public void Dispose()
  {
    Save();
  }

  public void Create(User user)
  {
    users.Add(user);
    Save();
  }

  public void Delete(User user)
  {
    users.Remove(user);
    Save();
  }

  public IEnumerable<User> List(Guid? id, string? username, bool? isAdmin)
  {
    return users
      .AsParallel()
      .Where(user => id == null || user.ID == id)
      .Where(user => username == null || user.Username == username)
      .Where(user => isAdmin == null || user.IsAdmin == isAdmin)
      .ToList();
  }

  public void Update(User user)
  {
    int index = users.FindIndex(u => user.ID == u.ID);
    users[index] = user;
    Save();
  }
}