using System;
using System.Linq;
namespace Flounder
{
    public class ConstantAcceleration : ISerializableJSON
    {
        public ConstantAcceleration(string id, Vector2 acceleration) {
            this.Acceleration = acceleration;
            this.ID = id;
        }
        public Vector2 Acceleration { get; }
        public string ID { get; }
        public string SerializeJSON(int indent = 0, bool singleLine = false) {
            if (singleLine) {
                throw new NotImplementedException();
            }
            string indentText = string.Concat(Enumerable.Repeat("\t", indent));
            string text = "{\n";
            text += indentText + $"\t\"id\": \"{this.ID}\",\n";
            text += indentText + $"\t\"vector\": {this.Acceleration.SerializeJSON(indent + 1)}\n";
            text += indentText + "}";
            return text;
        }
        public static ConstantAcceleration ParseJSO(dynamic jso) {
            return new ConstantAcceleration((string) jso.id, Vector2.ParseJSO(jso.vector));
        }
    }
}
