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
            string retrunString = "Simulation with ";
            foreach (var body in this._bodies){
                retrunString+=body.ToString();
            }
            return retrunString;
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