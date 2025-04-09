namespace IH1RZJ.View.ConsoleUI.Screen.Table;

using Context = Dictionary<string, object>;

public class TableScreen(List<ITableRow> rows, ref Context context) : IScreen
{
  private readonly List<ITableRow> rows = rows;
  private int index = 0;
  private Context context = context;

  private void Draw()
  {
    Console.Clear();

    Console.CursorTop = 0;
    Console.CursorLeft = 0;

    for (int i = 0; i < rows.Count; i++)
    {
      Console.WriteLine($"{(index == i ? '>' : ' ')} {rows[i].Text(ref context)}");
    }
  }

  public void Show()
  {
    if (rows.Count == 0)
    {
      Console.Clear();
      Console.WriteLine("<empty>");
      Console.ReadLine();
      return;
    }

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
            rows[index].Interact(ref context);
            Draw();
            break;

          case ConsoleKey.PageUp:
          case ConsoleKey.UpArrow:
            index = Math.Max(0, --index);
            Draw();
            break;

          case ConsoleKey.PageDown:
          case ConsoleKey.DownArrow:
            index = Math.Min(rows.Count - 1, ++index);
            Draw();
            break;
        }
      }
    }
  }
}