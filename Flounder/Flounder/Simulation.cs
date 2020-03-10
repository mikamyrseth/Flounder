﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
namespace Flounder
{
  public class Simulation : IIndentedLogger
  {
    private readonly SortedDictionary<int, Body> _bodies = new SortedDictionary<int, Body>();
    public Simulation(List<Body> bodies) {
      foreach (Body body in bodies) {
        this._bodies[body.ID] = body;
      }
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
      List<Body> bodies = new List<Body>();
      foreach (JObject bodyJSON in jso.bodies) {
        bodies.Add(Body.ParseJSO(bodyJSON));
      }
      return new Simulation(bodies);
    }
    public Body GetBody(int bodyID) {
      return this._bodies[bodyID];
    }
    public void Start(int ticks) {
      for (int i = 0; i < ticks; i++) {
        this.Tick();
      }
    }
    private void Tick() { }
  }
}