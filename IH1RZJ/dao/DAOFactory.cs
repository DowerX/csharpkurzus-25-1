using IH1RZJ.DAO.Json;

namespace IH1RZJ.DAO;

public class DAOFactory
{
  private static DAOFactory instance;

  public static DAOFactory Instance
  {
    get
    {
      if (instance == null)
        instance = new();

      return instance;
    }
  }
  private DAOFactory() { }

  private IUserDAO userDAO = new UserJsonDAO(Config.Instance.UsersPath);
  public IUserDAO UserDAO => userDAO;

  private IPersonDAO personDAO = new PersonJsonDAO(Config.Instance.PeoplePath);
  public IPersonDAO PersonDAO => personDAO;

  private IMovieDAO movieDAO = new MovieJsonDAO(Config.Instance.MoviesPath);
  public IMovieDAO MovieDAO => movieDAO;
}