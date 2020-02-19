using System;

namespace Flounder
{

    public class Body
    {
        private string id;
        private int x;
        private int y;
        private int mass;
        private IShape shape;
        
        public Body(string id, int x, int y, int mass, IShape shape){
            this.id = id;
            this.x = x;
            this.y = y;
            this.mass = mass;
            this.shape = shape;
        }
    }

}
