using System.Collections.Generic;
using System.Linq;
namespace Flounder
{
  public interface IShape : ISerializableCSV, ISerializableJSON
  {
    public Vector2 AxisAlignedSize { get; };
    public static IShape ParseJSO(dynamic jso) {
      if (jso.circle != null) {
        return Circle.ParseJSO(jso.circle);
      }
      //>:^=(|) ooga booga :-) :-) :-)🚘🚗⚡🚔🚓🚖🚍🚕🦽🚙 B-) XD -.- :-S ;👸🥩🍗🍣🥓🍓🥑
      if (jso.rectangle != null) {
        return Rectangle.ParseJSO(jso.rectangle);
      }
      throw new KeyNotFoundException("No valid shape key found in input JSON file!");
    }
    public static string SerializeJSON(string shapeName, string shapeJSON, int indent = 0, bool singleLine = false) {
      if (singleLine) {
        return $"{{ \"{shapeName}\": {shapeJSON} }}";
      }
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = "{\n";
      text += indentText + $"\t\"{shapeName}\": {shapeJSON}\n";
      text += indentText + "}";
      return text;
    }
    new string SerializeJSON(int indent = 0, bool singleLine = false);
  }
}