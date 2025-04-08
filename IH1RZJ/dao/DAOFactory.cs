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

  private IUserDAO userDAO = new UserJsonDAO(Config.Instance.UserPath);
  public IUserDAO UserDAO => userDAO;

  private IPersonDAO personDAO = new PersonJsonDAO(Config.Instance.PersonPath);
  public IPersonDAO PersonDAO => personDAO;
}