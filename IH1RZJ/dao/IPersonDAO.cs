using IH1RZJ.Model;

namespace IH1RZJ.DAO;

public interface IPersonDAO
{
  public void Create(Person person);
  public IEnumerable<Person> List(Guid? id, string? name);
  public void Update(Person person);
  public void Delete(Person person);
}