using IH1RZJ.Model;

namespace IH1RZJ.DAO;

public interface IReviewDAO
{
  public Task Create(Review review);
  public Task<IEnumerable<Review>> List(Guid? id, Guid? movie, Guid? user, float? score);
  public Task Update(Review review);
  public Task Delete(Review review);
}