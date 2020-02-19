using System.Collections.Generic;

namespace Flounder
{
    public static class InputParser
    {
        public static Simulation ParseInput(string filename){
            
            
            List<Body> bodies = new List<Body>();
            return new Simulation(bodies);
        }
    }
}