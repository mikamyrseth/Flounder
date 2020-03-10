namespace Flounder
{
  public interface IIndentedLogger
  {
    string ToString();
    string ToString(int indent);
  }
}