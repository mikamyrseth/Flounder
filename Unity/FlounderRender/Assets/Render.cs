using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Flounder;

using UnityEngine;

namespace FlounderRender
{

  public class Render
  {

    private const string UnexpectedLineNumberErrorMessage = "Found unexpected number of lines in output-file!";
    private const string BodyCountFormatErrorMessage = "Could not read number of bodies in output-file!";

    private int _frame = 0;
    private List<float> _frameTimes;
    private int _maxFrame = -1;
    private Program _program;
    private List<RenderObject> _renderObjects;

    private Vector2 _xBounds;
    private Vector2 _yBounds;

    public Vector2 Center {
      get {
        return new Vector2((this._xBounds.x + this._xBounds.y) / 2, (this._yBounds.x + this._yBounds.y) / 2);
      }
    }
    public int CurrentFrame { get { return this._frame + 1; } }
    public int MaxFrame { get { return this._maxFrame + 1; } }
    public float MaxTime { get { return this._frameTimes[this._frameTimes.Count - 1]; } }
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
          case "flo v1.0.3":
            this.ParseFLO_v1_0_3(reader);
            break;
          case "flo v1.0.2":
            this.ParseFLO_v1_0_2(reader);
            break;
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

    public float JumpFrames(int framesToJump) {
      int newFrameNumber = this._frame + framesToJump;
      if (newFrameNumber < 0) {
        this._frame = 0;
      } else if (this._maxFrame < newFrameNumber) {
        this._frame = this._maxFrame;
      } else {
        this._frame = newFrameNumber;
      }
      this.ShowFrame(this._frame);
      return this._frameTimes[this._frame];
    }

    private void ParseFLO_v1_0_3(OutputLineReader reader) {
      if (!reader.NextLine(out string precisionLine)) {
        throw new FormatException("Expected precision line!");
      }
      if (!Enum.TryParse(precisionLine, true, out ImpliedFraction.PrecisionLevel precision)) {
        ImpliedFraction.Precision = precision;
      }
      if (!reader.NextLine(out string shapeNumberLine)) {
        throw new FormatException("Expected shape number line!");
      }
      if (!int.TryParse(shapeNumberLine, out int shapeNumber)) {
        throw new FormatException("Shape number line could not be parsed to an integer!");
      }
      this._renderObjects = new List<RenderObject>();
      for (int i = 0; i < shapeNumber; i++) {
        if (!reader.NextLine(out string shapeLine)) {
          throw new FormatException("Expected shape line!");
        }
        this._renderObjects.Add(this._program.CreateRenderObject(shapeLine));
      }
      this._frameTimes = new List<float>();
      List<Vector3>[] positions = new List<Vector3>[this._renderObjects.Count];
      for (int i = 0; i < this._renderObjects.Count; i++) {
        positions[i] = new List<Vector3>();
      }
      while (reader.NextLine(out string timeLine)) {
        if (!long.TryParse(timeLine, out long timeNumerator)) {
          break;
        }
        this._frameTimes.Add(new ImpliedFraction(timeNumerator).FloatApproximation);
        foreach (List<Vector3> positionList in positions) {
          if (!reader.NextLine(out string positionLine)) {
            throw new FormatException("Incorrect number of position lines!");
          }
          string[] parts = positionLine.Split(',');
          Vector2 position = new Vector2(
            new ImpliedFraction(long.Parse(parts[0])).FloatApproximation, 
            new ImpliedFraction(long.Parse(parts[1])).FloatApproximation
          );
          this._xBounds = new Vector2(Mathf.Min(this._xBounds.x, position.x), Mathf.Max(this._xBounds.y, position.x));
          this._yBounds = new Vector2(Mathf.Min(this._yBounds.x, position.y), Mathf.Max(this._yBounds.y, position.y));
          positionList.Add(position);
        }
        ++this._maxFrame;
      }
      for (int i = 0; i < this._renderObjects.Count; i++) {
        this._renderObjects[i].AddPositions(positions[i]);
      }
    }

    private void ParseFLO_v1_0_2(OutputLineReader reader) {
      if (!reader.NextLine(out string shapeNumberLine)) {
        throw new FormatException("Expected shape number line!");
      }
      if (!int.TryParse(shapeNumberLine, out int shapeNumber)) {
        throw new FormatException("Shape number line could not be parsed to an integer!");
      }
      this._renderObjects = new List<RenderObject>();
      for (int i = 0; i < shapeNumber; i++) {
        if (!reader.NextLine(out string shapeLine)) {
          throw new FormatException("Expected shape line!");
        }
        this._renderObjects.Add(this._program.CreateRenderObject(shapeLine));
      }
      this._frameTimes = new List<float>();
      List<Vector3>[] positions = new List<Vector3>[this._renderObjects.Count];
      for (int i = 0; i < this._renderObjects.Count; i++) {
        positions[i] = new List<Vector3>();
      }
      while (reader.NextLine(out string timeLine)) {
        if (!float.TryParse(timeLine, NumberStyles.Any, CultureInfo.InvariantCulture, out float time)) {
          break;
        }
        this._frameTimes.Add(time);
        foreach (List<Vector3> positionList in positions) {
          if (!reader.NextLine(out string positionLine)) {
            throw new FormatException("Incorrect number of position lines!");
          }
          Vector2 position = this._program.ParseVector3FromCSV(positionLine);
          this._xBounds = new Vector2(Mathf.Min(this._xBounds.x, position.x), Mathf.Max(this._xBounds.y, position.x));
          this._yBounds = new Vector2(Mathf.Min(this._yBounds.x, position.y), Mathf.Max(this._yBounds.y, position.y));
          positionList.Add(position);
        }
        ++this._maxFrame;
      }
      for (int i = 0; i < this._renderObjects.Count; i++) {
        this._renderObjects[i].AddPositions(positions[i]);
      }
    }
    
    private void ParseFLO_v1_0_1(OutputLineReader reader) {
      throw new NotImplementedException("Parsing of flounder output file version 1.0.1 is unsupported!");
    }

    private void ParseFLO_v1_0_0(OutputLineReader reader) {
      throw new NotImplementedException("Parsing of flounder output file version 1.0.0 is unsupported!");
    }

    private void ParseFLOD_v1_0_0(OutputLineReader reader) {
      throw new NotImplementedException("Parsing of flounder output debug file version 1.0.0 is unsupported!");
    }

    public void SetTraceVisibility(bool isTraceVisible) {
      foreach (RenderObject renderObject in this._renderObjects) {
        renderObject.SetTraceVisibility(isTraceVisible);
      }
    }

    public void SetTraceWidth(float traceWidth) {
      foreach (RenderObject renderObject in this._renderObjects) {
        renderObject.SetTraceWidth(traceWidth);
      }
    }

    public void ShowTime(float time) {
      int index = this._frameTimes.BinarySearch(time);
      if (index < 0) { index = -index - 1; }
      if (index == this._frameTimes.Count) {
        this.ShowFrame(this._maxFrame);
        return;
      }
      if (index == 0) {
        this.ShowFrame(0);
        return;
      }
      if (this._frameTimes[index] - time < time - this._frameTimes[index - 1]) {
        this.ShowFrame(index);
      } else {
        this.ShowFrame(index - 1);
      }
    }

    public void ShowFrame(int frame) {
      this._frame = frame;
      foreach (RenderObject renderObject in this._renderObjects) { renderObject.ShowFrame(frame); }
    }

  }

}