using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
namespace Flounder
{
  public class Simulation : IIndentedLogger
  {
    private readonly SortedDictionary<string, Body> _bodies = new SortedDictionary<string, Body>();
    private readonly List<ConstantForce> _constantForces = new List<ConstantForce>();
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