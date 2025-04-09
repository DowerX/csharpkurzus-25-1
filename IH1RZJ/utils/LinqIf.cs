namespace IH1RZJ.Utils;

public static class LinqExtensions
{
  public static IEnumerable<T> If<T>(
    this IEnumerable<T> source,
    bool condition,
    Func<IEnumerable<T>, IEnumerable<T>> transform
  )
  {
    return condition ? transform(source) : source;
  }
}