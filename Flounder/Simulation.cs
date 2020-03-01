using System;
using System.Collections.Generic;
using System.Linq;

namespace Flounder
{
    
    public class Simulation : IIndentedLogger
    {

        public static Simulation ParseJSON(dynamic JSON) {
            SortedDictionary<int, Body> bodies = new SortedDictionary<int, Body>();
            foreach (Newtonsoft.Json.Linq.JObject bodyJSON in JSON.bodies)
            {
                Body body = Body.ParseJSON(bodyJSON);
                bodies[body.ID] = body;
            }
            return new Simulation(bodies);
        }
        
	private const SortedDictionary<string, Body> _bodies;

    public Body getBodyByID(string bodyID){
        return _bodies[bodyID];
    }

    public Simulation(SortedDictionary<string, Body> bodies) {
        this._bodies = bodies;
    }
	public string ToString(int indent) {
            string indentText = String.Concat(Enumerable.Repeat("\t", indent));
            string text = indentText + "Simulation { bodies: [\n";
            foreach (Body body in this._bodies) {
                text += indentText + body.ToString(indent + 1) + ",\n";
            }
            text += indentText + "] }";
            return text;
        }

        public override string ToString() {
            return this.ToString(0);
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
