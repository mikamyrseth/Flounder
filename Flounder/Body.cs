using System;
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
                Vector2.ParseJSON(JSON.position),
                Vector2.ParseJSON(JSON.velocity),
                Vector2.ParseJSON(JSON.acceleration)
            );
        }
        
        private int _id;
        private int _mass;
        private IShape _shape;
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;
        
        public Body(int id,  int mass, IShape shape, Vector2 position, Vector2 velocity, Vector2 acceleration) {
            this._id = id;
            this._mass = mass;
            this._shape = shape;
            this._position = position;
            this._velocity = velocity;
            this._acceleration = acceleration;
        }
        public Body(int id,  int mass, IShape shape, Vector2 position) :
            this(id, mass, shape, position, new Vector2(0, 0)) { }
        public Body(int id, int mass, IShape shape, Vector2 position, Vector2 velocity) :
            this(id, mass, shape, position, velocity, new Vector2(0, 0)) { }

        public string ToString(int indent) {
            string indentText = String.Concat(Enumerable.Repeat("\t", indent));
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
