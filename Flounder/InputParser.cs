using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Flounder
{
    public static class InputParser
    {
        public static string FileToJson(string fileLocation) {
            StreamReader streamReader = null;
            try {
                FileStream fileStream = new FileStream(fileLocation, FileMode.Open);
                streamReader = new StreamReader(fileStream);
                return streamReader.ReadToEnd();;
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                Debug.WriteLine(exception);
                return null;
            }
            finally {
                streamReader?.Close();
            }
        }
        
        public static Simulation ParseInput(string jsonString) {
            List<Body> bodies = new List<Body>();
            try {
                
            }
            catch (JsonReaderException exception){
                Debug.WriteLine(exception);
            }
            catch (JsonSerializationException exception) {
                Debug.WriteLine(exception);
            }

            return new Simulation(null);
        }
    }

}