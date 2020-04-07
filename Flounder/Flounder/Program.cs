using System;
using System.Collections.Generic;
using IF = Flounder.ImpliedFraction;
namespace Flounder
{
  internal class Program
  {
    private static void Main(string[] args) {
      StartSimulation(args);
      // Test(args);
    }
    private static void StartSimulation(string[] args) {
      if (args.Length < 2) {
        Console.WriteLine("Input file path and output file name are required as arguments!");
        return;
      }
      try {
        using Simulation simulation = new Simulation(args[0], args[1]);
        simulation.Start();
      } catch (KeyNotFoundException exception) {
        Console.WriteLine(exception.Message);
      }
    }
    private static void Test(string[] args) {
      IF.Precision = ImpliedFraction.PrecisionLevel.Micro;
      IF result;
      Console.WriteLine(IF.Parse("10.24"));
    }
  }
}
