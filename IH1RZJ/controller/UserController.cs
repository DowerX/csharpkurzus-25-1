using IH1RZJ.DAO;
using IH1RZJ.Model;

namespace IH1RZJ.Controller;

public class UserController
{
  private readonly IUserDAO userDAO;
  private readonly IReviewDAO reviewDAO;

  public static User? CurrentUser { get; private set; }

  public UserController(IUserDAO userDAO, IReviewDAO reviewDAO)
  {
    this.userDAO = userDAO ?? throw new ArgumentNullException(nameof(userDAO));
    this.reviewDAO = reviewDAO ?? throw new ArgumentNullException(nameof(reviewDAO));
  }

  public async Task Create(string username, string password, bool isAdmin)
  {
    // TODO: hash password
    await userDAO.Create(new User
    {
      Username = username,
      PasswordHash = password,
      IsAdmin = isAdmin
    });
  }

  public Task<IEnumerable<User>> List(Guid? id, string? username, bool? isAdmin)
  {
    return userDAO.List(id, username, isAdmin);
  }

  public async Task Update(User user)
  {
    // TODO: hash password
    await userDAO.Update(user);
  }

  public async Task Delete(User user)
  {
    foreach (Review review in await reviewDAO.List(null, null, user.ID, null))
    {
      await reviewDAO.Delete(review);
    }
    await userDAO.Delete(user);
  }

  public async Task<bool> Login(string username, string password)
  {
    // TODO: hash password
    try
    {
      User user = (await userDAO.List(null, username, null)).Single();
      if (user.PasswordHash == password)
      {
        CurrentUser = user;
        return true;
      }
      else
      {
        return false;
      }
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

    if ((await userDAO.List(null, username, null)).Count() != 0)
      return false;

    await userDAO.Create(new User
    {
      Username = username,
      PasswordHash = password,
      IsAdmin = false
    });

    return true;
  }
}