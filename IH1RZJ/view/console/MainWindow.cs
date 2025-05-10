using IH1RZJ.Controller;
using IH1RZJ.DAO;

using Terminal.Gui;

public class MainWindow : Window
{
  private readonly UserController controller = new UserController(DAOFactory.Instance.UserDAO);

  public MainWindow()
  {
    Title = "Movie manager";

    var peopleButton = new Button
    {
      Text = "People",
    };
    peopleButton.Clicked += () => Application.Run<PeopleListWindow>();

    var moviesButton = new Button
    {
      Text = "Movies",
      Y = Pos.Bottom(peopleButton)
    };
    moviesButton.Clicked += () => Application.Run<Window>();

    var reviewsButton = new Button
    {
      Text = "Reviews",
      Y = Pos.Bottom(moviesButton)
    };
    reviewsButton.Clicked += () => Application.Run<Window>();

    var logoutButton = new Button
    {
      Text = "Logout",
      Y = Pos.Bottom(reviewsButton)
    };
    logoutButton.Clicked += () => Application.RequestStop();

    Add(peopleButton, moviesButton, reviewsButton, logoutButton);
  }
}