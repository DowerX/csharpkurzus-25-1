namespace IH1RZJ.View.ConsoleUI.Screen;

using IH1RZJ.Controller;
using IH1RZJ.DAO;

using Context = Dictionary<string, object>;

public class LoginScreen : TableScreen<string>
{
  public LoginScreen(ref Context context) :
  base(
    new() { "Username", "Password", "Login" },
    (string text, ref Context c) =>
    {
      switch (text)
      {
        case "Username":
          return $"{text}: {(c.ContainsKey(text) ? c[text] : "")}";
        case "Password":
          return $"{text}: {(c.ContainsKey(text) ? string.Concat(Enumerable.Repeat("*", ((string)c[text]).Length)) : "")}";
        default:
          return text;
      }
    },
    (string text, ref Context c) =>
    {
      Console.Clear();
      if (text != "Login")
      {
        Console.Write(text + ": ");
        c[text] = Console.ReadLine();
      }
      else if (c.ContainsKey("Username") && c.ContainsKey("Password"))
      {
        if (new UserController(DAOFactory.Instance.UserDAO).Login((string)c["Username"], (string)c["Password"]))
        {
          new MainScreen(ref c).Show();
        }
        else
        {
          Console.WriteLine("Failed login! Press ENTER...");
          Console.ReadLine();
        }
      }
    },
    ref context)
  { }
}