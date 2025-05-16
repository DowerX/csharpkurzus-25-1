using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.View.Console;

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
      var config = new ConfigurationBuilder()
        .AddEnvironmentVariables(prefix: "MOVIE_")
        .AddCommandLine(args)
        .Build();

      if (args.Contains("--help") || args.Contains("-h"))
      {
        Console.WriteLine("Usage: IH1RZJ [options]");
        Console.WriteLine("Options:");
        Console.WriteLine("  --help, -h              Show this help message");
        Console.WriteLine("  --DemoMode (true/false) Enable demo mode (erases all data)");
        Console.WriteLine("  --AppearancesPath       Path to appearances file");
        Console.WriteLine("  --MoviesPath            Path to movies file");
        Console.WriteLine("  --PeoplePath            Path to people file");
        Console.WriteLine("  --ReviewsPath           Path to reviews file");
        Console.WriteLine("  --UsersPath             Path to users file");
        return;
      }

      config.Bind(Config.Instance);
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);
      return;
    }

    try
    {
      // fill database with demo data
      // *will erase all existing records!* 
      if (Config.Instance.DemoMode)
      {
        await DemoMode();
      }

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

    // interface
    Application.QuitKey = Key.Esc;
    Application.Run<WelcomeWindow>();
    Application.Shutdown();
  }

  private static async Task DemoMode()
  {
    // create files
    await File.WriteAllTextAsync(Config.Instance.AppearancesPath, "[]");
    await File.WriteAllTextAsync(Config.Instance.MoviesPath, "[]");
    await File.WriteAllTextAsync(Config.Instance.PeoplePath, "[]");
    await File.WriteAllTextAsync(Config.Instance.ReviewsPath, "[]");
    await File.WriteAllTextAsync(Config.Instance.UsersPath, "[]");

    // make sure they all load
    DAOFactory.GetInstance();

    // populate with demo data
    var movieCotroller = new MovieController(
      DAOFactory.Instance.MovieDAO,
      DAOFactory.Instance.ReviewDAO,
      DAOFactory.Instance.PersonDAO,
      DAOFactory.Instance.AppearanceDAO);
    var userCotroller = new UserController(
      DAOFactory.Instance.UserDAO,
      DAOFactory.Instance.ReviewDAO);
    var personController = new PersonController(DAOFactory.Instance.PersonDAO);

    await userCotroller.Create("admin", "admin", true);
    await userCotroller.Create("user1", "user1", false);

    await personController.Create("Alive Actor", DateTime.UtcNow, null, "this is a demo person");
    await personController.Create("Dead Actor", DateTime.UtcNow, DateTime.UtcNow, "this is a dead demo person");

    await movieCotroller.Create("Demo Movie", "this is a demo movie", DateTime.UtcNow);
  }
}