using IH1RZJ.Controller;
using IH1RZJ.DAO;

using Terminal.Gui;

public class RegisterWindow : Window
{
  private readonly UserController controller = new UserController(DAOFactory.Instance.UserDAO);

  public RegisterWindow()
  {
    Title = "Register";

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

    var passwordRepeatLabel = new Label
    {
      Text = "Repeat Password:",
      Y = Pos.Bottom(passwordField) + 1
    };
    var passwordRepeatField = new TextField
    {
      X = Pos.Right(passwordRepeatLabel) + 1,
      Y = Pos.Bottom(passwordField) + 1,
      Width = Dim.Fill(),
      Secret = true
    };

    var registerButton = new Button
    {
      Text = "Register",
      Y = Pos.Bottom(passwordRepeatField) + 1,
      X = Pos.Center(),
    };
    registerButton.Clicked += async () =>
    {
      if (await controller.Register(
        usernameField.Text?.ToString() ?? string.Empty,
        passwordField.Text?.ToString() ?? string.Empty,
        passwordRepeatField.Text?.ToString() ?? string.Empty))
      {
        MessageBox.Query("Register", "Register Succesful", "Ok");
      }
      else
      {
        MessageBox.ErrorQuery("Register", "Incorrect username or password", "Ok");
      }
    };

    var backButton = new Button
    {
      Text = "Back",
      Y = Pos.Bottom(registerButton) + 1,
      X = Pos.Center(),
    };
    backButton.Clicked += () =>
    {
      Application.RequestStop();
    };

    Add(usernameLabel, usernameField,
      passwordLabel, passwordField,
      passwordRepeatLabel, passwordRepeatField,
      registerButton, backButton);
  }
}