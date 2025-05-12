using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.Model;

using Terminal.Gui;

public class MovieEditWindow : Window
{
  private readonly Movie movie;
  private readonly MovieController controller = new MovieController(DAOFactory.Instance.MovieDAO);

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
      Text = movie.Title,
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
      Text = movie.Description,
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
      Text = movie.ReleaseDate.ToString("yyyy/MM/dd") ?? "",
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
      movie.Title = (string)titleField.Text;

      if (DateTime.TryParseExact(
        (string)releaseDateField.Text,
        "yyyy/MM/dd", null,
        System.Globalization.DateTimeStyles.None,
        out DateTime parsedReleasDate))
      {
        movie.ReleaseDate = parsedReleasDate;
      }
      else
      {
        MessageBox.ErrorQuery("Releas date", "Failed to parse date", "OK");
        return;
      }

      movie.Description = (string)movie.Description;

      if ((await controller.List(movie.ID, null)).Count() == 0)
      {
        await controller.Create(movie.Title, movie.Description, movie.ReleaseDate);
      }
      else
      {
        await controller.Update(movie);
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
      await controller.Delete(movie);
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