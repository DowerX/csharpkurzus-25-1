namespace IH1RZJ.View.ConsoleUI.Screen;

using IH1RZJ.DAO;
using IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

public class PersonScreen : TableScreen
{
  public PersonScreen(ref Context context) :
  base(
    DAOFactory.Instance.PersonDAO
      .List(null, null)
      .Select(person => (ITableRow)new StringRow(
        $"{person.Name}: {person.Birthday:yyyy/MM/dd}-{(person.Death != null ? person.Death?.ToString("yyyy/MM/dd") : "")}, {person.Bio}",
        (ref Context context) => { }))
      .ToList(),
    ref context)
  { }
}