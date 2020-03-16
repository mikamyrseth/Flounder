namespace Flounder
{
  public interface ISerializableCSV
  {
    string SerializeCSV(bool header = true);
  }
}