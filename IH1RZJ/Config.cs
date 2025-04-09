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

  public JsonSerializerOptions JsonOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Converters = { new JsonStringEnumConverter() },
    WriteIndented = true
  };
}