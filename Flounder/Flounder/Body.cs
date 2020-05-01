using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace Flounder
{
  public class Body : FlounderObject, ISerializableJSON
  {
    public static Body ParseJSO(dynamic jso) {
      Body body = new Body(
        (string)(jso.id ?? throw new KeyNotFoundException("Key \"id\" was expected in input JSON file!")), 
        (int)(jso.mass ?? throw new KeyNotFoundException("Key \"mass\" was expected in input JSON file!")),
        IShape.ParseJSO(jso.shape ?? throw new KeyNotFoundException("Key \"shape\" was expected in input JSON file!"))
      );
      body.Position = Vector2.ParseJSO(jso.position ?? throw new KeyNotFoundException("Key \"position\" was expected in input JSON file!")); 
      body.Velocity = Vector2.ParseJSO(jso.velocity ?? throw new KeyNotFoundException("Key \"velocity\" was expected in input JSON file!")); 
      return body;
    }
    public List<ConstantAcceleration> Accelerations { get; }
    public List<ConstantForce> Forces { get; }
    public float Mass { get; }
    public IShape Shape { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 NextPosition { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Acceleration { get; set; }
    public BoundingBox BoundingBox {
      get {
        BoundingBox shapeBoundingBox = this.Shape.OffsetBoundingBox;
        return (shapeBoundingBox + this.Position) + (shapeBoundingBox + this.NextPosition);
      }
    }

    public Body(string id, float mass, IShape shape, List<ConstantAcceleration> accelerations = null, List<ConstantForce> forces = null)
      : base(id)
    {
      this.Accelerations = accelerations ?? new List<ConstantAcceleration>();
      this.Forces = forces ?? new List<ConstantForce>();
      this.Mass = mass;
      this.Shape = shape ?? throw new ArgumentException("Shape cannot be null");
      this.Position = new Vector2();
      this.Velocity = new Vector2();
      this.Acceleration = new Vector2();
    }

    public void CalculateNextPosition(float timeInterval) {
      this.Acceleration = this.Forces.Aggregate(new Vector2(), (current, force) => current + force.Force) / this.Mass;
      this.Acceleration += this.Accelerations.Aggregate(new Vector2(), (current, acceleration) => current + acceleration.Acceleration);
      this.Velocity += timeInterval * this.Acceleration;
      this.NextPosition = this.Position + timeInterval * this.Velocity;
    }
    public void Commit() {
      this.Position = this.NextPosition;
    }
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
  }
}
