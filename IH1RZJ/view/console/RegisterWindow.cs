using IH1RZJ.Controller;
using IH1RZJ.DAO;

using Terminal.Gui;

public class RegisterWindow : Window
{
  private readonly UserController controller = new UserController(
    DAOFactory.Instance.UserDAO,
    DAOFactory.Instance.ReviewDAO);

  public RegisterWindow()
  {
    Title = "Register";

    var usernameLabel = new Label
    {
      Text = "Username:"
    };
    var usernameField = new TextField
    {
      X = Pos.Right(usernameLabel),
      Width = Dim.Fill()
    };

    var passwordLabel = new Label
    {
      Text = "Password:",
      Y = Pos.Bottom(usernameLabel)
    };
    var passwordField = new TextField
    {
      X = Pos.Right(passwordLabel),
      Y = Pos.Bottom(usernameField),
      Width = Dim.Fill(),
      Secret = true
    };

    var passwordRepeatLabel = new Label
    {
      Text = "Repeat Password:",
      Y = Pos.Bottom(passwordField)
    };
    var passwordRepeatField = new TextField
    {
      X = Pos.Right(passwordRepeatLabel),
      Y = Pos.Bottom(passwordField),
      Width = Dim.Fill(),
      Secret = true
    };

    var registerButton = new Button
    {
      Text = "Register",
      Y = Pos.Bottom(passwordRepeatField),
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
      Y = Pos.Bottom(registerButton),
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