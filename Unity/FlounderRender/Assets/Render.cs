using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using UnityEngine;

namespace FlounderRender
{

  public class Render
  {

    private const string UnexpectedLineNumberErrorMessage = "Found unexpected number of lines in output-file!";
    private const string BodyCountFormatErrorMessage = "Could not read number of bodies in output-file!";

    private int _frame = 0;
    private int _maxFrame = -1;
    private Program _program;
    private List<Tuple<Transform, List<Vector3>>> _positions;

    private Vector2 _xBounds;
    private Vector2 _yBounds;

    public Vector2 Center {
      get {
        return new Vector2((this._xBounds.x + this._xBounds.y) / 2, (this._yBounds.x + this._yBounds.y) / 2);
      }
    }
    public int CurrentFrame { get { return this._frame + 1; } }
    public int MaxFrame { get { return this._maxFrame + 1; } }
    public float Radius {
      get { return Mathf.Max((this._xBounds.y - this._xBounds.x) / 2, (this._yBounds.y - this._yBounds.x) / 2); }
    }

    public Render(string outputFilePath, Program program) {
      this._xBounds = new Vector2(float.PositiveInfinity, float.NegativeInfinity);
      this._yBounds = new Vector2(float.PositiveInfinity, float.NegativeInfinity);
      this._program = program;
      using (OutputLineReader reader = new OutputLineReader("#", new StreamReader(outputFilePath))) {
        if (!reader.NextLine(out string version)) {
          throw new FormatException("Expected version line!");
        }
        switch (version) {
          case "flo v1.0.1":
            this.ParseFLO_v1_0_1(reader);
            break;
          case "flo v1.0.0":
            this.ParseFLO_v1_0_0(reader);
            break;
          case "flod v1.0.0":
            this.ParseFLOD_v1_0_0(reader);
            break;
          default:
            throw new FormatException("File format was not recognized!");
        }
      }
      this.ShowFrame(0);
    }

    public void NextFrame() {
      if (this._maxFrame < this._frame + 1) { return; }
      ++this._frame;
      this.ShowFrame(this._frame);
    }

    private void ParseFLO_v1_0_1(OutputLineReader reader) {
      if (!reader.NextLine(out string shapeNumberLine)) {
        throw new FormatException("Expected shape number line!");
      }
      if (!int.TryParse(shapeNumberLine, out int shapeNumber)) {
        throw new FormatException("Shape number line could not be parsed to an integer!");
      }
      this._positions = new List<Tuple<Transform, List<Vector3>>>();
      for (int i = 0; i < shapeNumber; i++) {
        if (!reader.NextLine(out string shapeLine)) {
          throw new FormatException("Expected shape line!");
        }
        Transform transform = this._program.CreateShape(shapeLine);
        List<Vector3> positions = new List<Vector3>();
        this._positions.Add(new Tuple<Transform, List<Vector3>>(transform, positions));
      }
      while (reader.NextLine(out string timeLine)) {
        if (!float.TryParse(timeLine, NumberStyles.Any, CultureInfo.InvariantCulture, out float time)) {
          Debug.Log(timeLine);
          break;
        }
        foreach (Tuple<Transform, List<Vector3>> tuple in this._positions) {
          if (!reader.NextLine(out string positionLine)) {
            throw new FormatException("Incorrect number of position lines!");
          }
          Vector2 position = this._program.ParseVector3FromCSV(positionLine);
          this._xBounds = new Vector2(Mathf.Min(this._xBounds.x, position.x), Mathf.Max(this._xBounds.y, position.x));
          this._yBounds = new Vector2(Mathf.Min(this._yBounds.x, position.y), Mathf.Max(this._yBounds.y, position.y));
          tuple.Item2.Add(position);
        }
        ++this._maxFrame;
      }
    }

    private void ParseFLO_v1_0_0(OutputLineReader reader) {
      throw new NotImplementedException("Parsing of flounder output file version 1.0.0 is unsupported!");
    }

    private void ParseFLOD_v1_0_0(OutputLineReader reader) {
      throw new NotImplementedException("Parsing of flounder output debug file version 1.0.0 is unsupported!");
    }

    public void PreviousFrame() {
      if (0 > this._frame - 1) { return; }
      --this._frame;
      this.ShowFrame(this._frame);
    }

    public void ShowFrame(int frame) {
      foreach (Tuple<Transform, List<Vector3>> tuple in this._positions) {
        tuple.Item1.position = tuple.Item2[frame];
      }
    }

  }

}