namespace IH1RZJ.Model.DTO.Json;

public record class MovieJsonDTO
{
  public Guid ID { get; init; } = Guid.NewGuid();
  public required string Title { get; set; }
  public required string Description { get; set; }
  public required DateTime ReleaseDate { get; set; }
}