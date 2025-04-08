
using System.Text.Json;

using IH1RZJ.Model;
using IH1RZJ.Model.DTO.Json;
using IH1RZJ.Model.DTO;

namespace IH1RZJ.DAO.Json;

public class PersonJsonDAO : IPersonDAO, IDisposable
{
  private List<Person> people;
  private string path;

  public PersonJsonDAO(string path)
  {
    this.path = path;
    using FileStream stream = File.OpenRead(path);
    List<PersonJsonDTO>? peopleDTOs = JsonSerializer.Deserialize<List<PersonJsonDTO>>(stream, Config.Instance.JsonOptions);

    if (peopleDTOs == null)
    {
      throw new Exception("Failed to load users!");
    }

    people = peopleDTOs
      .AsParallel()
      .Select(dto => dto.ToDomain())
      .ToList();
  }

  public void Save()
  {
    string tempFile = Path.GetTempFileName();
    try
    {
      using FileStream stream = File.Create(tempFile);

      stream.Write(System.Text.Encoding.UTF8.GetBytes(
        JsonSerializer.Serialize(
          people
            .AsParallel()
            .Select(person => person.ToDTO())
            .ToList(),
          Config.Instance.JsonOptions
      )));
      stream.Close();

      File.Move(tempFile, path, true);
    }
    catch (Exception e)
    {
      Console.Error.WriteLine(e.Message);
    }
  }

  public void Dispose()
  {
    Save();
  }

  public void Create(Person person)
  {
    people.Add(person);
    Save();
  }

  public void Delete(Person person)
  {
    people.Remove(person);
    Save();
  }

  public IEnumerable<Person> List(Guid? id, string? name)
  {
    return people
      .AsParallel()
      .Where(person => id == null || person.ID == id)
      .Where(person => name == null || person.Name == name)
      .ToList();
  }

  public void Update(Person person)
  {
    int index = people.FindIndex(p => person.ID == p.ID);
    people[index] = person;
    Save();
  }
}