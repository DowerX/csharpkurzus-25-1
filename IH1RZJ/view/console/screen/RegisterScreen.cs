namespace IH1RZJ.View.ConsoleUI.Screen;

using IH1RZJ.Controller;
using IH1RZJ.DAO;

using Context = Dictionary<string, object>;

public class RegisterScreen : TableScreen<string>
{
  public RegisterScreen(ref Context context) :
  base(
    new() { "Username", "Password", "Repeat password", "Register" },
    (string text, ref Context c) =>
    {
      switch (text)
      {
        case "Username":
          return $"{text}: {(c.ContainsKey(text) ? c[text] : "")}";
        case "Password":
        case "Repeat password":
          return $"{text}: {(c.ContainsKey(text) ? string.Concat(Enumerable.Repeat("*", ((string)c[text]).Length)) : "")}";
        default:
          return text;
      }
    },
    (string text, ref Context c) =>
    {
      Console.Clear();
      if (text != "Register")
      {
        Console.Write(text + ": ");
        c[text] = Console.ReadLine();
      }
      else if (c.ContainsKey("Username") && c.ContainsKey("Password") && c.ContainsKey("Repeat password"))
      {
        if (new UserController(DAOFactory.Instance.UserDAO).Register((string)c["Username"], (string)c["Password"], (string)c["Repeat password"]))
        {
          Console.WriteLine("Succesful register! Press ENTER...");
          Console.ReadLine();
        }
        else
        {
          Console.WriteLine("Failed register! Press ENTER...");
          Console.ReadLine();
        }
      }
    },
    ref context)
  { }
}