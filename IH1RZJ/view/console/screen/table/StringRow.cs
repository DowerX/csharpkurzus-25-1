namespace IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

class StringRow(string text, StringRow.InteractFunc interaction) : ITableRow
{
  private readonly string text = text;
  public delegate void InteractFunc(ref Context context); private readonly InteractFunc interaction = interaction;

  public string Text(ref Context context) => text;

  public void Interact(ref Context context)
  {
    interaction(ref context);
  }
}