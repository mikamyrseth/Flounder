using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace Flounder
{
  public class Body : IIndentedLogger, ISerializableJSON
  {
    public static Body ParseJSO(dynamic jso) {
      return new Body(
        (string)jso.id,
        (int)jso.mass,
        IShape.ParseJSO(jso.shape),
        Vector2.ParseJSO(jso.position),
        Vector2.ParseJSO(jso.velocity),
        Vector2.ParseJSO(jso.acceleration)
      );
    }
    private readonly float _mass;
    private Vector2 _position;
    private Vector2 _velocity;
    private Vector2 _acceleration;
    private readonly List<ConstantForce> _forces;
    public string ID { get; }
    public IShape Shape { get; }
    public Body(string id, float mass, IShape shape, Vector2 position, Vector2 velocity, Vector2 acceleration) {
      this.ID = id;
      this._mass = mass;
      this.Shape = shape ?? throw new ArgumentException("Shape cannot be null");
      this._position = position;
      this._velocity = velocity;
      this._acceleration = acceleration;
      this._forces = new List<ConstantForce>();
    }
    public Body(string id, float mass, IShape shape, Vector2 position) : this(id, mass, shape, position,
      new Vector2(0, 0)) { }
    public Body(string id, float mass, IShape shape, Vector2 position, Vector2 velocity) : this(id, mass, shape,
      position, velocity, new Vector2(0, 0)) { }
    public string SerializeJSON(int indent = 0, bool singleLine = false) {
      if (singleLine) {
        throw new NotImplementedException();
      }
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = "{\n";
      text += indentText + $"\t\"id\": {this.ID.ToString(CultureInfo.InvariantCulture)},\n";
      text += indentText + $"\t\"mass\": {this._mass.ToString(CultureInfo.InvariantCulture)},\n";
      text += indentText + $"\t\"shape\": {this.Shape.SerializeJSON(indent + 1)},\n";
      text += indentText + $"\t\"position\": {this._position.SerializeJSON(indent + 1)},\n";
      text += indentText + $"\t\"velocity\": {this._velocity.SerializeJSON(indent + 1)},\n";
      text += indentText + $"\t\"acceleration\": {this._acceleration.SerializeJSON(indent + 1)}\n";
      text += indentText + "}";
      return text;
    }
    public string ToString(int indent) {
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = indentText + "Body {\n";
      text += indentText + "\tid: " + this.ID + "\n";
      text += indentText + "\tmass: " + this._mass + "\n";
      text += indentText + "\tshape: " + this.Shape.ToString() + "\n";
      text += indentText + "\tposition: " + this._position + "\n";
      text += indentText + "\tvelocity: " + this._velocity + "\n";
      text += indentText + "\tacceleration: " + this._acceleration + "\n";
      text += indentText + "}";
      return text;
    }

    public override string ToString() { return this.ToString(0); }
    public void AddConstantForce(ConstantForce constantForce) { this._forces.Add(constantForce); }
    public void Tick(float timeInterval) {
      this.UpdateAcceleration();
      this.UpdateVelocity(timeInterval);
    }
    private void UpdateAcceleration() {
      Vector2 forceSum = new Vector2(0, 0);
      foreach (ConstantForce force in this._forces) {
        forceSum += force.Force;
      }
      this._acceleration = forceSum / this._mass;
    }
    private void UpdateVelocity(float timeInterval) {
      this._velocity += timeInterval * this._acceleration;
    }
  }
}
