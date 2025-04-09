namespace IH1RZJ.View.ConsoleUI.Screen;

using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.Utils;
using IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

public class MovieMainScreen : TableScreen
{
  public MovieMainScreen(ref Context context) :
  base(
    new() {
      new StringRow("List", (ref Context context)=>{
        new MovieListScreen(ref context,
          new MovieController(DAOFactory.Instance.MovieDAO)
          .List(null, null)
          .OrderBy(movie => movie.Title)
          .If((Ordering)context["Ordering"] == Ordering.Descending, s => s.Reverse())
          .ToList())
          .Show();
      }),
      new StringRow("Search", (ref Context context) => {
        Console.Clear();
        Console.Write("Title: ");
        string title = Console.ReadLine();
        new MovieListScreen(ref context,
          new MovieController(DAOFactory.Instance.MovieDAO)
          .List(null, title)
          .OrderBy(movie => movie.Title)
          .If((Ordering)context["Ordering"] == Ordering.Descending, s => s.Reverse())
          .ToList())
          .Show();
      }),
      new CustomRow<string>("Ordering",
        (string text, ref Context context) => $"{text}: {(Ordering)context[text]}",
        (ref string text, ref Context context) => { context[text] = (Ordering)context[text] == Ordering.Ascending ? Ordering.Descending : Ordering.Ascending; }),
    },
    ref context)
  {
    context["Ordering"] = Ordering.Ascending;
  }
}