using System.Linq;

namespace Flounder
{
    public struct Vector2
    {
        public static Vector2 ParseJSON(dynamic JSON) {
            return new Vector2((float) JSON.x, (float) JSON.y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b) {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator *(float s, Vector2 a) {
            return new Vector2(s * a.X, s * a.Y);
        }

        public static float operator *(Vector2 a, Vector2 b) {
            return a.X * b.X + a.Y * b.Y;
        }

        public static Vector2 operator /(Vector2 a, float s) {
            return new Vector2(a.X / s, a.Y / s);
        }

        public float SquareLength {
            get { return this * this; }
        }

        public float X { get; }
        public float Y { get; }

        public Vector2(float x, float y) {
            this.X = x;
            this.Y = y;
        }

        public string SerializeJSON(int indent) {
            string indentText = string.Concat(Enumerable.Repeat("\t", indent));
            string text = indentText + "{\n";
            text += indentText + $"\t\"x\": {this.X},\n";
            text += indentText + $"\t\"y\": {this.Y}\n";
            text += indentText + "}";
            return text;
        }

        public override string ToString() {
            return $"({this.X}, {this.Y})";
        }
    }
}