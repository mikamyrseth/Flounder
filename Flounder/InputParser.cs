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
    }
}