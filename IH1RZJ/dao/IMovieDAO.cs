using IH1RZJ.Model;

namespace IH1RZJ.DAO;

public interface IMovieDAO
{
  public Task Create(Movie user);
  public Task<IEnumerable<Movie>> List(Guid? id, string? title);
  public Task Update(Movie user);
  public Task Delete(Movie user);
}