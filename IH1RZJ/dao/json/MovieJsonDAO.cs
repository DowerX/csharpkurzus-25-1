using System.Text.Json;

using IH1RZJ.Model;
using IH1RZJ.Model.DTO;
using IH1RZJ.Model.DTO.Json;

namespace IH1RZJ.DAO.Json;

public class MovieJsonDAO : IMovieDAO, IDisposable
{
  private List<Movie> movies;
  private string path;

  public MovieJsonDAO(string path)
  {
    this.path = path;
    using FileStream stream = File.OpenRead(path);
    List<MovieJsonDTO>? movieDTOs = JsonSerializer.Deserialize<List<MovieJsonDTO>>(stream, Config.Instance.JsonOptions);

    if (movieDTOs == null)
    {
      throw new Exception("Failed to load users!");
    }

    movies = movieDTOs
      .AsParallel()
      .Select(dto => dto.ToDomain())
      .ToList();
  }

  public void Save()
  {
    string tempFile = Path.GetTempFileName();
    try
    {
      using FileStream stream = File.Create(tempFile);

      stream.Write(System.Text.Encoding.UTF8.GetBytes(
        JsonSerializer.Serialize(
          movies
            .AsParallel()
            .Select(user => user.ToDTO())
            .ToList(),
          Config.Instance.JsonOptions
      )));
      stream.Close();

      File.Move(tempFile, path, true);
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);
    }
  }

  public void Dispose()
  {
    Save();
  }

  public void Create(Movie movie)
  {
    movies.Add(movie);
  }

  public IEnumerable<Movie> List(Guid? id, string? title)
  {
    return movies
      .AsParallel()
      .Where(movie => id == null || movie.ID == id)
      .Where(movie => title == null || movie.Title == title)
      .ToList();
  }

  public void Update(Movie movie)
  {
    int index = movies.FindIndex(u => movie.ID == u.ID);
    movies[index] = movie;
    Save();
  }

  public void Delete(Movie movie)
  {
    movies.Remove(movie);
  }
}