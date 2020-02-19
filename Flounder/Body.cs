using System;

namespace Flounder
{

    public struct Body
    {
        private string id;
        private int mass;
        private IShape shape;
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 acceleration;
        
        public Body(string id,  int mass, IShape shape, Vector2 position, Vector2 velocity, Vector2 acceleration) {
            this.id = id;
            this.mass = mass;
            this.shape = shape;
            this.position = position;
            this.velocity = velocity;
            this.acceleration = acceleration;
        }
        public Body(string id,  int mass, IShape shape, Vector2 position) :
            this(id, mass, shape, position, new Vector2(0, 0)) { }

        public Body(string id, int mass, IShape shape, Vector2 position, Vector2 velocity) :
            this(id, mass, shape, position, velocity, new Vector2(0, 0)) { }
    }

}
