using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
    private const string FLOVersion = "flo v1.0.1";
    private const string FLODFileExtension = "flod";
    private const string FLODVersion = "flod v1.0.0";
    private readonly Body[] _bodies;
    private readonly List<ConstantForce> _constantForces = new List<ConstantForce>();
    private readonly FileFormat _fileFormat;
    private readonly StreamWriter _fileWriter;
    private readonly float _timeInterval;
    private float _duration;
    private Simulation(float timeInterval) {
      this._timeInterval = timeInterval;
    }
    public Simulation(SortedDictionary<string, Body> bodies, float timeInterval, List<ConstantForce> constantForces, float duration) : this(timeInterval) {
      this._bodies = bodies;
      this._constantForces = constantForces;
      this._duration = duration;
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
    public static Simulation ParseJSO(dynamic jso) {
      Simulation simulation = new Simulation((float)jso.timeInterval) {
        _duration = (float)jso.duration
      };
      foreach (JObject bodyJSO in jso.bodies) {
        Body body = Body.ParseJSO(bodyJSO);
        simulation._bodies.Add(body.ID, body);
      }
      foreach (dynamic forceJSO in jso.constantForces) {
        ConstantForce constantForce = ConstantForce.ParseJSO(forceJSO);
        simulation._constantForces.Add(constantForce);
        foreach (string bodyID in forceJSO.bodies) {
          if (simulation._bodies.ContainsKey(bodyID)) {
            simulation._bodies[bodyID].AddConstantForce(constantForce);
          }
        }
      }
      return simulation;
    }
    private void RecordFrame() {
      this._fileWriter.WriteLine(this._duration.ToString(CultureInfo.InvariantCulture));
      foreach (Body body in this._bodies) {
        switch (this._fileFormat) {
          case FileFormat.FLO:
            this._fileWriter.WriteLine($"\t{body.Position.X.ToString(CultureInfo.InvariantCulture)}, {body.Position.Y.ToString(CultureInfo.InvariantCulture)}");
            break;
          case FileFormat.FLOD:
            this._fileWriter.WriteLine($"\"{body.ID}\", {body.Position.X.ToString(CultureInfo.InvariantCulture)}, {body.Position.Y.ToString(CultureInfo.InvariantCulture)},  {body.Velocity.X.ToString(CultureInfo.InvariantCulture)}, {body.Velocity.Y.ToString(CultureInfo.InvariantCulture)}, {body.Acceleration.X.ToString(CultureInfo.InvariantCulture)}, {body.Acceleration.Y.ToString(CultureInfo.InvariantCulture)}");
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }
    public void Start() {
      this.RecordFrame();
      while (this._duration > 0) {
        this.Tick();
        this._duration -= this._timeInterval;
        this.RecordFrame();
      }
      this.RecordFrame();
    }
    private void Tick() {
      for (int i = 0; i < this._bodies.Length; i++) { // For every body
        Body body = this._bodies[i];
        Vector2 forceSum = body.Forces.Aggregate(new Vector2(), (current, force) => current + force.Force);
        Vector2 acceleration = forceSum / body.Mass;
        Vector2 velocity = body.Velocity + this._timeInterval * acceleration;
        Vector2 position = body.Position + this._timeInterval * velocity;
        body = body.SetState(position, velocity, acceleration); // Get a new body with new position
        this._bodies[i] = body;                                 // Switch to new body in simulation
      }
    }
  }
}
