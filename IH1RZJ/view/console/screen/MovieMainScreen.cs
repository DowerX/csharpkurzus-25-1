namespace IH1RZJ.View.ConsoleUI.Screen;

using IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

public class MovieMainScreen : TableScreen
{
  public MovieMainScreen(ref Context context) :
  base(
    new() {
      new StringRow("List", (ref Context context)=>{}),
      new StringRow("Search", (ref Context context)=>{}),
      new CustomRow<string>("Ordering",
        (string text, ref Context context) => $"{text}: {(Ordering)context[text]}",
        (ref string text, ref Context context) => { context[text] = (Ordering)context[text] == Ordering.Ascending ? Ordering.Descending : Ordering.Ascending; }),
    },
    ref context)
  {
    context["Ordering"] = Ordering.Ascending;
  }
}