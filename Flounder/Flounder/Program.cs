using System;
namespace Flounder
{
  internal class Program
  {
    private static void Main(string[] args) {
      StartSimulation(args);
    }
    private static void StartSimulation(string[] args) {
      if (args.Length < 2) {
        Console.WriteLine("Input file path and output file name are required as arguments!");
        return;
      }
      using Simulation simulation = new Simulation(args[0], args[1]);
      simulation.Start();
    }
    private static void Test(string[] args) {
      Console.WriteLine(Rectangle.ParseCSV("Rectangle, 0.4, 3").SerializeJSON(singleLine: true));
      Console.WriteLine(Rectangle.ParseCSV("0.4, 3").SerializeJSON(singleLine: true));
    }
  }
}