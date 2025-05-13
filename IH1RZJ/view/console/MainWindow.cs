using IH1RZJ.Controller;
using IH1RZJ.DAO;

using Terminal.Gui;

public class MainWindow : Window
{
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
    moviesButton.Clicked += () => Application.Run<MoviesListWindow>();

    var logoutButton = new Button
    {
      Text = "Logout",
      Y = Pos.Bottom(moviesButton)
    };
    logoutButton.Clicked += () => Application.RequestStop();

    Add(peopleButton, moviesButton, logoutButton);
  }
}