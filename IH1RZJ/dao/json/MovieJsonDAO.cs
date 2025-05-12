using System.Text.Json;

using IH1RZJ.Model;
using IH1RZJ.Model.DTO;
using IH1RZJ.Model.DTO.Json;

namespace IH1RZJ.DAO.Json;

public class MovieJsonDAO : IMovieDAO, IAsyncDisposable
{
  private List<Movie> movies;
  private string path;

  public MovieJsonDAO(string path)
  {
    this.path = path;
    using FileStream stream = File.OpenRead(path);
    List<MovieJsonDTO>? movieDTOs = JsonSerializer.Deserialize<List<MovieJsonDTO>>(stream, Config.JsonOptions);

    if (movieDTOs == null)
    {
      throw new Exception("Failed to load users!");
    }

    movies = movieDTOs
      .AsParallel()
      .Select(dto => dto.ToDomain())
      .ToList();
  }

  public async Task Save()
  {
    string tempFile = Path.GetTempFileName();
    try
    {
      var dtoList = movies
        .AsParallel()
        .Select(movie => movie.ToDTO())
        .ToList();

      await using FileStream stream = File.Create(tempFile);
      await JsonSerializer.SerializeAsync(stream, dtoList, Config.JsonOptions);
      stream.Close();

      File.Move(tempFile, path, true);
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);
    }
  }

  public async ValueTask DisposeAsync()
  {
    await Save();
  }

  public async Task Create(Movie movie)
  {
    movies.Add(movie);
    await Save();
  }

  public Task<IEnumerable<Movie>> List(Guid? id, string? title)
  {
    var result = movies
      .Where(movie => id == null || movie.ID == id)
      .Where(movie => title == null || movie.Title == title)
      .ToList();

    return Task.FromResult<IEnumerable<Movie>>(result);
  }

  public async Task Update(Movie movie)
  {
    int index = movies.FindIndex(u => movie.ID == u.ID);
    if (index != -1)
    {
      movies[index] = movie;
      await Save();
    }
  }

  public async Task Delete(Movie movie)
  {
    movies.Remove(movie);
    await Save();
  }
}