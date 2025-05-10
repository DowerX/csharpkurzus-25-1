using IH1RZJ.Controller;
using IH1RZJ.DAO;

using Terminal.Gui;

public class LoginWindow : Window
{
  private readonly UserController controller = new UserController(DAOFactory.Instance.UserDAO);

  public LoginWindow()
  {
    Title = "Login";

    var usernameLabel = new Label
    {
      Text = "Username:"
    };
    var usernameField = new TextField
    {
      X = Pos.Right(usernameLabel) + 1,
      Width = Dim.Fill()
    };

    var passwordLabel = new Label
    {
      Text = "Password:",
      Y = Pos.Bottom(usernameLabel) + 1
    };
    var passwordField = new TextField
    {
      X = Pos.Right(passwordLabel) + 1,
      Y = Pos.Bottom(usernameField) + 1,
      Width = Dim.Fill(),
      Secret = true
    };

    var loginButton = new Button
    {
      Text = "Login",
      Y = Pos.Bottom(passwordField) + 1,
      X = Pos.Center(),
    };
    loginButton.Clicked += async () =>
    {
      if (await controller.Login(
        usernameField.Text?.ToString() ?? string.Empty,
        passwordField.Text?.ToString() ?? string.Empty))
      {
        // MessageBox.Query("Logging In", "Login Succesful", "Ok");
        Application.Run<MainWindow>();
      }
      else
      {
        MessageBox.ErrorQuery("Logging In", "Incorrect username or password", "Ok");
      }
    };

    var backButton = new Button
    {
      Text = "Back",
      Y = Pos.Bottom(loginButton) + 1,
      X = Pos.Center(),
    };
    backButton.Clicked += () =>
    {
      Application.RequestStop();
    };

    Add(usernameLabel, usernameField,
      passwordLabel, passwordField,
      loginButton, backButton);
  }
}