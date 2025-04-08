namespace IH1RZJ.View.ConsoleUI.Screen;

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
      else
      {
        // TODO: Register
      }
    },
    ref context)
  { }
}