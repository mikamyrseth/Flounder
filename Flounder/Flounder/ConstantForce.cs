using System.Linq;

namespace Flounder
{

  public class ConstantForce : ISerializableJSON
  {

    public Vector2 Force { get; }
    public string ID { get; }

    public ConstantForce(string id, Vector2 force) {
      this.Force = force;
      this.ID = id;
    }

    public string SerializeJSON(int indent) {
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = "{\n";
      text += indentText + $"\t\"id\": \"{this.ID}\",\n";
      text += indentText + $"\t\"force\": {this.Force.SerializeJSON(indent + 1)}\n";
      text += indentText + "}";
      return text;
    }

    public static ConstantForce ParseJSO(dynamic jso) { return new ConstantForce((string)jso.id, Vector2.ParseJSO(jso.force)); }

  }

}