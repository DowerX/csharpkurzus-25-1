namespace IH1RZJ.View.ConsoleUI.Screen;

using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.Model;

using Context = Dictionary<string, object>;

public class PersonScreen : TableScreen<Person>
{
  public PersonScreen(ref Context context) :
  base(
    DAOFactory.Instance.PersonDAO.List(null, null).ToList(),
    (Person person, ref Context c) => $"{person.Name}: {person.Birthday:yyyy/MM/dd}-{(person.Death != null ? person.Death?.ToString("yyyy/MM/dd") : "")}, {person.Bio}",
    (Person person, ref Context c) => { },
    ref context)
  { }
}