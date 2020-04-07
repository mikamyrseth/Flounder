using System.Collections.Generic;
using System;
using System.Globalization;
using System.Linq;
using Dumber = Flounder.ImpliedFraction;
namespace Flounder
{
  public struct Rectangle : IShape
  {
    public static Rectangle ParseCSV(string line) {
      string[] parts = line.Split(',');
      for (int i = 0; i < parts.Length; i++) {
        parts[i] = parts[i].Trim();
      }
      switch (parts.Length) {
        case 2:
          return new Rectangle(new Vector2(Dumber.Parse(parts[0]), Dumber.Parse(parts[1])));
        case 3:
          return new Rectangle(new Vector2(Dumber.Parse(parts[1]), Dumber.Parse(parts[2])));
        default:
          throw new FormatException("Could not parse Rectangle from CSV!");
      }
    }
    public static Rectangle ParseJSO(dynamic jso) {
      dynamic semiSizeJSO = jso.semiSize ?? throw new KeyNotFoundException("Key \"semiSize\" was expected as a key in \"rectangle\" in input JSON file!");
      return new Rectangle(Vector2.ParseJSO(semiSizeJSO));
    }
    private readonly Vector2 _semiSize;
    public Dumber Height {
      get { return 2 * this.SemiHeight; }
    }
    public Dumber SemiHeight {
      get { return this._semiSize.Y; }
    }
    public Vector2 SemiSize {
      get { return this._semiSize; }
    }
    public Dumber SemiWidth {
      get { return this._semiSize.X; }
    }
    public Dumber Width {
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