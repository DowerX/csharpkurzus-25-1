using IH1RZJ.DAO;
using IH1RZJ.View.Console;

using Microsoft.Extensions.Configuration;

using Terminal.Gui;

namespace IH1RZJ;

internal class Program
{
  private static void Main(string[] args)
  {
    // load config
    try
    {
      new ConfigurationBuilder()
        .AddEnvironmentVariables(prefix: "MOVIE_")
        .AddCommandLine(args)
        .Build()
        .Bind(Config.Instance);
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);
      return;
    }

    try
    {
      // this forces an instance creation
      // and tries to load all DAOs
      // might fail to read file, or parse...
      DAOFactory.GetInstance();
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"Failed to create DAOs: {ex.Message}");
      return;
    }

    // var movieCotroller = new MovieController(
    //   DAOFactory.Instance.MovieDAO,
    //   DAOFactory.Instance.ReviewDAO,
    //   DAOFactory.Instance.PersonDAO,
    //   DAOFactory.Instance.AppearanceDAO);
    // var userCotroller = new UserController(
    //   DAOFactory.Instance.UserDAO,
    //   DAOFactory.Instance.ReviewDAO);
    // var personController = new PersonController(DAOFactory.Instance.PersonDAO);

    // await movieCotroller.Create("A Minecraft Movie", "gaming", DateTime.UtcNow);

    // interface
    Application.QuitKey = Key.Esc;
    Application.Run<WelcomeWindow>();
    Application.Shutdown();
  }
}