using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace FlounderRender
{

  public class OutputLineReader : IDisposable
  {

    private readonly string _commentPrefix;
    private readonly TextReader _reader;

    public OutputLineReader(string commentPrefix, TextReader reader) {
      this._commentPrefix = commentPrefix;
      this._reader = reader ?? throw new ArgumentException("Text reader is required for OutputLineReader!");
    }

    public void Dispose() {
      this._reader?.Dispose();
    }

    public bool NextLine(out string line) {
      line = null;
      while (line == null && this._reader.Peek() != -1) {
        string newLine = this._reader.ReadLine();
        if (newLine == null || (this._commentPrefix != null && newLine.StartsWith(this._commentPrefix))) { continue; }
        line = newLine;
        return true;
      }
      return false;
    }
    
  }

}