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
        public static string FileToJson(string fileLocaton){
            try {
                StreamReader streamReader = new StreamReader(fileLocaton);
                string jsonString = streamReader.ReadToEnd();
                streamReader.Close();
                return jsonString;
            }
            catch (IOException e){
                Debug.WriteLine(e);
                return null;
            }
        }
        
        public static Simulation ParseInput(string jsonString){
            try {
                var jsonObject = JObject.Parse(jsonString);
                Simulation simulation = JsonConvert.DeserializeObject<Simulation>(jsonString);
                Debug.WriteLine(simulation);
            }
            catch (JsonReaderException exception){
                Debug.WriteLine(exception);
            }
            catch (JsonSerializationException excepton) {
                Debug.WriteLine(excepton);
            }

            List<Body> bodies = new List<Body>();
            return new Simulation(bodies);
        }
    }

}