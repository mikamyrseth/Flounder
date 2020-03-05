using System.Linq;

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

        public string ToString(int indent) {
            string indentText = string.Concat(Enumerable.Repeat("\t", indent));
            return indentText + $"Rectangle {{ semiSize: {this._semiSize} }}";
        }

        public override string ToString() {
            return this.ToString(0);
        }
    }
}