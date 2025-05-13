using IH1RZJ.DAO;
using IH1RZJ.Model;

namespace IH1RZJ.Controller;

public class MovieController
{
  private readonly IMovieDAO movieDao;
  private readonly IReviewDAO reviewDAO;
  private readonly IPersonDAO personDAO;
  private readonly IAppearanceDAO appearanceDAO;

  public MovieController(IMovieDAO movieDao,
                         IReviewDAO reviewDAO,
                         IPersonDAO personDAO,
                         IAppearanceDAO appearanceDAO)
  {
    this.movieDao = movieDao ?? throw new ArgumentNullException(nameof(movieDao));
    this.reviewDAO = reviewDAO ?? throw new ArgumentNullException(nameof(reviewDAO));
    this.personDAO = personDAO ?? throw new ArgumentNullException(nameof(personDAO));
    this.appearanceDAO = appearanceDAO ?? throw new ArgumentNullException(nameof(appearanceDAO));
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

  public async Task<IEnumerable<Person>> GetCast(Guid movie)
  {
    var appearances = await appearanceDAO.List(null, movie, null, null);
    IEnumerable<Person> cast = new List<Person>();

    foreach (Appearance appearance in appearances)
    {
      Person person = (await personDAO.List(appearance.PersonID, null)).Single();
      person.Role = appearance.Role;
      cast.Append(person);
    }

    return cast;
  }

  public async Task LeaveReview(Guid movie, Guid user, float score)
  {
    if (score < 0 || score > 10)
    {
      throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 10.");
    }

    Review review;
    try
    {
      review = (await reviewDAO.List(null, movie, user, null)).Single();
      review.Score = score;
      await reviewDAO.Update(review);
    }
    catch (InvalidOperationException)
    {
      review = new Review
      {
        MovieID = movie,
        UserID = user,
        Score = score
      };
      await reviewDAO.Create(review);
    }
  }

  public async Task<float> GetUserScore(Guid moive, Guid user)
  {
    var reviews = await reviewDAO.List(null, moive, user, null);
    return reviews.Any() ? reviews.Single().Score : 0;
  }
}