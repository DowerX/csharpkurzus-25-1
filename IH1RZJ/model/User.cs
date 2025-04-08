namespace IH1RZJ.Model;

public record class User
{
  public Guid ID { get; init; } = Guid.NewGuid();
  public required string Username { get; set; }
  public required string PasswordHash { get; set; }
  public required bool IsAdmin { get; set; }
}