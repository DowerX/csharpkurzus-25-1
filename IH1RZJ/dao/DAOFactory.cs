using IH1RZJ.DAO.Json;

namespace IH1RZJ.DAO;

public class DAOFactory
{
  private static DAOFactory instance = new DAOFactory();

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

  private readonly IUserDAO userDAO = new UserJsonDAO(Config.Instance.UsersPath);
  public IUserDAO UserDAO => userDAO;

  private readonly IPersonDAO personDAO = new PersonJsonDAO(Config.Instance.PeoplePath);
  public IPersonDAO PersonDAO => personDAO;

  private readonly IMovieDAO movieDAO = new MovieJsonDAO(Config.Instance.MoviesPath);
  public IMovieDAO MovieDAO => movieDAO;

  private readonly IReviewDAO reviewDAO = new ReviewJsonDAO(Config.Instance.ReviewsPath);
  public IReviewDAO ReviewDAO => reviewDAO;
}