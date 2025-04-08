using System.Text.Json;
using System.Text.Json.Serialization;

using IH1RZJ.Model;
using IH1RZJ.Model.DTO.Json;

using Microsoft.Extensions.Configuration;

namespace IH1RZJ;

internal class Program
{
    private static int Main(string[] args)
    {
        // load config
        Config config = new();
        {
            var configRoot = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "MOVIE_")
                .AddCommandLine(args)
                .Build();
            configRoot.Bind(config);
        }

        switch (config.Operation)
        {
            case Opertaion.Print:
                return Print();
            case Opertaion.Load:
                return Load(config.Path);
        }

        return 0;
    }

    static int Print()
    {
        User user = new User
        {
            Username = "testuser1",
            PasswordHash = "hash",
            IsAdmin = true
        };
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = true
        };
        Console.WriteLine(JsonSerializer.Serialize(user, options));
        return 0;
    }

    static int Load(string path)
    {
        try
        {
            using FileStream stream = File.OpenRead(path);
            JsonSerializerOptions options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() },
                WriteIndented = true
            };
            UserJsonDTO? user = JsonSerializer.Deserialize<UserJsonDTO>(stream, options);
            if (user == null) {
                Console.Error.WriteLine("Failed to deserialize!");
                return -1;
            }
            Console.WriteLine(user);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return -1;
        }
        return 0;
    }
}