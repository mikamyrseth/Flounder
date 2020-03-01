using System.Collections.Generic;
using Newtonsoft.Json;

namespace Flounder
{
    public class ConstantForce
    {
        private string _id;
        private Vector2 _force;

        public ConstantForce(string id, Vector2 force) {
            this._id = id;
            this._force = force;
        }


        [JsonConstructor]
        public ConstantForce(
                string id,
                int forceX,
                int forceY,
                List<string> bodies
        ):
            this(
                id,
                new Vector2(forceX, forceY)
            ) { } 

        public override string ToString (){
          return "cool force";
      }
    }
}