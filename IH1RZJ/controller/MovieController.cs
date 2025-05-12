using IH1RZJ.DAO;
using IH1RZJ.Model;

namespace IH1RZJ.Controller;

public class MovieController
{
  private readonly IMovieDAO movieDao;
  private readonly IReviewDAO reviewDAO;

  public MovieController(IMovieDAO movieDao, IReviewDAO reviewDAO)
  {
    this.movieDao = movieDao ?? throw new ArgumentNullException(nameof(movieDao));
    this.reviewDAO = reviewDAO ?? throw new ArgumentNullException(nameof(reviewDAO));
  }

  public async Task Create(string title, string description, DateTime releaseDate)
  {
    await movieDao.Create(new Movie
    {
      Title = title,
      Description = description,
      ReleaseDate = releaseDate
    });
  }

  public Task<IEnumerable<Movie>> List(Guid? id, string? title)
  {
    return movieDao.List(id, title);
  }

  public async Task Update(Movie movie)
  {
    await movieDao.Update(movie);
  }

  public async Task Delete(Movie movie)
  {
    foreach (Review review in await reviewDAO.List(null, movie.ID, null, null))
    {
      await reviewDAO.Delete(review);
    }
    await movieDao.Delete(movie);
  }
}