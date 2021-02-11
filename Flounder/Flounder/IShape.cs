using System;
using System.Collections.Generic;
using System.Linq;
namespace Flounder
{
  public interface IShape : ISerializableCSV, ISerializableJSON
  {
    public BoundingBox OffsetBoundingBox { get; }
    public bool DoesCollide(IShape shape, Vector2 startPosition, Vector2 endPosition, out float timeFactor, out Vector2 normal);
    public static bool DoesCollide(Circle stationaryCircle, Circle movingCircle, Vector2 startPosition, Vector2 endPosition, out float timeFactor, out Vector2 normal) {
      float radius = stationaryCircle.Radius + movingCircle.Radius;
      float stationary = startPosition * (startPosition - endPosition);
      Vector2 difference = endPosition - startPosition;
      float squareDifference = difference * difference;
      float cross = (startPosition.X * endPosition.Y - startPosition.Y * endPosition.X);
      float squareCross = cross * cross;
      float inner = radius * radius * squareDifference - squareCross;
      if (inner < 0) {
        timeFactor = -1;
        normal = new Vector2(1, 0);
        return false;
      }
      float rootInner = MathF.Sqrt(inner);
      float t0 = (stationary + rootInner) / squareDifference;
      float t1 = (stationary - rootInner) / squareDifference;
      if (0 <= t0 && t0 <= 1) {
        if (0 <= t1 && t1 <= 1) {
          timeFactor = MathF.Min(t0, t1);
        } else {
          timeFactor = t0;
        }
      } else {
        if (0 <= t1 && t1 <= 1) {
          timeFactor = t1;
        } else {
          timeFactor = -1;
          normal = new Vector2(1, 0);
          return false;
        }
      }
      normal = (1 - timeFactor) * startPosition + timeFactor * endPosition;
      return true;
    }
    public static bool DoesCollide(Line stationaryLine, Circle movingCircle, Vector2 startPosition, Vector2 endPosition, out float timeFactor, out Vector2 normal) {
      timeFactor = 2;
      normal = new Vector2(1, 0);
      Vector2 spn = startPosition.ToBaseSpace(stationaryLine.SemiLength); // Start position in normal space
      Vector2 epn = endPosition.ToBaseSpace(stationaryLine.SemiLength); // End position in normal space
      float vn = epn.Y - spn.Y; // Velocity along normal axis
      float t0 = (spn.Y - movingCircle.Radius / normal.Length) / vn;
      if (0 <= t0 && t0 <= 1) {
        float m0 = spn.X * (1 - t0) + epn.X * t0;
        if (-1 <= m0 && m0 <= 1) {
          if (t0 < timeFactor) {
            timeFactor = t0;
            normal = new Vector2(-stationaryLine.SemiLength.Y, stationaryLine.SemiLength.X);
          }
        }
      }
      float t1 = (spn.Y + movingCircle.Radius / normal.Length) / vn;
      if (0 <= t1 && t1 <= 1) {
        float m1 = spn.X * (1 - t1) + epn.X * t1;
        if (-1 <= m1 && m1 <= 1) {
          if (t1 < timeFactor) {
            timeFactor = t1;
            normal = new Vector2(-stationaryLine.SemiLength.Y, stationaryLine.SemiLength.X);
          }
        }
      }
      Circle endPoint = new Circle(0);
      float tempTime;
      Vector2 tempNormal;
      IShape.DoesCollide(
        endPoint,
        movingCircle,
        startPosition - stationaryLine.SemiLength,
        endPosition - stationaryLine.SemiLength,
        out tempTime,
        out tempNormal
      );
      if (0 <= tempTime && tempTime < timeFactor) {
        timeFactor = tempTime;
        normal = tempNormal;
      }
      IShape.DoesCollide(
        endPoint,
        movingCircle,
        startPosition + stationaryLine.SemiLength,
        endPosition + stationaryLine.SemiLength,
        out tempTime,
        out tempNormal
      );
      if (0 <= tempTime && tempTime < timeFactor) {
        timeFactor = tempTime;
        normal = tempNormal;
      }
      return timeFactor <= 1; // Time factor is valid
    }
    public static IShape ParseJSO(dynamic jso) {
      if (jso.circle != null) {
        return Circle.ParseJSO(jso.circle);
      }
      if (jso.rectangle != null) {
        return Rectangle.ParseJSO(jso.rectangle);
      }
      if (jso.line != null) {
        return Line.ParseJSO(jso.line);
      }
      throw new KeyNotFoundException("No valid shape key found in input JSON file!");
    }
    public static string SerializeJSON(string shapeName, string shapeJSON, int indent = 0, bool singleLine = false) {
      if (singleLine) {
        return $"{{ \"{shapeName}\": {shapeJSON} }}";
      }
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = "{\n";
      text += indentText + $"\t\"{shapeName}\": {shapeJSON}\n";
      text += indentText + "}";
      return text;
    }
    new string SerializeJSON(int indent = 0, bool singleLine = false);
  }
}