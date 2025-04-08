using System.Data.Common;

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

  public void Update(User user, string? username, string? password, bool? isAdmin)
  {
    // TODO: hash password
    dao.Update(user, username, password, isAdmin);
  }

  public void Delete(User user)
  {
    dao.Delete(user);
  }
}