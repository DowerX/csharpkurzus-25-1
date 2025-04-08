using System.Text.Json.Serialization;

namespace IH1RZJ.Model.DTO.Json;

public record class AppearanceJsonDTO
{
  public Guid ID { get; init; } = Guid.NewGuid();
  public required Guid MovieID { get; set; }
  public required Guid PersonID { get; set; }
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public required Role Role;
}