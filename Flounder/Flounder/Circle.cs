using System.Linq;

namespace Flounder
{
    public struct Circle : IShape
    {
        public static Circle ParseJSO(dynamic jso) {
            return new Circle((float) jso.radius);
        }

        private readonly float _radius;

        public float Radius { get { return this._radius; } }

        public Circle(float radius) {
            this._radius = radius;
        }

        public string ToString(int indent) {
            string indentText = string.Concat(Enumerable.Repeat("\t", indent));
            return indentText + $"Circle {{ radius: {this._radius} }}";
        }

        public override string ToString() {
            return this.ToString(0);
        }
    }
}
