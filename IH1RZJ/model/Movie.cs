namespace IH1RZJ.Model;

public record class Movie
{
  public Guid ID { get; init; } = Guid.NewGuid();
  public required string Title { get; set; }
  public required string Description { get; set; }
  public required DateTime ReleaseDate { get; set; }
  public float Score { get; set; } = 0f;
}