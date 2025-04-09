namespace IH1RZJ.View.ConsoleUI.Screen;

using IH1RZJ.Model;
using IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

public class MovieListScreen : TableScreen
{
  public MovieListScreen(ref Context context, IEnumerable<Movie> movies) :
  base(
    movies
      .Select(movie => (ITableRow)new StringRow(
        $"{movie.Title} - {movie.ReleaseDate:yyyy/MM/dd}, {movie.Description}",
        (ref Context context) => { }))
      .ToList(),
    ref context)
  { }
}