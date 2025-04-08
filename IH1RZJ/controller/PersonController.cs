using IH1RZJ.DAO;
using IH1RZJ.Model;

namespace IH1RZJ.Controller;

public class PersonController
{
  private readonly IPersonDAO dao;

  public PersonController(IPersonDAO dao)
  {
    this.dao = dao ?? throw new ArgumentNullException(nameof(dao));
  }

  public void Create(string name, DateTime birthday, DateTime? death, string bio)
  {
    dao.Create(new Person
    {
      Name = name,
      Birthday = birthday,
      Death = death,
      Bio = bio
    });
  }

  public IEnumerable<Person> List(Guid? id, string? name)
  {
    return dao.List(id, name);
  }

  public void Update(Person person)
  {
    dao.Update(person);
  }

  public void Delete(Person person)
  {
    dao.Delete(person);
  }
}