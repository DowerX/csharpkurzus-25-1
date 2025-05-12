using System.Threading.Tasks;

using IH1RZJ.Controller;
using IH1RZJ.DAO;

using Microsoft.Extensions.Configuration;

using Terminal.Gui;

namespace IH1RZJ;

internal class Program
{
  private static async Task Main(string[] args)
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
    var movieCotroller = new MovieController(DAOFactory.Instance.MovieDAO, DAOFactory.Instance.ReviewDAO);
    var personController = new PersonController(DAOFactory.Instance.PersonDAO);
    var reviewController = new ReviewController(DAOFactory.Instance.ReviewDAO);
    var userCotroller = new UserController(DAOFactory.Instance.UserDAO, DAOFactory.Instance.ReviewDAO);

    // await movieCotroller.Create("A Minecraft Movie", "gaming", DateTime.UtcNow);

    await reviewController.Create(
        (await movieCotroller.List(null, null)).First().ID,
        (await userCotroller.List(null, null, null)).First().ID,
        9
    );

    // interface
    Application.QuitKey = Key.Esc;
    Application.Run<WelcomeWindow>();
    Application.Shutdown();
  }
}