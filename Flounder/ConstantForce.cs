using System.Collections.Generic;

namespace Flounder
{
    public struct ConstantForce
    {
        private Vector2 _force;
        private List<Body> _bodies;

        public ConstantForce(Vector2 force, List<Body> bodies) {
            this._force = force;
            this._bodies = bodies;
        }

    }
}