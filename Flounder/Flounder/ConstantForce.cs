namespace Flounder
{
    public class ConstantForce
    {
        private Vector2 _force;
        private string _id;

        public ConstantForce(string id, Vector2 force) {
            this._id = id;
            this._force = force;
        }

        public override string ToString() {
            return "cool force";
        }
    }
}