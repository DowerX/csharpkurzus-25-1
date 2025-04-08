namespace IH1RZJ;

public enum Opertaion
{
  Load,
  Print
}

public class Config
{
  public Opertaion Operation { get; set; } = Opertaion.Print;
  public string Path { get; set; } = "./test.json";
}