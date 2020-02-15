using System.Collections.Generic;

namespace Flounder
{
    public class Simulation
    {
        private List<PhysicsObject> _physicsObjects;

        public Simulation(List<PhysicsObject> physicsObjects) {
            this._physicsObjects = physicsObjects;
        }

    }
}