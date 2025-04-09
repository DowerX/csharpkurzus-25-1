namespace IH1RZJ.View.ConsoleUI.Screen;

using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

public class RegisterScreen : TableScreen
{
  public RegisterScreen(ref Context context) :
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
      new CustomRow<string>("Repeat password",
        (string text, ref Context context) => $"{text}: {(context.ContainsKey(text) ? string.Concat(Enumerable.Repeat("*", ((string)context[text]).Length)) : "")}",
        (ref string text, ref Context context) => {
          Console.Clear();
          Console.Write(text + ": ");
          context[text] = Console.ReadLine();
        }),
      new StringRow("Register", (ref Context context) => {
        if (context.ContainsKey("Username") && context.ContainsKey("Password") && context.ContainsKey("Repeat password"))
        {
          if (new UserController(DAOFactory.Instance.UserDAO).Register((string)context["Username"], (string)context["Password"], (string)context["Repeat password"]))
          {
           Console.WriteLine("Succesful register! Press ENTER...");
           Console.ReadLine();
          }
          else
          {
           Console.WriteLine("Failed register! Press ENTER...");
           Console.ReadLine();
          }
        } else {
          Console.WriteLine("Missing field! Press ENTER...");
        }
      })
    },
    ref context)
  { }
}