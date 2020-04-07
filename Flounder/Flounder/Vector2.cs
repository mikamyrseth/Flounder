using System.Collections.Generic;
using System.Linq;
using Dumber = Flounder.ImpliedFraction;
namespace Flounder
{
  public readonly struct Vector2 : ISerializableCSV, ISerializableJSON
  {
    public static Vector2 Zero {
      get { return new Vector2(Dumber.Zero, Dumber.Zero); }
    }
    public static Vector2 operator +(Vector2 a, Vector2 b) {
      return new Vector2(a.X + b.X, a.Y + b.Y);
    }
    public static Vector2 operator -(Vector2 a, Vector2 b) {
      return new Vector2(a.X - b.X, a.Y - b.Y);
    }
    public static Vector2 operator *(Dumber s, Vector2 a) {
      return new Vector2(s * a.X, s * a.Y);
    }
    public static Dumber operator *(Vector2 a, Vector2 b) {
      return a.X * b.X + a.Y * b.Y;
    }
    public static Vector2 operator /(Vector2 a, Dumber s) {
      return new Vector2(a.X / s, a.Y / s);
    }
    public static Vector2 ParseJSO(dynamic jso) {
      dynamic xJSO = jso.x ?? throw new KeyNotFoundException("Key \"x\" was expected as a key in \"vector\" in input JSON file!");
      dynamic yJSO = jso.y ?? throw new KeyNotFoundException("Key \"y\" was expected as a key in \"vector\" in input JSON file!");
      return new Vector2(Dumber.Parse((string)xJSO), Dumber.Parse((string)yJSO));
    }
    public Dumber SquareLength {
      get { return this * this; }
    }
    public Dumber X { get; }
    public Dumber Y { get; }
    public Vector2(Dumber x, Dumber y) {
      this.X = x;
      this.Y = y;
    }
    public string SerializeCSV(bool header = true) {
      return (header ? "Vector2, " : "") + $"{this.X.SerializeJSON()}, {this.Y.SerializeJSON()}";
    }
    public string SerializeJSON(int indent = 0, bool singleLine = false) {
      if (singleLine) {
        return $"{{ \"x\": {this.X.ToString()}, \"y\": {this.Y.ToString()} }}";
      }
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = "{\n";
      text += indentText + $"\t\"x\": {this.X.ToString()},\n";
      text += indentText + $"\t\"y\": {this.Y.ToString()}\n";
      text += indentText + "}";
      return text;
    }
  }
}