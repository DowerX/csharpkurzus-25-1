using System.Threading.Tasks;

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

  public async Task Create(string name, DateTime birthday, DateTime? death, string bio)
  {
    await dao.Create(new Person
    {
      Name = name,
      Birthday = birthday,
      Death = death,
      Bio = bio
    });
  }

  public Task<IEnumerable<Person>> List(Guid? id, string? name)
  {
    return dao.List(id, name);
  }

  public async Task Update(Person person)
  {
    await dao.Update(person);
  }

  public async Task Delete(Person person)
  {
    await dao.Delete(person);
  }
}