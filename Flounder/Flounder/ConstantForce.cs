using System.Collections.Generic;
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
      dynamic idJSO = jso.id ?? throw new KeyNotFoundException("Key \"id\" was expected as a key in \"constantFroces\" in input JSON file!");
      dynamic vectorJSO = jso.vector ?? throw new KeyNotFoundException("Key \"vector\" was expected as a key in \"constantForces\" in input JSON file!");
      return new ConstantForce((string)idJSO, Vector2.ParseJSO(vectorJSO));
    }
  }
}