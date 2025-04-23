namespace IH1RZJ.View.ConsoleUI.Screen;

using System.Globalization;

using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.Model;
using IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

public class MovieEditScreen : TableScreen
{
  public MovieEditScreen(ref Context context, Movie movie) :
  base(
    new() {
      new CustomRow<string>(
        "Title",
        (string text, ref Context context) => $"{text}: {((Movie)context["movie"]).Title}",
        (ref string text, ref Context context) => {
          Console.Clear();
          ((Movie)context["movie"]).Title = Console.ReadLine();
          new MovieController(DAOFactory.Instance.MovieDAO).Update((Movie)context["movie"]);
        }),
      new CustomRow<string>(
        "Description",
        (string text, ref Context context) => $"{text}: {((Movie)context["movie"]).Description}",
        (ref string text, ref Context context) => {
          Console.Clear();
          ((Movie)context["movie"]).Description = Console.ReadLine();
          new MovieController(DAOFactory.Instance.MovieDAO).Update((Movie)context["movie"]);
        }),
      new CustomRow<string>(
        "Release date",
        (string text, ref Context context) => $"{text}: {((Movie)context["movie"]).ReleaseDate:yyyy/MM/dd}",
        (ref string text, ref Context context) => {
          Console.Clear();
          DateTime result;
          if (DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd",null, DateTimeStyles.None, out result)) {
            ((Movie)context["movie"]).ReleaseDate = result;
            new MovieController(DAOFactory.Instance.MovieDAO).Update((Movie)context["movie"]);
          } else {
            Console.Clear();
            Console.WriteLine("Wrong format!");
          }
        }),
    }, ref context
  )
  {
    context["movie"] = movie;
  }
}