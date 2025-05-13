using System.Data;

using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.Model;
using IH1RZJ.Utils;

using Terminal.Gui;

public class PeopleListWindow : Window
{
  private readonly PersonController controller = new PersonController(DAOFactory.Instance.PersonDAO);

  private IEnumerable<Person>? data;
  private readonly TableView view;
  private bool descending = false;

  public PeopleListWindow()
  {
    Title = "People";

    var addButton = new Button
    {
      Text = "Add",
      Enabled = UserController.CurrentUser?.IsAdmin ?? false
    };
    addButton.Clicked += async () =>
    {
      Application.Run(new PersonEditWindow(new Person
      {
        Name = "",
        Birthday = DateTime.Now,
        Bio = ""
      }));
      await Update();
      Display();
    };

    var sortButton = new Button
    {
      Text = "Ascending",
      X = Pos.Right(addButton)
    };
    sortButton.Clicked += () =>
    {
      descending = !descending;
      sortButton.Text = descending ? "Descending" : "Ascending";

      Sort();
      Display();
    };

    var searchButton = new Button
    {
      Text = "Search",
      Y = Pos.Bottom(addButton)
    };
    var searchField = new TextField
    {
      X = Pos.Right(searchButton),
      Y = Pos.Bottom(addButton),
      Width = Dim.Fill()
    };
    searchButton.Clicked += async () =>
    {
      await Update();
      Search((string)searchField.Text);
      Sort();
      Display();
    };

    view = new TableView
    {
      X = 0,
      Y = Pos.Bottom(searchButton),
      Width = Dim.Fill(),
      Height = Dim.Fill()
    };
    view.CellActivationKey = Key.Enter;
    view.CellActivated += async args =>
    {
      if (!(UserController.CurrentUser?.IsAdmin ?? false)) return;
      if (data == null) return;
      var item = data.ToList()[args.Row];

      Application.Run(new PersonEditWindow(item));
      await Update();
      Sort();
      Display();
    };

    Add(addButton, sortButton,
      searchButton, searchField,
      view);

    Task.Run(async () =>
    {
      await Update();
      Sort();
      Display();
    });
  }

  private void Display()
  {
    if (data == null) return;
    view.Table = new PeopleTable(data);
    view.Update();
    view.Redraw(Rect.Empty);
  }

  private void Sort()
  {
    data = data?.OrderBy(item => item.Name).If(descending, people => people.Reverse());
  }

  private void Search(string term)
  {
    data = data?.Where(person => person.Name.Contains(term));
  }

  private async Task Update()
  {
    data = await controller.List(null, null);
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