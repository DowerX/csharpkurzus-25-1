using System.Text.Json;

using IH1RZJ.Controller;
using IH1RZJ.DAO.Json;
using IH1RZJ.Model;
using IH1RZJ.View.ConsoleUI;
using IH1RZJ.View.ConsoleUI.Screen;

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

    // create daos
    using var userDao = new UserJsonDAO(Config.Instance.UserPath);

    // create controllers
    UserController userController = new UserController(userDao);

    // interface
    new ConsoleUI().Show();

    return 0;
  }

  static int Print()
  {
    User user = new User
    {
      Username = "testuser1",
      PasswordHash = "hash",
      IsAdmin = true
    };

    Console.WriteLine(JsonSerializer.Serialize(user, Config.Instance.JsonOptions));
    return 0;
  }
}