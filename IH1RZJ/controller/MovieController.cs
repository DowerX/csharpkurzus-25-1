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

  public async Task Create(string title, string description, DateTime releaseDate)
  {
    await dao.Create(new Movie
    {
      Title = title,
      Description = description,
      ReleaseDate = releaseDate
    });
  }

  public Task<IEnumerable<Movie>> List(Guid? id, string? title)
  {
    return dao.List(id, title);
  }

  public async Task Update(Movie movie)
  {
    await dao.Update(movie);
  }

  public async Task Delete(Movie movie)
  {
    await dao.Delete(movie);
  }
}