namespace IH1RZJ.View.ConsoleUI.Screen;

using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

public class LoginScreen : TableScreen
{
  public LoginScreen(ref Context context) :
  base(
    new() {
      new CustomRow<string>("Username",
        (string text, ref Context context) => $"{text}: {(context.ContainsKey(text) ? (string)context[text] : "")}",
        (ref string text, ref Context context) => {
          Console.Clear();
          Console.Write(text + ": ");
          context[text] = Console.ReadLine();
        }),
      new CustomRow<string>("Password",
        (string text, ref Context context) => $"{text}: {(context.ContainsKey(text) ? string.Concat(Enumerable.Repeat("*", ((string)context[text]).Length)) : "")}",
        (ref string text, ref Context context) => {
          Console.Clear();
          Console.Write(text + ": ");
          context[text] = Console.ReadLine();
        }),
      new StringRow("Login",
        (ref Context context) => {
          if (context.ContainsKey("Username") && context.ContainsKey("Password"))
          {
            if (new UserController(DAOFactory.Instance.UserDAO).Login((string)context["Username"], (string)context["Password"]))
            {
            new MainScreen(ref context).Show();
            }
            else
            {
            Console.WriteLine("Failed login! Press ENTER...");
            Console.ReadLine();
            }
          } else {
            Console.WriteLine("Missing username or password! Press ENTER...");
          }
      })
    },
    ref context)
  { }
}