using System.Text.Json;

using IH1RZJ.Model;
using IH1RZJ.Model.DTO.Json;
using IH1RZJ.Model.DTO;

namespace IH1RZJ.DAO.Json;

public class ReviewJsonDAO : IReviewDAO, IAsyncDisposable
{
  private List<Review> reviews;
  private string path;

  public ReviewJsonDAO(string path)
  {
    this.path = path;
    using FileStream stream = File.OpenRead(path);
    List<ReviewJsonDTO>? reviewDTOs = JsonSerializer.Deserialize<List<ReviewJsonDTO>>(stream, Config.JsonOptions);

    if (reviewDTOs == null)
    {
      throw new Exception("Failed to load reviews!");
    }

    reviews = reviewDTOs
      .AsParallel()
      .Select(dto => dto.ToDomain())
      .ToList();
  }

  public async Task Save()
  {
    string tempFile = Path.GetTempFileName();
    try
    {
      var dtoList = reviews
        .AsParallel()
        .Select(review => review.ToDTO())
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

  public async Task Create(Review review)
  {
    reviews.Add(review);
    await Save();
  }

  public async Task Delete(Review review)
  {
    reviews.Remove(review);
    await Save();
  }

  public Task<IEnumerable<Review>> List(Guid? id, Guid? movie, Guid? user, float? score)
  {
    var result = reviews
      .AsParallel()
      .Where(review => id == null || review.ID == id)
      .Where(review => movie == null || review.MovieID == movie)
      .Where(review => user == null || review.UserID == user)
      .Where(review => score == null || review.Score == score)
      .ToList();

    return Task.FromResult<IEnumerable<Review>>(result);
  }

  public async Task Update(Review review)
  {
    int index = reviews.FindIndex(u => review.ID == u.ID);
    if (index != -1)
    {
      reviews[index] = review;
      await Save();
    }
  }
}