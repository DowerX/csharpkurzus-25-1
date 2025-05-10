using IH1RZJ.Model;

namespace IH1RZJ.DAO;

public interface IUserDAO
{
  public Task Create(User user);
  public Task<IEnumerable<User>> List(Guid? id, string? username, bool? isAdmin);
  public Task Update(User user);
  public Task Delete(User user);
}