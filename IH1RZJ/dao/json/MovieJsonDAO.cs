using System.Text.Json;

using IH1RZJ.Model;
using IH1RZJ.Model.DTO;
using IH1RZJ.Model.DTO.Json;

namespace IH1RZJ.DAO.Json;

public class MovieJsonDAO : IMovieDAO, IAsyncDisposable
{
  private readonly List<Movie> movies;
  private readonly string path;
  private readonly Lock lockObj = new();

  public MovieJsonDAO(string path)
  {
    this.path = path;
    using FileStream stream = File.OpenRead(path);
    List<MovieJsonDTO>? movieDTOs = JsonSerializer.Deserialize<List<MovieJsonDTO>>(stream, Config.JsonOptions);

    if (movieDTOs == null)
    {
      throw new Exception("Failed to load movies!");
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
      List<MovieJsonDTO> dtoList;
      lock (lockObj)
      {
        dtoList = movies
          .AsParallel()
          .Select(movie => movie.ToDTO())
          .ToList();
      }
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
    lock (lockObj)
    {
      movies.Add(movie);
    }
    await Save();
  }

  public Task<IEnumerable<Movie>> List(Guid? id, string? title)
  {
    List<Movie> result;

    lock (lockObj)
    {
      result = movies
        .Where(movie => id == null || movie.ID == id)
        .Where(movie => title == null || movie.Title == title)
        .ToList();
    }

    return Task.FromResult<IEnumerable<Movie>>(result);
  }

  public async Task Update(Movie movie)
  {
    int index = -1;
    lock (lockObj)
    {
      index = movies.FindIndex(u => movie.ID == u.ID);
      if (index != -1)
      {
        movies[index] = movie;
      }
    }
    
    if (index != -1) await Save();
  }

  public async Task Delete(Movie movie)
  {
    lock (lockObj)
    {
      movies.Remove(movie);
    }
    await Save();
  }
}