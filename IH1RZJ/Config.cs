using System.Text.Json;
using System.Text.Json.Serialization;

namespace IH1RZJ;

public enum Opertaion
{
  Load,
  Print
}

public class Config
{
  private static Config? instance;

  public static Config Instance
  {
    get
    {
      if (instance == null)
      {
        instance = new();
      }

      return instance;
    }
  }

  public Opertaion Operation { get; set; } = Opertaion.Print;
  public string UserPath { get; set; } = Path.Combine(AppContext.BaseDirectory, "users.json");

  public JsonSerializerOptions JsonOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Converters = { new JsonStringEnumConverter() },
    WriteIndented = true
  };
}