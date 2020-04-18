using System.Globalization;
using System;
using System.Collections.Generic;
namespace Flounder{

  public struct Line : IShape {

    public Vector2 SemiLength { get; }

    public BoundingBox OffsetBoundingBox {
      get { return new BoundingBox(-this.SemiLength, 2 * this.SemiLength); }
    }

    public Line(Vector2 semiLength) {
      this.SemiLength = semiLength;
    }

    public bool DoesCollide(IShape shape, Vector2 startPosition, Vector2 endPosition, out float timeFactor) {
        timeFactor = 0;
        return true;
    }

    public static Line ParseJSO(dynamic jso) {
      return new Line(Vector2.ParseJSO(jso.semiLength ?? throw new KeyNotFoundException("Key \"semiLength\" was expected in \"Line\" input JSON file!")));
    }
    public static Line ParseCSV(string line) {
      string[] parts = line.Split(',');
      for (int i = 0; i < parts.Length; i++) {
        parts[i] = parts[i].Trim();
      }
      switch (parts.Length) {
        case 2:
          return new Line(new Vector2(float.Parse(parts[0], CultureInfo.InvariantCulture), float.Parse(parts[1], CultureInfo.InvariantCulture)));
        case 3:
          return new Line(new Vector2(float.Parse(parts[1], CultureInfo.InvariantCulture), float.Parse(parts[2], CultureInfo.InvariantCulture)));
        default:
          throw new FormatException("Could not parse Line from CSV!");
      }
    }

    public string SerializeCSV(bool header = true) {
      return (header ? "Line, " : "") + this.SemiLength.SerializeCSV(false);
    }       

  }

}