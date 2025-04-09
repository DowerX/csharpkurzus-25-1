namespace IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

public interface ITableRow {
  public string Text(ref Context context);
  public void Interact(ref Context context);
}