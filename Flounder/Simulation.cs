using System.Collections.Generic;

namespace Flounder
{
    
    public class Simulation
    {
        
        private List<Body> _bodies;

        public Simulation(List<Body> bodies) {
            this._bodies = bodies;
        }

        public override string ToString(){
            string returnString = "Simulation with ";
            foreach (Body body in this._bodies){
                returnString+=body.ToString() + ", ";
            }
            return returnString;
        }

        public void Start(int ticks) {
            for (int i = 0; i < ticks; i++) {
                this.Tick();
            }
        }
        private void Tick() {
            
        }

    }
    
}