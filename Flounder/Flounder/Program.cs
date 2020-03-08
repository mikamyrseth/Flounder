using System;
using Newtonsoft.Json;

namespace Flounder
{
    internal class Program
    {

        private static void Main(string[] args) {
            Console.WriteLine(((IShape) new Rectangle(new Vector2(3.0f, 2.5f))).SerializeJSON(0));
        }
        
    }
}