using System.Collections.Generic;

namespace Flounder
{
    public class Simulation
    {
        private List<Body> _physicsObjects;

        public Simulation(List<Body> physicsObjects) {
            this._physicsObjects = physicsObjects;
        }

    }
}