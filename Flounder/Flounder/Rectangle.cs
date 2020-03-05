using System.Linq;

namespace Flounder
{
    public struct Rectangle : IShape
    {
        public static Rectangle ParseJSON(dynamic JSON) {
            return new Rectangle(Vector2.ParseJSON(JSON.semiSize));
        }

        private readonly Vector2 _semiSize;

        public Rectangle(Vector2 semiSize) {
            this._semiSize = semiSize;
        }

        string IShape.SerializeJSON(int indent) { return IShape.SerializeJSON(indent, "rectangle", this.SerializeJSON(indent + 1)); }

        public string SerializeJSON(int indent) {
            string indentText = string.Concat(Enumerable.Repeat("\t", indent));
            string text = "{\n";
            text += indentText + $"\t\"semiSize\": {this._semiSize.SerializeJSON(indent + 1)}\n";
            text += indentText + "}";
            return text;
        }

        public string ToString(int indent) {
            string indentText = string.Concat(Enumerable.Repeat("\t", indent));
            return indentText + $"Rectangle {{ semiSize: {this._semiSize} }}";
        }

        public override string ToString() {
            return this.ToString(0);
        }

    }
}