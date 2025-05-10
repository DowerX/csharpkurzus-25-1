using IH1RZJ.Controller;
using IH1RZJ.DAO;
using IH1RZJ.Model;

using Terminal.Gui;

public class PersonEditWindow : Window
{
  private readonly Person person;
  private readonly PersonController controller = new PersonController(DAOFactory.Instance.PersonDAO);

  public PersonEditWindow(Person person)
  {
    this.person = person;

    Title = "Edit person";

    var nameLabel = new Label
    {
      Text = "Name:",
    };
    var nameField = new TextField
    {
      Text = person.Name,
      X = Pos.Right(nameLabel),
      Width = Dim.Fill()
    };

    var birthdayLabel = new Label
    {
      Text = "Birthday:",
      Y = Pos.Bottom(nameLabel)
    };
    var birthdayField = new TextField
    {
      Text = person.Birthday.ToString("yyyy/MM/dd"),
      X = Pos.Right(birthdayLabel),
      Y = Pos.Bottom(nameField),
      Width = Dim.Fill()
    };

    var deathLabel = new Label
    {
      Text = "Death:",
      Y = Pos.Bottom(birthdayLabel)
    };
    var deathField = new TextField
    {
      Text = person.Death?.ToString("yyyy/MM/dd") ?? "",
      X = Pos.Right(deathLabel),
      Y = Pos.Bottom(birthdayField),
      Width = Dim.Fill()
    };

    var bioLabel = new Label
    {
      Text = "Bio:",
      Y = Pos.Bottom(deathLabel)
    };
    var bioField = new TextField
    {
      Text = person.Bio,
      X = Pos.Right(bioLabel),
      Y = Pos.Bottom(deathField),
      Width = Dim.Fill()
    };

    var okButton = new Button
    {
      Text = "OK",
      X = Pos.Center(),
      Y = Pos.Bottom(bioLabel)
    };
    okButton.Clicked += async () =>
    {
      person.Name = (string)nameField.Text;

      if (DateTime.TryParseExact(
        (string)birthdayField.Text,
        "yyyy/MM/dd", null,
        System.Globalization.DateTimeStyles.None,
        out DateTime parsedBirthday))
      {
        person.Birthday = parsedBirthday;
      }
      else
      {
        MessageBox.ErrorQuery("Birthday", "Failed to parse date", "OK");
        return;
      }

      if ((string)deathField.Text == "")
      {
        person.Death = null;
      }
      else if (DateTime.TryParseExact(
        (string)deathField.Text,
        "yyyy/MM/dd", null,
        System.Globalization.DateTimeStyles.None,
        out DateTime parsedDeath))
      {
        person.Death = parsedDeath;
      }
      else
      {
        MessageBox.ErrorQuery("Death", "Failed to parse date", "OK");
        return;
      }

      person.Bio = (string)bioField.Text;

      if ((await controller.List(person.ID, null)).Count() == 0)
      {
        await controller.Create(person.Name, person.Birthday, person.Death, person.Bio);
      }
      else
      {
        await controller.Update(person);
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
      await controller.Delete(person);
      Application.RequestStop();
    };

    var cancelButton = new Button
    {
      Text = "Cancel",
      X = Pos.Center(),
      Y = Pos.Bottom(deleteButton)
    };
    cancelButton.Clicked += () => Application.RequestStop();

    Add(nameLabel, nameField,
      birthdayLabel, birthdayField,
      deathLabel, deathField,
      bioLabel, bioField,
      okButton, deleteButton, cancelButton);
  }
}