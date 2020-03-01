using System;
using Newtonsoft.Json;

namespace Flounder
{

  class Program
  {

    static void Main(string[] args) {
      string inputString = InputParser.FileToJson(
        "C:\\Users\\leona\\Documents\\Sourcetree\\flounder\\Flounder\\inputSchema.json"
      );
      dynamic input = JsonConvert.DeserializeObject(inputString);
      // Console.WriteLine(Vector2.ParseJSON(input));
      Console.WriteLine(Simulation.ParseJSON(input));
    }

  }

}
