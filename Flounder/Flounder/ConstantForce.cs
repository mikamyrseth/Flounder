using System;
using System.Linq;
namespace Flounder
{
  public class ConstantForce : ISerializableJSON
  {
    public ConstantForce(string id, Vector2 force) {
      this.Force = force;
      this.ID = id;
    }
    public Vector2 Force { get; }
    public string ID { get; }
    public string SerializeJSON(int indent = 0, bool singleLine = false) {
      if (singleLine) {
        throw new NotImplementedException();
      }
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = "{\n";
      text += indentText + $"\t\"id\": \"{this.ID}\",\n";
      text += indentText + $"\t\"vector\": {this.Force.SerializeJSON(indent + 1)}\n";
      text += indentText + "}";
      return text;
    }
    public static ConstantForce ParseJSO(dynamic jso) {
      return new ConstantForce((string) jso.id, Vector2.ParseJSO(jso.vector));
    }
  }
}
