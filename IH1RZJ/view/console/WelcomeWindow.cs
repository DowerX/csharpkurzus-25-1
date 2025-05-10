using Terminal.Gui;

public class WelcomeWindow : Window
{
  public WelcomeWindow()
  {
    Title = "Movie manager";

    var loginButton = new Button
    {
      Text = "Login",
    };
    loginButton.Clicked += () => Application.Run<LoginWindow>();

    var registerButton = new Button
    {
      Text = "Register",
      Y = Pos.Bottom(loginButton) + 1
    };
    registerButton.Clicked += ()=> Application.Run<RegisterWindow>();

    var quit = new Button
    {
      Text = "Quit",
      Y = Pos.Bottom(registerButton) + 1
    };
    quit.Clicked += () => Application.RequestStop();

    Add(loginButton, registerButton, quit);
  }
}