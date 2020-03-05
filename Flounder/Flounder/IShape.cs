using System.Globalization;
using System.Linq;

namespace Flounder
{
    public interface IShape : IIndentedLogger, ISerializableJSON
    {
        
        public static IShape ParseJSON(dynamic json) {
            if (json.circle != null) {
                return Circle.ParseJSON(json.circle);
            }
            if (json.rectangle != null) {
                return Rectangle.ParseJSON(json.rectangle);
            }
            return null;
        }

        public static string SerializeJSON(int indent, string shapeName, string shapeJSON) {
            string indentText = string.Concat(Enumerable.Repeat("\t", indent));
            string text = "{\n";
            text += indentText + $"\t\"{shapeName}\": {shapeJSON}\n";
            text += indentText + "}";
            return text;
        }

        new string SerializeJSON(int indent);

    }
}