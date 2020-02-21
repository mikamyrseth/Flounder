using System.Collections.Generic;

namespace Flounder
{
    
    public class Simulation
    {
        
        private List<Body> _bodies;

        public Simulation(List<Body> bodies) {
            this._bodies = bodies;
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