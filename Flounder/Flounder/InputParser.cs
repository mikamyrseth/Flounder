using System;
using System.Diagnostics;
using System.IO;

namespace Flounder
{

  public static class InputParser
  {

    public static string FileToJson(string fileLocation) {
      StreamReader streamReader = null;
      try {
        FileStream fileStream = new FileStream(fileLocation, FileMode.Open);
        streamReader = new StreamReader(fileStream);
        return streamReader.ReadToEnd();
        ;
      } catch (Exception exception) {
        Console.WriteLine(exception);
        Debug.WriteLine(exception);
        return null;
      } finally { streamReader?.Close(); }
    }

  }

}