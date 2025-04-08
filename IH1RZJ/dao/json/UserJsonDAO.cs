
using System.Text.Json;

using IH1RZJ.Model;
using IH1RZJ.Model.DTO.Json;
using IH1RZJ.Model.DTO;

namespace IH1RZJ.DAO.Json;

public class UserJsonDAO : IUserDAO, IDisposable
{
  private List<User> users;

  public UserJsonDAO(string path)
  {
    using FileStream stream = File.OpenRead(Config.Instance.UserPath);
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

  public void Dispose()
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

      File.Move(tempFile, Config.Instance.UserPath, true);
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);
    }
  }

  public void Create(User user)
  {
    users.Add(user);
  }

  public void Delete(User user)
  {
    users.Remove(user);
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

  public void Update(User user, string? username, string? passwordHash, bool? isAdmin)
  {
    int index = users.FindIndex(u => user.ID == u.ID);
    users[index].Username = username != null ? username : users[index].Username;
    users[index].PasswordHash = passwordHash != null ? passwordHash : users[index].PasswordHash;
    users[index].IsAdmin = (bool)(isAdmin != null ? isAdmin : users[index].IsAdmin);
  }
}