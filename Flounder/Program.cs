using System;

namespace Flounder
{

  class Program
  {

    static void Main(string[] args) { 
      Console.WriteLine("Hello World!"); 
      InputParser.ParseInput(InputParser.FileToJson("inputSchema.json"));
    }

  }

}