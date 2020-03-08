using System.Globalization;

namespace Flounder
{

  public interface ISerializableJSON
  {

    string SerializeJSON(int indent = 0, bool singleLine = false);

  }

}