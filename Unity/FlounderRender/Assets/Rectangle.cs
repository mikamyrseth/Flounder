using System;
using System.Globalization;
using System.Linq;

using UnityEngine;

namespace Flounder
{
  public struct Rectangle
  {
    public static Rectangle ParseCSV(string line) {
      string[] parts = line.Split(',');
      for (int i = 0; i < parts.Length; i++) { parts[i] = parts[i].Trim(); }
      switch (parts.Length) {
        case 2:
          return new Rectangle(new Vector2(
            float.Parse(parts[0], CultureInfo.InvariantCulture), 
            float.Parse(parts[1], CultureInfo.InvariantCulture)
          ));
        case 3:
          return new Rectangle(new Vector2(
            float.Parse(parts[1], CultureInfo.InvariantCulture), 
            float.Parse(parts[2], CultureInfo.InvariantCulture)
          ));
        default:
          throw new FormatException("Could not parse Rectangle from CSV!");
      }
    }
    private readonly Vector2 _semiSize;
    public Vector2 SemiSize {
      get { return this._semiSize; }
    }
    public Rectangle(Vector2 semiSize) {
      this._semiSize = semiSize;
    }

  }
}
