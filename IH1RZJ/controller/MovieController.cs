using IH1RZJ.DAO;
using IH1RZJ.Model;

namespace IH1RZJ.Controller;

public class MovieController
{
  private readonly IMovieDAO dao;

  public MovieController(IMovieDAO dao)
  {
    this.dao = dao ?? throw new ArgumentNullException(nameof(dao));
  }

  public void Create(string title, string description, DateTime releaseDate)
  {
    dao.Create(new Movie
    {
      Title = title,
      Description = description,
      ReleaseDate = releaseDate
    });
  }

  public IEnumerable<Movie> List(Guid? id, string? title)
  {
    return dao.List(id, title);
  }

  public void Update(Movie movie)
  {
    dao.Update(movie);
  }

  public void Delete(Movie movie)
  {
    dao.Delete(movie);
  }
}