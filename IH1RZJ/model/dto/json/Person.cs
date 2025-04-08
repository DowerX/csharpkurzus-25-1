namespace IH1RZJ.Model.DTO.Json;

public record class PersonJsonDTO
{
  public Guid ID { get; init; } = Guid.NewGuid();
  public required string Name { get; set; }
  public required DateTime Birthday { get; set; }
  public DateTime? Death { get; set; }
  public required string Bio { get; set; }
}