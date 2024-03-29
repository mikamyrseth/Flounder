﻿using System;
using System.Globalization;
using System.Linq;
namespace Flounder
{

  public struct Circle
  {

    public static Circle ParseCSV(string line) {
      string[] parts = line.Split(',');
      for (int i = 0; i < parts.Length; i++) { parts[i] = parts[i].Trim(); }
      switch (parts.Length) {
        case 1:
          return new Circle(float.Parse(parts[0], CultureInfo.InvariantCulture));
        case 2:
          return new Circle(float.Parse(parts[1], CultureInfo.InvariantCulture));
        default:
          throw new FormatException("Could not parse Circle from CSV!");
      }
    }
    public float Radius { get; }
    public Circle(float radius) { this.Radius = radius; }

  }

}