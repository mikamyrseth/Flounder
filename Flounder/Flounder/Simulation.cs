using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Flounder
{
  public class Simulation : IDisposable, IIndentedLogger
  {
    public enum FileFormat
    {
      FLO,
      FLOD
    }
    private const string FLOFileExtension = "flo";
    private const string FLOVersion = "flo v1.0.0";
    private const string FLODFileExtension = "flod";
    private const string FLODVersion = "flod v1.0.0";
    private readonly SortedList<string, Body> _bodies = new SortedList<string, Body>();
    private readonly List<ConstantForce> _constantForces = new List<ConstantForce>();
    private readonly FileFormat _fileFormat;
    private readonly StreamWriter _fileWriter;
    private readonly float _timeInterval;
    private float _duration;
    public Simulation(string inputFilePath, string outputFileName, FileFormat fileFormat = FileFormat.FLO) {
      this._fileFormat = fileFormat;
      #region File setup
      string json = File.ReadAllText(inputFilePath);
      string outputFilePath = outputFileName + "." +
        (this._fileFormat == FileFormat.FLO ? FLOFileExtension : FLODFileExtension);
      this._fileWriter = new StreamWriter(File.Create(outputFilePath));
      #endregion
      #region Parse input
      dynamic jso = JsonConvert.DeserializeObject(json);
      this._duration = (float) (jso.duration ??
        throw new KeyNotFoundException("Key \"duration\" was expected in input JSON file!"));
      if (!Enum.TryParse(
        (string) (jso.precision ??
          throw new KeyNotFoundException("Key \"precision\" was expected in input JSON file!")),
        true, out ImpliedFraction.PrecisionLevel precision
      )) {
        throw new FormatException("Precision value could not be parsed to PrecisionLevel!");
      }
      ImpliedFraction.Precision = precision;
      this._timeInterval = (float) (jso.timeInterval ??
        throw new KeyNotFoundException("Key \"timeInterval\" was expected in input JSON file!"));
      this.ParseBodies(jso.bodies ?? throw new KeyNotFoundException("Key \"bodies\" was expected in input JSON file!"));
      this.ParseConstantForces(jso.constantForces ??
        throw new KeyNotFoundException("Key \"constantForces\" was expected in input JSON file!"));
      #endregion
      #region Output setup
      this._fileWriter.WriteLine(this._fileFormat == FileFormat.FLO ? FLOVersion : FLODVersion);
      this._fileWriter.WriteLine(this._bodies.Count);
      foreach (Body body in this._bodies.Values) {
        this._fileWriter.WriteLine(body.ID);
      }
      #endregion
    }
    public void Dispose() {
      this._fileWriter?.Dispose();
    }
    public string ToString(int indent) {
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = indentText + "Simulation { bodies: [\n";
      foreach (Body body in this._bodies.Values) {
        text += indentText + body.ToString(indent + 1) + ",\n";
      }
      text += indentText + "] }";
      return text;
    }
    public override string ToString() {
      return this.ToString(0);
    }
    private void ParseBodies(dynamic bodiesJso) {
      foreach (JObject bodyJSO in bodiesJso) {
        Body body = Body.ParseJSO(bodyJSO);
        this._bodies.Add(body.ID, body);
      }
    }
    private void ParseConstantForces(dynamic constantForcesJso) {
      foreach (dynamic forceJSO in constantForcesJso) {
        ConstantForce constantForce = ConstantForce.ParseJSO(forceJSO);
        this._constantForces.Add(constantForce);
        foreach (string bodyID in forceJSO.bodies) {
          if (this._bodies.ContainsKey(bodyID)) {
            this._bodies[bodyID].AddConstantForce(constantForce);
          }
        }
      }
    }
    public Body GetBody(string bodyID) {
      return this._bodies[bodyID];
    }
    public void Start() {
      while (this._duration > 0) {
        this.Tick();
      }
    }
    private void Tick() {
      foreach (Body body in this._bodies.Values) {
        body.Tick(this._timeInterval);
      }
      this._duration -= this._timeInterval;
    }
  }
}
