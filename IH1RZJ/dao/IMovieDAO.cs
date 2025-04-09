using IH1RZJ.Model;

namespace IH1RZJ.DAO;

public interface IMovieDAO
{
  public void Create(Movie user);
  public IEnumerable<Movie> List(Guid? id, string? title);
  public void Update(Movie user);
  public void Delete(Movie user);
}