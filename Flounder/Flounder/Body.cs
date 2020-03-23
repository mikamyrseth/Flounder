using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace Flounder
{
  public readonly struct Body : ISerializableJSON
  {
    public static Body ParseJSO(dynamic jso) {
      return new Body((string)jso.id, (int)jso.mass, IShape.ParseJSO(jso.shape), Vector2.ParseJSO(jso.position), Vector2.ParseJSO(jso.velocity), Vector2.ParseJSO(jso.acceleration));
    }
    public List<ConstantAcceleration> Accelerations { get; }
    public List<ConstantForce> Forces { get; }
    public string ID { get; }
    public float Mass { get; }
    public Vector2 Position { get; }
    public IShape Shape { get; }
    public Vector2 Velocity { get; }
    public Vector2 Acceleration { get; }

    public Body(string id, float mass, IShape shape, Vector2 position, Vector2 velocity, Vector2 acceleration, List<ConstantAcceleration> accelerations = null, List<ConstantForce> forces = null) {
      this.ID = id;
      this.Mass = mass;
      this.Shape = shape ?? throw new ArgumentException("Shape cannot be null");
      this.Position = position;
      this.Velocity = velocity;
      this.Accelerations = accelerations ?? new List<ConstantAcceleration>();
      this.Forces = forces ?? new List<ConstantForce>();
      this.Acceleration = new Vector2(0,0);
    }
    //public Body(string id, float mass, IShape shape, Vector2 position) : this(id, mass, shape, position, new Vector2(0, 0)) { }
    //public Body(string id, float mass, IShape shape, Vector2 position, Vector2 velocity) : this(id, mass, shape, position, velocity, new Vector2(0, 0)) { }
    public string SerializeJSON(int indent = 0, bool singleLine = false) {
      if (singleLine) {
        throw new NotImplementedException();
      }
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = "{\n";
      text += indentText + $"\t\"id\": {this.ID.ToString(CultureInfo.InvariantCulture)},\n";
      text += indentText + $"\t\"mass\": {this.Mass.ToString(CultureInfo.InvariantCulture)},\n";
      text += indentText + $"\t\"shape\": {this.Shape.SerializeJSON(indent + 1)},\n";
      text += indentText + $"\t\"position\": {this.Position.SerializeJSON(indent + 1)},\n";
      text += indentText + $"\t\"velocity\": {this.Velocity.SerializeJSON(indent + 1)},\n";
      text += indentText + $"\t\"acceleration\": {this.Acceleration.SerializeJSON(indent + 1)}\n";
      text += indentText + "}";
      return text;
    }
    public Body SetState(Vector2 position, Vector2 velocity, Vector2 acceleration) {
      return new Body(this.ID, this.Mass, this.Shape, position, velocity, acceleration, this.Accelerations, this.Forces);
    }
  }
}
