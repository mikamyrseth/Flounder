using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dumber = Flounder.ImpliedFraction;
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
          return new Circle(Dumber.Parse(parts[0]));
        case 2:
          return new Circle(Dumber.Parse(parts[1]));
        default:
          throw new FormatException("Could not parse Circle from CSV!");
      }
    }
    public static Circle ParseJSO(dynamic jso) {
      return new Circle(Dumber.Parse((string)(jso.radius ?? throw new KeyNotFoundException("Key \"radius\" was expected in input JSON file!"))));
    }
    public Dumber Radius { get; }
    public Circle(Dumber radius) {
      this.Radius = radius;
    }
    string IShape.SerializeJSON(int indent, bool singleLine) {
      return IShape.SerializeJSON("circle", this.SerializeJSON(indent + 1, singleLine), indent, singleLine);
    }
    public string SerializeCSV(bool header = true) {
      return (header ? "Circle, " : "") + this.Radius.SerializeJSON();
    }
    public string SerializeJSON(int indent = 0, bool singleLine = false) {
      if (singleLine) {
        return $"{{ \"radius\": {this.Radius} }}";
      }
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = "{\n";
      text += indentText + $"\t\"radius\": {this.Radius}\n";
      text += indentText + "}";
      return text;
    }
  }
}