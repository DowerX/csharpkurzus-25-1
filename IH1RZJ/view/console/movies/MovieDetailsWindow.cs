using System.Data;

using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.Model;
using IH1RZJ.Utils;

using Terminal.Gui;

public class MovieDetailsWindow : Window
{
  private readonly MovieController controller = new MovieController(
    DAOFactory.Instance.MovieDAO,
    DAOFactory.Instance.ReviewDAO,
    DAOFactory.Instance.PersonDAO,
    DAOFactory.Instance.AppearanceDAO);

  private IEnumerable<Person>? data;
  private readonly Movie movie;
  private readonly TableView view;
  private readonly TextField yourScoreField;
  private bool descending = false;

  public MovieDetailsWindow(Movie movie)
  {
    this.movie = movie;
    Title = $"Details - {movie.Title}";

    var titleLabel = new Label
    {
      Text = $"Title: {movie.Title}",
    };

    var releaseDateLabel = new Label
    {
      Text = $"Release date: {movie.ReleaseDate:yyyy/MM/dd}",
      Y = Pos.Bottom(titleLabel)
    };

    var descriptionLabel = new Label
    {
      Text = $"Description: {movie.Description}",
      Y = Pos.Bottom(releaseDateLabel)
    };

    var scoreLabel = new Label
    {
      Text = $"Score: {movie.Score}",
      Y = Pos.Bottom(descriptionLabel)
    };

    var yourScoreLabel = new Label
    {
      Text = $"Your score:",
      X = Pos.Right(scoreLabel) + 1,
      Y = Pos.Bottom(descriptionLabel)
    };
    yourScoreField = new TextField
    {
      Width = 10,
      X = Pos.Right(yourScoreLabel),
      Y = Pos.Bottom(descriptionLabel)
    };
    var yourScoreButton = new Button
    {
      Text = "Set",
      X = Pos.Right(yourScoreField) + 1,
      Y = Pos.Bottom(descriptionLabel)
    };
    yourScoreButton.Clicked += async () =>
    {
      float score;
      if (float.TryParse((string)yourScoreField.Text, out score))
      {
        await controller.LeaveReview(movie.ID, UserController.CurrentUser.ID, score);
      }
      else
      {
        MessageBox.ErrorQuery("Leave review", "Failed to parse score.", "OK");
      }
    };

    var sortButton = new Button
    {
      Text = "Ascending",
      Y = Pos.Bottom(scoreLabel)
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
      X = Pos.Right(sortButton),
      Y = Pos.Bottom(scoreLabel)
    };
    var searchField = new TextField
    {
      X = Pos.Right(searchButton),
      Y = Pos.Bottom(scoreLabel),
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

    Add(titleLabel,
      releaseDateLabel,
      descriptionLabel,
      scoreLabel, yourScoreLabel, yourScoreField, yourScoreButton,
      sortButton, searchButton, searchField,
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
    data = await controller.GetCast(movie.ID);
    yourScoreField.Text = (await controller.GetUserScore(movie.ID, UserController.CurrentUser.ID)).ToString();
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
      column.ColumnName = "Role";
      column.ReadOnly = false;
      Columns.Add(column);

      // populate rows
      foreach (var person in people)
      {
        DataRow row = NewRow();
        row["Name"] = person.Name;
        row["Role"] = person.Role;
        Rows.Add(row);
      }
    }
  }
}