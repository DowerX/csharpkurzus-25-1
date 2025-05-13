using System.Text.Json;
using System.Text.Json.Serialization;

namespace IH1RZJ;

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

  public string UsersPath { get; set; } = Path.Combine(AppContext.BaseDirectory, "users.json");
  public string PeoplePath { get; set; } = Path.Combine(AppContext.BaseDirectory, "people.json");
  public string MoviesPath { get; set; } = Path.Combine(AppContext.BaseDirectory, "movies.json");
  public string ReviewsPath { get; set; } = Path.Combine(AppContext.BaseDirectory, "reviews.json");
  public string AppearancesPath { get; set; } = Path.Combine(AppContext.BaseDirectory, "appearances.json");

  public static JsonSerializerOptions JsonOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Converters = { new JsonStringEnumConverter() },
    WriteIndented = true
  };
}