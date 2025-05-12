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

  public async Task<IEnumerable<Movie>> List(Guid? id, string? title)
  {
    var movies = (await movieDao.List(id, title)).ToList();
    for (int i = 0; i < movies.Count(); i++)
    {
      movies[i].Score = (await reviewDAO.List(null, movies[i].ID, null, null))
      .AsParallel()
      .Select(review => review.Score)
      .DefaultIfEmpty(0)
      .Average();
    }
    return movies;
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