using System.Globalization;
using System.Linq;

namespace Flounder
{

  public struct Circle : IShape
  {

    public static Circle ParseJSO(dynamic jso) { return new Circle((float)jso.radius); }

    public float Radius { get; }

    public Circle(float radius) { this.Radius = radius; }

    string IShape.SerializeJSON(int indent) { return IShape.SerializeJSON(indent, "circle", this.SerializeJSON(indent + 1)); }

    public string SerializeJSON(int indent) {
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = "{\n";
      text += indentText + $"\t\"radius\": {this.Radius.ToString(CultureInfo.InvariantCulture)}\n";
      text += indentText + "}";
      return text;
    }

    public string ToString(int indent) {
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      return indentText + $"Circle {{ radius: {this.Radius.ToString(CultureInfo.InvariantCulture)} }}";
    }

    public override string ToString() { return this.ToString(0); }

  }

}