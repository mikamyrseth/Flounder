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

        public string ToString(int indent) {
            string indentText = string.Concat(Enumerable.Repeat("\t", indent));
            return indentText + $"Rectangle {{ semiSize: {this._semiSize} }}";
        }

        public override string ToString() {
            return this.ToString(0);
        }
    }
}