using System.Collections.Generic;
using System;
using System.Globalization;
using System.Linq;
namespace Flounder
{
  public struct Rectangle : IShape
  {
    public bool DoesCollide(IShape shape, Vector2 startPosition, Vector2 endPosition, out float timeFactor) {
      throw new NotImplementedException("The collision between Rectangle and any other shape is not implemented!");
    }
    public static Rectangle ParseCSV(string line) {
      string[] parts = line.Split(',');
      for (int i = 0; i < parts.Length; i++) {
        parts[i] = parts[i].Trim();
      }
      switch (parts.Length) {
        case 2:
          return new Rectangle(new Vector2(float.Parse(parts[0], CultureInfo.InvariantCulture), float.Parse(parts[1], CultureInfo.InvariantCulture)));
        case 3:
          return new Rectangle(new Vector2(float.Parse(parts[1], CultureInfo.InvariantCulture), float.Parse(parts[2], CultureInfo.InvariantCulture)));
        default:
          throw new FormatException("Could not parse Rectangle from CSV!");
      }
    }
    public static Rectangle ParseJSO(dynamic jso) {
      dynamic semiSizeJSO = jso.semiSize ?? throw new KeyNotFoundException("Key \"semiSize\" was expected as a key in \"rectangle\" in input JSON file!");
      return new Rectangle(Vector2.ParseJSO(semiSizeJSO));
    }
    private readonly Vector2 _semiSize;
    public float Height {
      get { return 2 * this.SemiHeight; }
    }
    public BoundingBox OffsetBoundingBox {
      get { return new BoundingBox(-this.SemiSize, this.Size); }
    }
    public float SemiHeight {
      get { return this._semiSize.Y; }
    }
    public Vector2 SemiSize {
      get { return this._semiSize; }
    }
    public float SemiWidth {
      get { return this._semiSize.X; }
    }
    public Vector2 Size {
      get { return 2 * this.SemiSize; }
    }
    public float Width {
      get { return 2 * this.SemiWidth; }
    }
    public Rectangle(Vector2 semiSize) {
      this._semiSize = semiSize;
    }
    string IShape.SerializeJSON(int indent, bool singleLine) {
      return IShape.SerializeJSON("rectangle", this.SerializeJSON(indent + 1, singleLine), indent, singleLine);
    }
    public string SerializeCSV(bool header = true) {
      return (header ? "Rectangle, " : "") + this.SemiSize.SerializeCSV(false);
    }
    public string SerializeJSON(int indent = 0, bool singleLine = false) {
      if (singleLine) {
        return $"{{ \"semiSize\": {this._semiSize.SerializeJSON(singleLine: true)} }}";
      }
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = "{\n";
      text += indentText + $"\t\"semiSize\": {this._semiSize.SerializeJSON(indent + 1)}\n";
      text += indentText + "}";
      return text;
    }
  }
}