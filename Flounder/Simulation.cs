using System.Collections.Generic;

namespace Flounder
{
    public class Simulation
    {
        private List<Body> bodies;

        public Simulation(List<Body> bodies) {
            this.bodies = bodies;
        }

        public override string ToString(){
            string retrunString = "Simulation with ";
            foreach (var body in this.bodies){
                retrunString+=body.ToString();
            }
            return retrunString;
        }

    }
}