using System.Numerics;

namespace Flounder
{
    public struct Vector2
    {

        public static Vector2 operator +(Vector2 a, Vector2 b) {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        
        public static Vector2 operator -(Vector2 a, Vector2 b) {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator *(float s, Vector2 a) {
            return new Vector2(s * a.x, s * a.y);
        }

        public static float operator *(Vector2 a, Vector2 b) {
            return a.x * b.x + a.y * b.y;
        }

        public static Vector2 operator /(Vector2 a, float s) {
            return new Vector2(a.x / s, a.y / s);
        }

        private float x;
        private float y;

        public Vector2(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public float norm2() {
            return this.x * this.x + this.y * this.y;
        }
    }
}