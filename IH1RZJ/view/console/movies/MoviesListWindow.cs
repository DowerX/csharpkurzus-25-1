using System.Data;

using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.Model;

using Terminal.Gui;

public class MoviesListWindow : Window
{
  private readonly MovieController controller = new MovieController(DAOFactory.Instance.MovieDAO);

  private IEnumerable<Movie>? data;
  private readonly TableView view;

  public MoviesListWindow()
  {
    Title = "Movies";

    var addButton = new Button
    {
      Text = "Add",
    };
    addButton.Clicked += async () =>
    {
      Application.Run(new MovieEditWindow(new Movie
      {
        Title = "",
        Description = "",
        ReleaseDate = DateTime.Now
      }));
      await update();
    };

    view = new TableView
    {
      X = 0,
      Y = Pos.Bottom(addButton),
      Width = Dim.Fill(),
      Height = Dim.Fill()
    };
    view.CellActivationKey = Key.Enter;
    view.CellActivated += async args =>
    {
      if (data == null) return;
      var item = data.ToList()[args.Row];

      Application.Run(new MovieEditWindow(item));
      await update();
    };

    Add(addButton, view);

    Task.Run(update);
  }

  private async Task update()
  {
    data = await controller.List(null, null);
    view.Table = new MoviesTable(data);
    view.Update();
    view.Redraw(Rect.Empty);
  }

  class MoviesTable : DataTable
  {
    public MoviesTable(IEnumerable<Movie> movies)
    {
      // setup schema
      DataColumn column;

      column = new DataColumn();
      column.DataType = typeof(string);
      column.ColumnName = "Title";
      column.ReadOnly = false;
      Columns.Add(column);

      column = new DataColumn();
      column.DataType = typeof(string);
      column.ColumnName = "Description";
      column.ReadOnly = false;
      Columns.Add(column);

      column = new DataColumn();
      column.DataType = typeof(string);
      column.ColumnName = "Release date";
      column.ReadOnly = false;
      Columns.Add(column);

      // populate rows
      foreach (var movie in movies)
      {
        DataRow row = NewRow();
        row["Title"] = movie.Title;
        row["Description"] = movie.Description;
        row["Release date"] = movie.ReleaseDate.ToString("yyyy/MM/dd");
        Rows.Add(row);
      }
    }
  }
}