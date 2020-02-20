using System;

namespace Flounder
{

    public struct Body
    {
        private string _id;
        private int _mass;
        private IShape _shape;
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;
        
        public Body(string id,  int mass, IShape shape, Vector2 position, Vector2 velocity, Vector2 acceleration) {
            this._id = id;
            this._mass = mass;
            this._shape = shape;
            this._position = position;
            this._velocity = velocity;
            this._acceleration = acceleration;
        }
        public Body(string id,  int mass, IShape shape, Vector2 position) :
            this(id, mass, shape, position, new Vector2(0, 0)) { }
        public Body(string id, int mass, IShape shape, Vector2 position, Vector2 velocity) :
            this(id, mass, shape, position, velocity, new Vector2(0, 0)) { }
    }

}
