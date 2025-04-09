using IH1RZJ.View.ConsoleUI.Screen;
using IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = System.Collections.Generic.Dictionary<string, object>;

namespace IH1RZJ.View.ConsoleUI;

public class ConsoleUI
{
  private Context context = new();

  private IScreen rootScreen;

  public ConsoleUI()
  {
    rootScreen = new TableScreen(
      new() {
        new StringRow("Login", (ref Context context)=>{
          new LoginScreen(ref context).Show();
        }),
        new StringRow("Register", (ref Context context)=>{
          new RegisterScreen(ref context).Show();
        }),
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