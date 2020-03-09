using System.Linq;
using Newtonsoft.Json;

namespace Flounder
{
    public struct Rectangle : IShape
    {
        public static Rectangle ParseJSO(dynamic JSON) {
            return new Rectangle(Vector2.ParseJSO(JSON.semiSize));
        }

        private readonly Vector2 _semiSize;

        public float Height { get { return 2 * this.SemiHeight; } }
        public float SemiHeight { get { return this._semiSize.Y; } }
        public Vector2 SemiSize { get { return this._semiSize; } }
        public float SemiWidth { get { return this._semiSize.X; } }
        public float Width { get { return 2 * this.SemiWidth; } }

        public Rectangle(Vector2 semiSize) {
            this._semiSize = semiSize;
        }

        string IShape.SerializeJSON(int indent, bool singleLine) {
            return IShape.SerializeJSON("rectangle", this.SerializeJSON(indent + 1, singleLine), indent, singleLine);
        }

        public string SerializeJSON(int indent = 0, bool singleLine = false) {
            if (singleLine) {
                return $"{{ \"semiSize\": {this._semiSize.SerializeJSON(singleLine: singleLine)} }}";
            } else {
                string indentText = string.Concat(Enumerable.Repeat("\t", indent));
                string text = "{\n";
                text += indentText + $"\t\"semiSize\": {this._semiSize.SerializeJSON(indent + 1)}\n";
                text += indentText + "}";
                return text;
            }
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