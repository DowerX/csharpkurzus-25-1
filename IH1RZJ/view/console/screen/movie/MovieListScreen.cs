namespace IH1RZJ.View.ConsoleUI.Screen;

using IH1RZJ.Model;
using IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

public class MovieListScreen : TableScreen
{
  public MovieListScreen(ref Context context, IEnumerable<Movie> movies) :
  base(
    movies
      .Select(movie => (ITableRow)new CustomRow<Movie>(movie,
        (Movie movie, ref Context context) => $"{movie.Title} - {movie.ReleaseDate:yyyy/MM/dd}, {movie.Description}",
        (ref Movie movie, ref Context context) =>
        {
          new MovieEditScreen(ref context, movie).Show();
          movie = (Movie)context["movie"];
        }))
      .ToList(),
    ref context)
  { }
}