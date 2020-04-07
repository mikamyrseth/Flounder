using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Dumber = Flounder.ImpliedFraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Flounder
{
  public class Simulation : IDisposable
  {
    public enum FileFormat
    {
      FLO,
      FLOD
    }
    private const string FLOFileExtension = "flo";
    private const string FLOVersion = "flo v1.0.3";
    private const string FLODFileExtension = "flod";
    private const string FLODVersion = "flod v1.0.0";
    private readonly Body[] _bodies;
    private readonly List<ConstantForce> _constantForces = new List<ConstantForce>();
    private readonly List<ConstantAcceleration> _constantAccelerations = new List<ConstantAcceleration>();
    private readonly FileFormat _fileFormat;
    private readonly StreamWriter _fileWriter;
    private Dumber _duration;
    private Dumber _time;
    private readonly Dumber _timeInterval;
    
    public string ToString(int indent) {
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = indentText + "Simulation { bodies: [\n";
      foreach (Body body in this._bodies) {
        text += indentText + body.SerializeJSON() + ",\n";
      }
      return text;
    }
    
    public void Dispose() {
      this._fileWriter?.Dispose();
    }
    public Simulation(string inputFilePath, string outputFileName, FileFormat fileFormat = FileFormat.FLO) {
      if (fileFormat == FileFormat.FLOD) {
        throw new NotImplementedException("Writing Flounder Output Debug files – .flod-files – is not supported!");
      }
      this._fileFormat = fileFormat;
      #region File setup
      string json = File.ReadAllText(inputFilePath);
      string outputFilePath = outputFileName + "." + (this._fileFormat == FileFormat.FLO ? FLOFileExtension : FLODFileExtension);
      this._fileWriter = new StreamWriter(File.Create(outputFilePath));
      #endregion
      #region Parse input
      dynamic jso = JsonConvert.DeserializeObject(json);
      this._duration = Dumber.Parse((string)(jso.duration ?? throw new KeyNotFoundException("Key \"duration\" was expected in input JSON file!")));
      this._time = new Dumber(0);
      if (!Enum.TryParse((string)(jso.precision ?? throw new KeyNotFoundException("Key \"precision\" was expected in input JSON file!")), true, out ImpliedFraction.PrecisionLevel precision)) {
        throw new FormatException("Precision value could not be parsed to PrecisionLevel!");
      }
      ImpliedFraction.Precision = precision;
      this._timeInterval = Dumber.Parse((string)(jso.timeInterval ?? throw new KeyNotFoundException("Key \"timeInterval\" was expected in input JSON file!")));
      #region Bodies
      dynamic bodiesJso = jso.bodies ?? throw new KeyNotFoundException("Key \"bodies\" was expected in input JSON file!");
      SortedList<string, Body> bodies = new SortedList<string, Body>();
      foreach (JObject bodyJSO in bodiesJso) {
        Body body = Body.ParseJSO(bodyJSO);
        bodies.Add(body.ID, body);
      }
      #endregion
      #region Constant forces
      dynamic forcesJso = jso.constantForces ?? throw new KeyNotFoundException("Key \"constantForces\" was expected in input JSON file!");
      foreach (dynamic forceJSO in forcesJso) {
        ConstantForce constantForce = ConstantForce.ParseJSO(forceJSO);
        this._constantForces.Add(constantForce);
        dynamic forceBodiesJSO = forceJSO.bodies ?? throw new KeyNotFoundException("Key \"bodies\" was expected as a key in \"constantForces\" in input JSON file!");
        foreach (string bodyID in forceJSO.bodies) {
          if (bodies.ContainsKey(bodyID)) {
            bodies[bodyID].Forces.Add(constantForce);
          }
        }
      }
      dynamic accelerationsJSO = jso.constantAccelerations ?? throw new KeyNotFoundException("Key \"constantAccelerations\" was expected in input JSON file!");
      foreach (dynamic accelerationJSO in accelerationsJSO) {
        ConstantAcceleration constantAcceleration = ConstantAcceleration.ParseJSO(accelerationJSO);
        this._constantAccelerations.Add(constantAcceleration);
        dynamic accelerationBodiesJSO = accelerationJSO.bodies ?? throw new KeyNotFoundException("Key \"bodies\" was expected as a key in \"constantAccelerations\" in input JSON file!");
        foreach (string bodyID in accelerationBodiesJSO) {
          if (bodies.ContainsKey(bodyID)) {
            bodies[bodyID].Accelerations.Add(constantAcceleration);
          }
        }
      }
      #endregion
      this._bodies = new Body[bodies.Count];
      int i = 0;
      foreach (Body body in bodies.Values) {
        this._bodies[i++] = body;
      }
      #endregion
      #region Output setup
      this._fileWriter.WriteLine(this._fileFormat == FileFormat.FLO ? FLOVersion : FLODVersion);
      this._fileWriter.WriteLine(precision);
      this._fileWriter.WriteLine(this._bodies.Length.ToString(CultureInfo.InvariantCulture));
      foreach (Body body in this._bodies) {
        switch (this._fileFormat) {
          case FileFormat.FLO:
            this._fileWriter.WriteLine($"# {body.ID}");
            this._fileWriter.WriteLine(body.Shape.SerializeCSV());
            break;
          case FileFormat.FLOD:
            this._fileWriter.WriteLine($"\"{body.ID}\", \"{body.Shape.SerializeJSON(singleLine: true)}\"");
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      #endregion
    }
    private void RecordFrame() {
      this._fileWriter.WriteLine("# " + this._time.DoubleApproximation.ToString(CultureInfo.InvariantCulture));
      this._fileWriter.WriteLine(this._time.SerializeJSON());
      foreach (Body body in this._bodies) {
        switch (this._fileFormat) {
          case FileFormat.FLO:
            this._fileWriter.WriteLine($"\t{body.Position.X.SerializeJSON()}, {body.Position.Y.SerializeJSON()}");
            break;
          case FileFormat.FLOD:
            // this._fileWriter.WriteLine($"\"{body.ID}\", {body.Position.X.ToString(CultureInfo.InvariantCulture)}, {body.Position.Y.ToString(CultureInfo.InvariantCulture)},  {body.Velocity.X.ToString(CultureInfo.InvariantCulture)}, {body.Velocity.Y.ToString(CultureInfo.InvariantCulture)}, {body.Acceleration.X.ToString(CultureInfo.InvariantCulture)}, {body.Acceleration.Y.ToString(CultureInfo.InvariantCulture)}");
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }
    public void Start() {
      this.Tick(new Dumber(0));
      this.RecordFrame();
      Dumber end = new Dumber(0);
      while (this._duration > end) {
        this.Tick(this._timeInterval);
        this._duration -= this._timeInterval;
        this._time += this._timeInterval;
        this.RecordFrame();
      }
    }
    private void Tick(Dumber timeInterval) {
      for (int i = 0; i < this._bodies.Length; i++) { // For every body
        Body body = this._bodies[i];
        Vector2 forceSum = body.Forces.Aggregate(new Vector2(), (current, force) => current + force.Force);
        Vector2 acceleration = forceSum / body.Mass;
        acceleration += body.Accelerations.Aggregate(new Vector2(), (current, acceleration) => current + acceleration.Acceleration);
        Vector2 velocity = body.Velocity + timeInterval * acceleration;
        Vector2 position = body.Position + timeInterval * velocity;
        body = body.SetState(position, velocity, acceleration); // Get a new body with new position
        this._bodies[i] = body;                                 // Switch to new body in simulation
      }
    }
  }
}
