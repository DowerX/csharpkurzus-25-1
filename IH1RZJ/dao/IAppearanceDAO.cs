using IH1RZJ.Model;

namespace IH1RZJ.DAO;

public interface IAppearanceDAO
{
  public Task Create(Appearance apperance);
  public Task<IEnumerable<Appearance>> List(Guid? id, Guid? movie, Guid? person, Role? role);
  public Task Update(Appearance apperance);
  public Task Delete(Appearance apperance);
}