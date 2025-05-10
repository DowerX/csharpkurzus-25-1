using System.Data;

using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.Model;

using Terminal.Gui;

public class PeopleListWindow : Window
{
  private readonly PersonController controller = new PersonController(DAOFactory.Instance.PersonDAO);

  private IEnumerable<Person>? data;
  private readonly TableView view;

  public PeopleListWindow()
  {
    Title = "People";

    view = new TableView
    {
      X = 0,
      Y = 0,
      Width = Dim.Fill(),
      Height = Dim.Fill()
    };
    view.CellActivationKey = Key.Enter;
    view.CellActivated += async args =>
    {
      if (data == null) return;
      var item = data.ToList()[args.Row];

      Application.Run(new PersonEditWindow(item));
      await update();
    };

    Add(view);

    Task.Run(update);
  }

  private async Task update()
  {
    data = await controller.List(null, null);
    view.Table = new PeopleTable(data);
    view.Update();
  }

  class PeopleTable : DataTable
  {
    public PeopleTable(IEnumerable<Person> people)
    {
      // setup schema
      DataColumn column;

      column = new DataColumn();
      column.DataType = typeof(string);
      column.ColumnName = "Name";
      column.ReadOnly = false;
      Columns.Add(column);

      column = new DataColumn();
      column.DataType = typeof(string);
      column.ColumnName = "Birthday";
      column.ReadOnly = false;
      Columns.Add(column);

      column = new DataColumn();
      column.DataType = typeof(string);
      column.ColumnName = "Death";
      column.ReadOnly = false;
      Columns.Add(column);

      column = new DataColumn();
      column.DataType = typeof(string);
      column.ColumnName = "Bio";
      column.ReadOnly = false;
      Columns.Add(column);

      // populate rows
      foreach (var person in people)
      {
        DataRow row = NewRow();
        row["Name"] = person.Name;
        row["Birthday"] = person.Birthday.ToString("yyyy/MM/dd");
        row["Death"] = person.Death?.ToString("yyyy/MM/dd") ?? "-";
        row["Bio"] = person.Bio;
        Rows.Add(row);
      }
    }
  }
}