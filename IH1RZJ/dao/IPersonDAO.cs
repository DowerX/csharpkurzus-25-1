using IH1RZJ.Model;

namespace IH1RZJ.DAO;

public interface IPersonDAO
{
  public Task Create(Person person);
  public Task<IEnumerable<Person>> List(Guid? id, string? name);
  public Task Update(Person person);
  public Task Delete(Person person);
}