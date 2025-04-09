namespace IH1RZJ.View.ConsoleUI.Screen;

using Context = Dictionary<string, object>;

public class MovieMainScreen : TableScreen<string>
{
  public MovieMainScreen(ref Context context) :
  base(
    new() { "List", "Search", "Ordering" },
    (string text, ref Context c) =>
    {
      if (text == "Ordering")
      {
        return $"{text}: {(Ordering)c["Ordering"]}";
      }
      else
      {
        return text;
      }
    },
    (string text, ref Context c) =>
    {
      switch (text)
      {
        case "List":
          break;
        case "Search":
          break;
        case "Ordering":
          c["Ordering"] = (Ordering)c["Ordering"] == Ordering.Ascending ? Ordering.Descending : Ordering.Ascending;
          break;
      }
    },
    ref context)
  {
    context["Ordering"] = Ordering.Ascending;
  }
}