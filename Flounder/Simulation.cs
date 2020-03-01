using System;
using System.Collections.Generic;
using System.Linq;

namespace Flounder
{
    
    public class Simulation : IIndentedLogger
    {

        public static Simulation ParseJSON(dynamic JSON) {
            List<Body> bodies = new List<Body>();
            foreach (Newtonsoft.Json.Linq.JObject bodyJSON in JSON.bodies)
            {
                bodies.Add(Body.ParseJSON(bodyJSON));
            }
            return new Simulation(bodies);
        }
        
	private readonly SortedDictionary<int, Body> _bodies = new SortedDictionary<int, Body>();

    public Body GetBody(int bodyID){
        return this._bodies[bodyID];
    }

    public Simulation(List<Body> bodies) {
        foreach (Body body in bodies) {
            this._bodies[body.ID] = body;
        }
    }
	public string ToString(int indent) {
            string indentText = String.Concat(Enumerable.Repeat("\t", indent));
            string text = indentText + "Simulation { bodies: [\n";
            foreach (Body body in this._bodies.Values) {
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
