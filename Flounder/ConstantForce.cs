using System.Collections.Generic;
using Newtonsoft.Json;

namespace Flounder
{
    public class ConstantForce
    {
        private string _id;
        private Vector2 _force;

        public Vector2 getVector(){
            return this._force;
        }

        public void setVector(Vector2 vector){
            _force = vector;
        }

        public ConstantForce(string id, Vector2 force) {
            this._id = id;
            this._force = force;
        }
        
        public override string ToString (){
          return "cool force";
      }
    }
}