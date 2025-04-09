using IH1RZJ.View.ConsoleUI;

using Microsoft.Extensions.Configuration;

namespace IH1RZJ;

internal class Program
{
  private static int Main(string[] args)
  {
    // load config
    try
    {
      var configRoot = new ConfigurationBuilder()
          .AddEnvironmentVariables(prefix: "MOVIE_")
          .AddCommandLine(args)
          .Build();
      configRoot.Bind(Config.Instance);
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);
      return -1;
    }

    // new PersonController(DAOFactory.Instance.PersonDAO).Create("Adam Scott", DateTime.UtcNow, null, "cool guy");

    // interface
    new ConsoleUI().Show();

    return 0;
  }
}