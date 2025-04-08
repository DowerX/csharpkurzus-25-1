namespace IH1RZJ.Model;

public enum Role
{
  Actor,
  Director,
  Producer
}

public record class Appearance
{
  public Guid ID { get; init; } = Guid.NewGuid();
  public required Guid MovieID { get; set; }
  public required Guid PersonID { get; set; }
  public required Role Role;
}