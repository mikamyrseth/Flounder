using System.Linq;

namespace Flounder
{
    internal struct Circle : IIndentedLogger, IShape
    {
        public static Circle ParseJSON(dynamic JSON) {
            return new Circle((int) JSON.radius);
        }

        private readonly int _radius;

        public Circle(int radius) {
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