using System.Globalization;
using System.Linq;

namespace Flounder
{
    public interface IShape : IIndentedLogger, ISerializableJSON
    {
        
        public static IShape ParseJSO(dynamic jso) {
            if (jso.circle != null) {
                return Circle.ParseJSO(jso.circle);
            }
            if (jso.rectangle != null) {
                return Rectangle.ParseJSO(jso.rectangle);
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
