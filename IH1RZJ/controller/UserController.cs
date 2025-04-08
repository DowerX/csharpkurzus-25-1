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

  public void Create(string username, string password, bool isAdmin)
  {
    // TODO: hash password
    dao.Create(new User
    {
      Username = username,
      PasswordHash = password,
      IsAdmin = isAdmin
    });
  }

  public IEnumerable<User> List(Guid? id, string? username, bool? isAdmin)
  {
    return dao.List(id, username, isAdmin);
  }

  public void Update(User user)
  {
    // TODO: hash password
    dao.Update(user);
  }

  public void Delete(User user)
  {
    dao.Delete(user);
  }

  public bool Login(string username, string password)
  {
    // TODO: hash password
    try
    {
      return dao.List(null, username, null).Single().PasswordHash == password;
    }
    catch (Exception) // didn't find any users with that name
    {
      return false;
    }
  }

  public bool Register(string username, string password, string repeatPassword)
  {
    if (password != repeatPassword)
      return false;

    if (dao.List(null, username, null).Count() != 0)
      return false;

    dao.Create(new User
    {
      Username = username,
      PasswordHash = password,
      IsAdmin = false
    });

    return true;
  }
}