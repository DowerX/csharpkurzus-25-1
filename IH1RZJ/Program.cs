using System.Text.Json;
using System.Text.Json.Serialization;

using IH1RZJ.Controller;
using IH1RZJ.DAO.Json;
using IH1RZJ.Model;
using IH1RZJ.Model.DTO.Json;

using Microsoft.Extensions.Configuration;

namespace IH1RZJ;

internal class Program
{
    private static int Main(string[] args)
    {
        // load config
        {
            var configRoot = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "MOVIE_")
                .AddCommandLine(args)
                .Build();
            configRoot.Bind(Config.Instance);
        }

        // create daos
        using var userDao = new UserJsonDAO(Config.Instance.UserPath);

        // create controllers
        UserController userController = new UserController(userDao);

        // interface
        switch (Config.Instance.Operation)
        {
            case Opertaion.Print:
                return Print();
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
            if (user == null)
            {
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