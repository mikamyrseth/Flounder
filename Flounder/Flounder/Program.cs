using System;
using Newtonsoft.Json;
namespace Flounder
{
  internal class Program
  {
    private static void Main(string[] args) {
      // string json = InputParser.FileToJson("inputSchema.json");
      string json = InputParser.FileToJson("C:\\Users\\leona\\Documents\\Sourcetree\\flounder\\Flounder\\Flounder\\inputSchema.json");
      // string json = InputParser.FileToJson("Vet ikke Sol sin fillokasjon");
      dynamic jso = JsonConvert.DeserializeObject(json);
      Simulation simulation = Simulation.ParseJSO(jso);
      simulation.Start();
      Console.WriteLine(simulation);
    }
  }
}