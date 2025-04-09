using IH1RZJ.Controller;
using IH1RZJ.View.ConsoleUI.Screen;

using Context = System.Collections.Generic.Dictionary<string, object>;

namespace IH1RZJ.View.ConsoleUI;


public class ConsoleUI
{
  private Context context = new();

  private IScreen rootScreen;

  public ConsoleUI()
  {
    rootScreen = new TableScreen<string>(
      new() { "Login", "Register" },
      (string text, ref Context c) => text,
      (string text, ref Context c) =>
      {
        switch (text)
        {
          case "Login":
            new LoginScreen(ref context).Show();
            break;
          case "Register":
            new RegisterScreen(ref context).Show();
            break;
        }
      },
      ref context
    );
  }

  public void Show()
  {
    try
    {
      rootScreen.Show();

    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);
    }
  }
}