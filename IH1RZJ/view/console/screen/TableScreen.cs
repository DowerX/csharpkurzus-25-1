namespace IH1RZJ.View.ConsoleUI.Screen;

using Context = Dictionary<string, object>;

public delegate string DisplayFunc<T>(T item, ref Context context);
public delegate void InteractFunc<T>(T item, ref Context context);
public class TableScreen<T> : IScreen
{
  private List<T> items;
  private int index = 0;
  private DisplayFunc<T> displayFunc;
  private InteractFunc<T> interactFunc;
  private Context context;

  public TableScreen(
    List<T> items,
    DisplayFunc<T> displayFunc,
    InteractFunc<T> interactFunc,
    ref Context context)
  {
    this.items = items;
    this.displayFunc = displayFunc;
    this.interactFunc = interactFunc;
    this.context = context;
  }

  private void Draw()
  {
    Console.Clear();

    Console.CursorTop = 0;
    Console.CursorLeft = 0;

    for (int i = 0; i < items.Count; i++)
    {
      Console.WriteLine($"{(index == i ? '>' : ' ')} {displayFunc(items[i], ref context)}");
    }
  }

  public void Show()
  {
    Draw();

    bool stop = false;
    while (!stop)
    {
      if (Console.KeyAvailable)
      {
        var key = Console.ReadKey(true);
        switch (key.Key)
        {
          case ConsoleKey.LeftArrow:
          case ConsoleKey.Escape:
            stop = true;
            break;

          case ConsoleKey.Enter:
          case ConsoleKey.Spacebar:
          case ConsoleKey.RightArrow:
            interactFunc(items[index], ref context);
            Draw();
            break;

          case ConsoleKey.PageUp:
          case ConsoleKey.UpArrow:
            index = Math.Max(0, --index);
            Draw();
            break;

          case ConsoleKey.PageDown:
          case ConsoleKey.DownArrow:
            index = Math.Min(items.Count - 1, ++index);
            Draw();
            break;
        }
      }
    }
  }
}