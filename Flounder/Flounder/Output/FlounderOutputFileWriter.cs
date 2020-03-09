using System;
using System.Collections.Generic;
using System.IO;

namespace Flounder.Output
{
    public class FlounderOutputFileWriter : IDisposable
    {
        
        private const string Version = "flo 1.0.0";

        private readonly HashSet<string> _bodyIDs = new HashSet<string>();
        private readonly StreamWriter _streamWriter;

        private bool _isConfigurationsFinished = false;

        public FlounderOutputFileWriter(string filePath) {
            this._streamWriter = new StreamWriter(filePath);
            this._streamWriter.WriteLine(Version);
        }

        public void AddBody(string bodyID, IShape shape = null) {
            if (shape == null) {
                this._bodyIDs.Add(bodyID ?? throw new ArgumentNullException());
                this._streamWriter.WriteLine($"\"{bodyID}\"");
            } else {
                this._bodyIDs.Add(bodyID ?? throw new ArgumentNullException());
                this._streamWriter.WriteLine($"\"{bodyID}\" \"{shape.SerializeJSON(singleLine: true)}\"");
            }
        }

        public void Dispose() {
            this._streamWriter?.Dispose();
        }
    }
}