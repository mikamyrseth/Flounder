using System;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;

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
        private List<ConstantForce> _forces;
        
        public Body(string id,  int mass, IShape shape, Vector2 position, Vector2 velocity, Vector2 acceleration) {
            if (shape == null) {
                throw new System.ArgumentException("Shape cannot be null");
            }
            this._id = id;
            this._mass = mass;
            this._shape = shape;
            this._position = position;
            this._velocity = velocity;
            this._acceleration = acceleration;
            this._forces = new List<ConstantForce>();
        }

        public Body(string id,  int mass, IShape shape, Vector2 position) :
            this(id, mass, shape, position, new Vector2(0, 0)) { }
        public Body(string id, int mass, IShape shape, Vector2 position, Vector2 velocity) :
            this(id, mass, shape, position, velocity, new Vector2(0, 0)) { }
    }

}
