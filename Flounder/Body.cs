using System;
using Newtonsoft.Json;
using System.Diagnostics;

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
            if (shape == null) {
                throw new System.ArgumentException("Shape cannot be null");
            }
            Debug.WriteLine(shape.ToString());
            this._id = id;
            this._mass = mass;
            this._shape = shape;
            this._position = position;
            this._velocity = velocity;
            this._acceleration = acceleration;
        }

        [JsonConstructor]
        public Body(
                string id, 
                int positionX, 
                int positionY, 
                int mass, 
                IShape shape, 
                int accelerationX, 
                int accelerationY, 
                int velocityX,
                int velocityY
        ):
            this(
                id, 
                mass, 
                shape, 
                new Vector2(positionX, positionY), 
                new Vector2(velocityX, velocityY),
                new Vector2(accelerationX, accelerationY)
            ) { } 
 
        public override string ToString(){
            return ("Cool object of type " + _shape.ToString());
        }

        public Body(string id,  int mass, IShape shape, Vector2 position) :
            this(id, mass, shape, position, new Vector2(0, 0)) { }
        public Body(string id, int mass, IShape shape, Vector2 position, Vector2 velocity) :
            this(id, mass, shape, position, velocity, new Vector2(0, 0)) { }
    }

}
