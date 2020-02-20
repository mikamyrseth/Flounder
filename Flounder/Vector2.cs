namespace Flounder
{
    public struct Vector2
    {

        public static Vector2 operator +(Vector2 a, Vector2 b) {
            return new Vector2(a._x + b._x, a._y + b._y);
        }
        
        public static Vector2 operator -(Vector2 a, Vector2 b) {
            return new Vector2(a._x - b._x, a._y - b._y);
        }

        public static Vector2 operator *(float s, Vector2 a) {
            return new Vector2(s * a._x, s * a._y);
        }

        public static float operator *(Vector2 a, Vector2 b) {
            return a._x * b._x + a._y * b._y;
        }

        public static Vector2 operator /(Vector2 a, float s) {
            return new Vector2(a._x / s, a._y / s);
        }

        private float _x;
        private float _y;

        public float SquareLength { get { return this * this; } }
        public float X { get { return this._x; } }
        public float Y { get { return this._y; } }

        public Vector2(float x, float y) {
            this._x = x;
            this._y = y;
        }
    }
}