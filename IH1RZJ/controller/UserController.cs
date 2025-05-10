using System.Threading.Tasks;

using IH1RZJ.DAO;
using IH1RZJ.Model;

namespace IH1RZJ.Controller;

public class UserController
{
  private readonly IUserDAO dao;

  public UserController(IUserDAO dao)
  {
    this.dao = dao ?? throw new ArgumentNullException(nameof(dao));
  }

  public async Task Create(string username, string password, bool isAdmin)
  {
    // TODO: hash password
    await dao.Create(new User
    {
      Username = username,
      PasswordHash = password,
      IsAdmin = isAdmin
    });
  }

  public Task<IEnumerable<User>> List(Guid? id, string? username, bool? isAdmin)
  {
    return dao.List(id, username, isAdmin);
  }

  public async Task Update(User user)
  {
    // TODO: hash password
    await dao.Update(user);
  }

  public async Task Delete(User user)
  {
    await dao.Delete(user);
  }

  public async Task<bool> Login(string username, string password)
  {
    // TODO: hash password
    try
    {
      return (await dao.List(null, username, null)).Single().PasswordHash == password;
    }
    catch (Exception) // didn't find any users with that name
    {
      return false;
    }
  }

  public async Task<bool> Register(string username, string password, string repeatPassword)
  {
    if (password != repeatPassword)
      return false;

    if ((await dao.List(null, username, null)).Count() != 0)
      return false;

    await dao.Create(new User
    {
      Username = username,
      PasswordHash = password,
      IsAdmin = false
    });

    return true;
  }
}