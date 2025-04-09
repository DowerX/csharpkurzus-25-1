namespace IH1RZJ.View.ConsoleUI.Screen;

using IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

public class MainScreen : TableScreen
{
  public MainScreen(ref Context context) :
  base(
    new() {
      new StringRow("People", (ref Context context)=> new PersonScreen(ref context).Show()),
      new StringRow("Movies", (ref Context context)=> new MovieMainScreen(ref context).Show()),
     },
    ref context)
  { }
}