using IH1RZJ.Model;

namespace IH1RZJ.DAO;

public interface IUserDAO
{
  public void Create(User user);
  public IEnumerable<User> List(Guid? id, string? username, bool? isAdmin);
  public void Update(User user, string? username, string? passwordHash, bool? isAdmin);
  public void Delete(User user);
}