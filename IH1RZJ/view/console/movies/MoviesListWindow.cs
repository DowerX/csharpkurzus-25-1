using System.Data;

using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.Model;
using IH1RZJ.Utils;

using Terminal.Gui;

namespace IH1RZJ.View.Console;

public class MoviesListWindow : Window
{
  private readonly MovieController controller = new MovieController(
    DAOFactory.Instance.MovieDAO,
    DAOFactory.Instance.ReviewDAO,
    DAOFactory.Instance.PersonDAO,
    DAOFactory.Instance.AppearanceDAO);

  private IEnumerable<Movie>? data;
  private readonly TableView view;
  private bool descending = false;

  public MoviesListWindow()
  {
    Title = "Movies";

    var addButton = new Button
    {
      Text = "Add",
      Enabled = UserController.CurrentUser?.IsAdmin ?? false
    };
    addButton.Clicked += async () =>
    {
      Application.Run(new MovieEditWindow(new Movie
      {
        Title = "",
        Description = "",
        ReleaseDate = DateTime.Now
      }));
      await Update();
      Sort();
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
      if (data == null) return;
      var item = data.ToList()[args.Row];

      if (UserController.CurrentUser?.IsAdmin ?? false)
      {
        // admins edit
        Application.Run(new MovieEditWindow(item));
        await Update();
        Sort();
        Display();
      }
      else
      {
        // users view details
        Application.Run(new MovieDetailsWindow(item));
      }
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
    view.Table = new MoviesTable(data);
    view.Update();
    view.Redraw(Rect.Empty);
  }

  private void Sort()
  {
    data = data?.OrderBy(item => item.Title).If(descending, movies => movies.Reverse());
  }

  private void Search(string term)
  {
    data = data?.Where(movie => movie.Title.Contains(term));
  }

  private async Task Update()
  {
    try
    {
      data = await controller.List(null, null);
    }
    catch (Exception ex)
    {
      MessageBox.ErrorQuery("Error", $"An error occurred: {ex.Message}", "Ok");
    }
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

      column = new DataColumn();
      column.DataType = typeof(float);
      column.ColumnName = "Score";
      column.ReadOnly = false;
      Columns.Add(column);

      // populate rows
      foreach (var movie in movies)
      {
        DataRow row = NewRow();
        row["Title"] = movie.Title;
        row["Description"] = movie.Description;
        row["Release date"] = movie.ReleaseDate.ToString("yyyy/MM/dd");
        row["Score"] = movie.Score;
        Rows.Add(row);
      }
    }
  }
}