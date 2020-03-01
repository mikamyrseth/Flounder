using System.Collections.Generic;

namespace Flounder
{
    
    public class Simulation
    {
        
        //private List<Body> _bodies;
        private SortedDictionary<string, Body> _bodies;
        private double _deltaTime;
        private int _ticks;
        private int _ticksPassed;

        public Body getBodyByID(string bodyID){
            return _bodies[bodyID];
        }

        public Simulation(SortedDictionary<string, Body> bodies, double deltaTime, int ticks) {
            this._bodies = bodies;
            this._deltaTime = deltaTime;
            this._ticks = ticks;
        }

        public override string ToString(){
            string returnString = "Simulation with ";
            foreach (Body body in this._bodies.Values){
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
            this._ticksPassed ++;
            foreach (Body body in this._bodies.Values){
                body.Tick(_deltaTime);
            }
        }

    }
    
}