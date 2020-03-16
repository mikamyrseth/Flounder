using System;
using System.Globalization;
using System.Linq;
namespace Flounder
{
  public struct Circle : IShape
  {
    public static Circle ParseCSV(string line) {
      string[] parts = line.Split(',');
      for (int i = 0; i < parts.Length; i++) {
        parts[i] = parts[i].Trim();
      }
      switch (parts.Length) {
        case 1:
          return new Circle(float.Parse(parts[0], CultureInfo.InvariantCulture));
        case 2:
          return new Circle(float.Parse(parts[1], CultureInfo.InvariantCulture));
        default:
          throw new FormatException("Could not parse Circle from CSV!");
      }
    }
    public static Circle ParseJSO(dynamic jso) {
      return new Circle((float)jso.radius);
    }
    public float Radius { get; }
    public Circle(float radius) {
      this.Radius = radius;
    }
    string IShape.SerializeJSON(int indent, bool singleLine) {
      return IShape.SerializeJSON("circle", this.SerializeJSON(indent + 1, singleLine), indent, singleLine);
    }
    public string SerializeCSV(bool header = true) {
      return (header ? "Circle, " : "") + this.Radius.ToString(CultureInfo.InvariantCulture);
    }
    public string SerializeJSON(int indent = 0, bool singleLine = false) {
      if (singleLine) {
        return $"{{ \"radius\": {this.Radius.ToString(CultureInfo.InvariantCulture)} }}";
      }
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = "{\n";
      text += indentText + $"\t\"radius\": {this.Radius.ToString(CultureInfo.InvariantCulture)}\n";
      text += indentText + "}";
      return text;
    }
  }
}