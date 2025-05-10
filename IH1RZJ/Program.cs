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

    // new PersonController(DAOFactory.Instance.PersonDAO).Create("Adam Scott", DateTime.UtcNow, null, "cool guy");
    // new MovieController(DAOFactory.Instance.MovieDAO).Create("A Minecraft Movie", "gayming", DateTime.UtcNow);

    // interface
    Application.QuitKey = Key.Esc;
    Application.Run<WelcomeWindow>();
    Application.Shutdown();
  }
}