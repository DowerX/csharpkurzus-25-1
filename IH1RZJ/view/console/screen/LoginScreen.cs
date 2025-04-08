namespace IH1RZJ.View.ConsoleUI.Screen;

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
      else
      {
        // TODO: Login
      }
    },
    ref context)
  { }
}