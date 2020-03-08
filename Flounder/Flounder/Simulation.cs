using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Flounder
{
    public class Simulation : IIndentedLogger
    {
        private readonly SortedDictionary<string, Body> _bodies = new SortedDictionary<string, Body>();
        private readonly float _deltaT; 

        private List<ConstantForce> _constantForces;
        
        public Simulation(SortedDictionary<string, Body> bodies, float deltaT, List<ConstantForce> constantForces) {
            _bodies = bodies;
            _deltaT = deltaT;
            _constantForces = constantForces;
        }

        public string ToString(int indent) {
            string indentText = string.Concat(Enumerable.Repeat("\t", indent));
            string text = indentText + "Simulation { bodies: [\n";
            foreach (Body body in this._bodies.Values) text += indentText + body.ToString(indent + 1) + ",\n";
            text += indentText + "] }";
            return text;
        }

        public override string ToString() {
            return this.ToString(0);
        }

        public static Simulation ParseJSON(dynamic JSON) {
            SortedDictionary<string, Body> bodies = new SortedDictionary<string, Body>();
            foreach (JObject bodyJSON in JSON.bodies) {
                Body body = Body.ParseJSO(bodyJSON);
                bodies.Add(body.ID, body);
            }

            float deltaT = JSON.deltaT;
            
            List<ConstantForce> constantForces = new List<ConstantForce>();
            foreach (dynamic forceJSON in JSON.constantForces) {
                ConstantForce constantForce = ConstantForce.ParseJSO(forceJSON);
                constantForces.Add(constantForce);
                foreach(string bodyID in forceJSON.bodies){
                    if(bodies.ContainsKey(bodyID)){
                        (bodies[bodyID]).addConstantForce(constantForce);
                    }
                }
            }
            
            return new Simulation(bodies, deltaT, constantForces);
        }

        public Body GetBody(string bodyID) {
            return this._bodies[bodyID];
        }

        public void Start(int ticks) {
            for (int i = 0; i < ticks; i++) this.Tick();
        }

        private void Tick() {
            foreach(Body body in _bodies.Values){
                body.Tick(_deltaT);
            }
        }
    }
}