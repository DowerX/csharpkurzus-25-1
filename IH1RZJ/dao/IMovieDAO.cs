using IH1RZJ.Model;

namespace IH1RZJ.DAO;

public interface IMovieDAO
{
  public Task Create(Movie movie);
  public Task<IEnumerable<Movie>> List(Guid? id, string? title);
  public Task Update(Movie movie);
  public Task Delete(Movie movie);
}