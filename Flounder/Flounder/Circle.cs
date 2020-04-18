using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace Flounder
{
  public struct Circle : IShape
  {
    public bool DoesCollide(IShape shape, Vector2 startPosition, Vector2 endPosition, out float timeFactor) {
      switch(shape) {
        case Circle circle:
          float radius = this.Radius + circle.Radius;
          float stationary = startPosition * (startPosition - endPosition);
          Vector2 difference = endPosition - startPosition;
          float squareDifference = difference * difference;
          float cross = (startPosition.X * endPosition.Y - startPosition.Y * endPosition.X);
          float squareCross = cross * cross;
          float inner = radius * radius * squareDifference - squareCross;
          if (inner < 0) {
            timeFactor = -1;
            return false;
          }
          float rootInner = MathF.Sqrt(inner);
          float t0 = (stationary + rootInner) / squareDifference;
          float t1 = (stationary - rootInner) / squareDifference;
          if (0 <= t0 && t0 <= 1) {
            if (0 <= t1 && t1 <= 1) {
              timeFactor = MathF.Min(t0, t1);
              return true;
            } else {
              timeFactor = t0;
              return true;
            }
          } else {
            if (0 <= t1 && t1 <= 1) {
              timeFactor = t1;
              return true;
            }
          }
          timeFactor = -1;
          return false;
        default:
          throw new NotImplementedException($"The collision between circle and supplied shape {shape} is not defined.");
      }
    }
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
      return new Circle((float)(jso.radius ?? throw new KeyNotFoundException("Key \"radius\" was expected in input JSON file!")));
    }
    public BoundingBox OffsetBoundingBox {
      get { return new BoundingBox(new Vector2(-this.Radius, -this.Radius), new Vector2(2 * this.Radius, 2 * this.Radius)); }
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