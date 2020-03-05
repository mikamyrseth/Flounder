using System;
using System.Collections.Generic;
using System.Linq;

namespace Flounder
{
    public struct Body : IIndentedLogger
    {
        public static Body ParseJSON(dynamic JSON) {
            return new Body(
                (int) JSON.id,
                (int) JSON.mass,
                IShape.ParseJSON(JSON.shape),
                Vector2.ParseJSO(JSON.position),
                Vector2.ParseJSO(JSON.velocity),
                Vector2.ParseJSO(JSON.acceleration)
            );
        }

        private readonly int _mass;
        private readonly IShape _shape;
        private readonly Vector2 _position;
        private readonly Vector2 _velocity;
        private readonly Vector2 _acceleration;
        private List<ConstantForce> _forces;

        public int ID { get; }

        public Body(int id, int mass, IShape shape, Vector2 position, Vector2 velocity, Vector2 acceleration) {
            if (shape == null) throw new ArgumentException("Shape cannot be null");
            this.ID = id;
            this._mass = mass;
            this._shape = shape;
            this._position = position;
            this._velocity = velocity;
            this._acceleration = acceleration;
            this._forces = new List<ConstantForce>();
        }

        public Body(int id, int mass, IShape shape, Vector2 position) :
            this(id, mass, shape, position, new Vector2(0, 0)) { }

        public Body(int id, int mass, IShape shape, Vector2 position, Vector2 velocity) :
            this(id, mass, shape, position, velocity, new Vector2(0, 0)) { }

        public string ToString(int indent) {
            string indentText = string.Concat(Enumerable.Repeat("\t", indent));
            string text = indentText + "Body {\n";
            text += indentText + "\tmass: " + this._mass + "\n";
            text += indentText + "\tshape: " + this._shape.ToString() + "\n";
            text += indentText + "\tposition: " + this._position + "\n";
            text += indentText + "\tvelocity: " + this._velocity + "\n";
            text += indentText + "\tacceleration: " + this._acceleration + "\n";
            text += indentText + "}";
            return text;
        }

        public override string ToString() {
            return this.ToString(0);
        }
    }
}