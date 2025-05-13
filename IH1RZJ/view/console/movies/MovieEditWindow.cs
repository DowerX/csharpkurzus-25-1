using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.Model;

using Terminal.Gui;

public class MovieEditWindow : Window
{
  private readonly Movie movie;
  private readonly MovieController controller = new MovieController(
    DAOFactory.Instance.MovieDAO,
    DAOFactory.Instance.ReviewDAO,
    DAOFactory.Instance.PersonDAO,
    DAOFactory.Instance.AppearanceDAO);

  public MovieEditWindow(Movie movie)
  {
    this.movie = movie;

    Title = "Edit movie";

    var titleLabel = new Label
    {
      Text = "Title:",
    };
    var titleField = new TextField
    {
      Text = this.movie.Title,
      X = Pos.Right(titleLabel),
      Width = Dim.Fill()
    };

    var descriptionLabel = new Label
    {
      Text = "Description:",
      Y = Pos.Bottom(titleLabel)
    };
    var descriptionField = new TextField
    {
      Text = this.movie.Description,
      X = Pos.Right(descriptionLabel),
      Y = Pos.Bottom(titleField),
      Width = Dim.Fill()
    };

    var releaseDateLabel = new Label
    {
      Text = "Release date:",
      Y = Pos.Bottom(descriptionLabel)
    };
    var releaseDateField = new TextField
    {
      Text = this.movie.ReleaseDate.ToString("yyyy/MM/dd") ?? "",
      X = Pos.Right(releaseDateLabel),
      Y = Pos.Bottom(descriptionField),
      Width = Dim.Fill()
    };


    var okButton = new Button
    {
      Text = "OK",
      X = Pos.Center(),
      Y = Pos.Bottom(releaseDateLabel)
    };
    okButton.Clicked += async () =>
    {
      this.movie.Title = (string)titleField.Text;

      if (DateTime.TryParseExact(
        (string)releaseDateField.Text,
        "yyyy/MM/dd", null,
        System.Globalization.DateTimeStyles.None,
        out DateTime parsedReleasDate))
      {
        this.movie.ReleaseDate = parsedReleasDate;
      }
      else
      {
        MessageBox.ErrorQuery("Release date", "Failed to parse date", "OK");
        return;
      }

      this.movie.Description = (string)descriptionField.Text;

      if (!(await controller.List(movie.ID, null)).Any())
      {
        await controller.Create(this.movie.Title, this.movie.Description, this.movie.ReleaseDate);
      }
      else
      {
        await controller.Update(this.movie);
      }

      Application.RequestStop();
    };

    var deleteButton = new Button
    {
      Text = "Delete",
      X = Pos.Center(),
      Y = Pos.Bottom(okButton)
    };
    deleteButton.Clicked += async () =>
    {
      await controller.Delete(this.movie);
      Application.RequestStop();
    };

    var cancelButton = new Button
    {
      Text = "Cancel",
      X = Pos.Center(),
      Y = Pos.Bottom(deleteButton)
    };
    cancelButton.Clicked += () => Application.RequestStop();

    Add(titleLabel, titleField,
      descriptionLabel, descriptionField,
      releaseDateLabel, releaseDateField,
      okButton, deleteButton, cancelButton);
  }
}