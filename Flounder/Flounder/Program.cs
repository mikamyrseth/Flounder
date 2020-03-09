using System;

namespace Flounder
{

  internal class Program
  {

    private static void Main(string[] args) {
      if (args.Length < 2) {
        Console.WriteLine("Input file path and output file name are required as arguments!");
        return;
      }
      using Simulation simulation = new Simulation(args[0], args[1]);
      Console.WriteLine(simulation);
    }

  }

}