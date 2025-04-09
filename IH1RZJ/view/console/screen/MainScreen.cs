namespace IH1RZJ.View.ConsoleUI.Screen;

using Context = Dictionary<string, object>;

public class MainScreen : TableScreen<string>
{
  public MainScreen(ref Context context) :
  base(
    new() { "People" },
    (string text, ref Context c) => text,
    (string text, ref Context c) =>
    {
      switch (text)
      {
        case "People":
          new PersonScreen(ref c).Show();
          break;
      }
    },
    ref context)
  { }
}