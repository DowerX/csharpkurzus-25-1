using IH1RZJ.DAO;
using IH1RZJ.Model;

namespace IH1RZJ.Controller;

public class ReviewController
{
  private readonly IReviewDAO dao;

  public ReviewController(IReviewDAO dao)
  {
    this.dao = dao ?? throw new ArgumentNullException(nameof(dao));
  }

  public async Task Create(Guid movie, Guid user, float score)
  {
    await dao.Create(new Review
    {
      MovieID = movie,
      UserID = user,
      Score = score
    });
  }

  public Task<IEnumerable<Review>> List(Guid? id, Guid? movie, Guid? user, float? score)
  {
    return dao.List(id, movie, user, score);
  }

  public async Task Update(Review review)
  {
    await dao.Update(review);
  }

  public async Task Delete(Review review)
  {
    await dao.Delete(review);
  }
}