using System.ComponentModel.DataAnnotations;

namespace IH1RZJ.Model;

public record class Review
{
  public Guid ID { get; init; } = Guid.NewGuid();
  public required Guid MovieID { get; set; }
  public required Guid UserID { get; set; }

  [Range(0.0, 10.0, ErrorMessage = "Score out of range")]
  public required float Score { get; set; }
}