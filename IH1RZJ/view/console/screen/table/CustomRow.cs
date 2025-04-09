namespace IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

class CustomRow<T>(T item, CustomRow<T>.DisplayFunc displayFunction, CustomRow<T>.InteractFunc interaction) : ITableRow
{
  private T item = item;
  public delegate void InteractFunc(ref T item, ref Context context);
  private readonly InteractFunc interaction = interaction;

  public delegate string DisplayFunc(T item, ref Context context);
  private readonly DisplayFunc displayFunction = displayFunction;

  public string Text(ref Context context)
  {
    return displayFunction(item, ref context);
  }

  public void Interact(ref Context context)
  {
    interaction(ref item, ref context);
  }
}