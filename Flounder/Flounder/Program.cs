using System;
using Newtonsoft.Json;

namespace Flounder
{
    internal class Program
    {

        private static void Main(string[] args) { 
            String Jsonstring = InputParser.FileToJson("inputSchema.json");
            dynamic JsonObject = JsonConvert.DeserializeObject(Jsonstring);
            Simulation simulation = Simulation.ParseJSON(JsonObject);
            simulation.Tick();
            Console.WriteLine(simulation); 
        }
        
    }
}