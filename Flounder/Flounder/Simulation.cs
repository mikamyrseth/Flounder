using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Flounder
{
  public class Simulation : IDisposable
  {
    public enum FileFormat
    {
      FLO,
      FLOD
    }
    private const string FLOFileExtension = "flo";
    private const string FLOVersion = "flo v1.0.2";
    private const string FLODFileExtension = "flod";
    private const string FLODVersion = "flod v1.0.0";
    private readonly Body[] _bodies;
    private readonly List<ConstantForce> _constantForces = new List<ConstantForce>();
    private readonly List<ConstantAcceleration> _constantAccelerations = new List<ConstantAcceleration>();
    private readonly FileFormat _fileFormat;
    private readonly StreamWriter _fileWriter;
    private float _duration;
    private float _time;
    private readonly float _timeInterval;
    private Simulation(float timeInterval) {
      this._timeInterval = timeInterval;
    }
    
    public string ToString(int indent) {
      string indentText = string.Concat(Enumerable.Repeat("\t", indent));
      string text = indentText + "Simulation { bodies: [\n";
      foreach (Body body in this._bodies) {
        text += indentText + body.SerializeJSON() + ",\n";
      }
      return text;
    }
    
    public void Dispose() {
      this._fileWriter?.Dispose();
    }
    public Simulation(string inputFilePath, string outputFileName, FileFormat fileFormat = FileFormat.FLO) {
      this._fileFormat = fileFormat;
      #region File setup
      string json = File.ReadAllText(inputFilePath);
      string outputFilePath = outputFileName + "." + (this._fileFormat == FileFormat.FLO ? FLOFileExtension : FLODFileExtension);
      this._fileWriter = new StreamWriter(File.Create(outputFilePath));
      #endregion
      #region Parse input
      dynamic jso = JsonConvert.DeserializeObject(json);
      this._duration = (float)(jso.duration ?? throw new KeyNotFoundException("Key \"duration\" was expected in input JSON file!"));
      this._time = 0;
      if (!Enum.TryParse((string)(jso.precision ?? throw new KeyNotFoundException("Key \"precision\" was expected in input JSON file!")), true, out ImpliedFraction.PrecisionLevel precision)) {
        throw new FormatException("Precision value could not be parsed to PrecisionLevel!");
      }
      ImpliedFraction.Precision = precision;
      this._timeInterval = (float)(jso.timeInterval ?? throw new KeyNotFoundException("Key \"timeInterval\" was expected in input JSON file!"));
      #region Bodies
      dynamic bodiesJso = jso.bodies ?? throw new KeyNotFoundException("Key \"bodies\" was expected in input JSON file!");
      SortedList<string, Body> bodies = new SortedList<string, Body>();
      foreach (JObject bodyJSO in bodiesJso) {
        Body body = Body.ParseJSO(bodyJSO);
        bodies.Add(body.ID, body);
      }
      #endregion
      #region Constant forces
      dynamic forcesJso = jso.constantForces ?? throw new KeyNotFoundException("Key \"constantForces\" was expected in input JSON file!");
      foreach (dynamic forceJSO in forcesJso) {
        ConstantForce constantForce = ConstantForce.ParseJSO(forceJSO);
        this._constantForces.Add(constantForce);
        dynamic forceBodiesJSO = forceJSO.bodies ?? throw new KeyNotFoundException("Key \"bodies\" was expected as a key in \"constantForces\" in input JSON file!");
        foreach (string bodyID in forceJSO.bodies) {
          if (bodies.ContainsKey(bodyID)) {
            bodies[bodyID].Forces.Add(constantForce);
          }
        }
      }
      dynamic accelerationsJSO = jso.constantAccelerations ?? throw new KeyNotFoundException("Key \"constantAccelerations\" was expected in input JSON file!");
      foreach (dynamic accelerationJSO in accelerationsJSO) {
        ConstantAcceleration constantAcceleration = ConstantAcceleration.ParseJSO(accelerationJSO);
        this._constantAccelerations.Add(constantAcceleration);
        dynamic accelerationBodiesJSO = accelerationJSO.bodies ?? throw new KeyNotFoundException("Key \"bodies\" was expected as a key in \"constantAccelerations\" in input JSON file!");
        foreach (string bodyID in accelerationBodiesJSO) {
          if (bodies.ContainsKey(bodyID)) {
            bodies[bodyID].Accelerations.Add(constantAcceleration);
          }
        }
      }
      #endregion
      this._bodies = new Body[bodies.Count];
      int i = 0;
      foreach (Body body in bodies.Values) {
        this._bodies[i++] = body;
      }
      #endregion
      #region Output setup
      this._fileWriter.WriteLine(this._fileFormat == FileFormat.FLO ? FLOVersion : FLODVersion);
      this._fileWriter.WriteLine(this._bodies.Length.ToString(CultureInfo.InvariantCulture));
      foreach (Body body in this._bodies) {
        switch (this._fileFormat) {
          case FileFormat.FLO:
            this._fileWriter.WriteLine($"# {body.ID}");
            this._fileWriter.WriteLine(body.Shape.SerializeCSV());
            break;
          case FileFormat.FLOD:
            this._fileWriter.WriteLine($"\"{body.ID}\", \"{body.Shape.SerializeJSON(singleLine: true)}\"");
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      #endregion
    }
    private void RecordFrame() {
      this._fileWriter.WriteLine(this._time.ToString(CultureInfo.InvariantCulture));
      foreach (Body body in this._bodies) {
        switch (this._fileFormat) {
          case FileFormat.FLO:
            this._fileWriter.WriteLine($"\t{body.Position.X.ToString(CultureInfo.InvariantCulture)}, {body.Position.Y.ToString(CultureInfo.InvariantCulture)}");
            break;
          case FileFormat.FLOD:
            this._fileWriter.WriteLine($"\"{body.ID}\", {body.Position.X.ToString(CultureInfo.InvariantCulture)}, {body.Position.Y.ToString(CultureInfo.InvariantCulture)},  {body.Velocity.X.ToString(CultureInfo.InvariantCulture)}, {body.Velocity.Y.ToString(CultureInfo.InvariantCulture)}, {body.Acceleration.X.ToString(CultureInfo.InvariantCulture)}, {body.Acceleration.Y.ToString(CultureInfo.InvariantCulture)}");
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }
    public void Start() {
      this.Tick(0);
      this.RecordFrame();
      while (this._duration > 0) {
        this.Tick(this._timeInterval);
        this._duration -= this._timeInterval;
        this._time += this._timeInterval;
        this.RecordFrame();
      }
    }
    private void Tick(float timeInterval) {
      List<BoundingBox> boundingBoxes = new List<BoundingBox>();
      Dictionary<string, (Vector2, Vector2, Vector2)> futureState = new Dictionary<string, (Vector2, Vector2, Vector2)>();
      for (int i = 0; i < this._bodies.Length; i++) { // For every body
        Body body = this._bodies[i];
        Vector2 forceSum = body.Forces.Aggregate(new Vector2(), (current, force) => current + force.Force);
        Vector2 acceleration = forceSum / body.Mass;
        acceleration += body.Accelerations.Aggregate(new Vector2(), (current, acceleration) => current + acceleration.Acceleration);
        Vector2 velocity = body.Velocity + timeInterval * acceleration;
        Vector2 position = body.Position + timeInterval * velocity;
        futureState.Add(body.ID, (position, velocity, acceleration)); // Get a new body with new position 
        Vector2 axisAlignedSizeBefore = body.Shape.AxisAlignedSize;
        float minXBefore = body.Position.X - axisAlignedSizeBefore.X / 2;
        float minYBefore = body.Position.Y - axisAlignedSizeBefore.Y / 2;
        float maxXBefore = body.Position.X + axisAlignedSizeBefore.X / 2;
        float maxYBefore = body.Position.Y + axisAlignedSizeBefore.Y / 2;
        body = body.SetState(position, velocity, acceleration);
        Vector2 axisAlignedSizeLater = body.Shape.AxisAlignedSize;
        float minXLater = body.Position.X - axisAlignedSizeLater.X / 2;
        float minYLater = body.Position.Y - axisAlignedSizeLater.Y / 2;
        float maxXLater = body.Position.X + axisAlignedSizeLater.X / 2;
        float maxYLater = body.Position.Y + axisAlignedSizeLater.Y / 2;
        float minX = Math.Min(minXBefore, minXLater);
        float minY = Math.Min(minYBefore, minYLater);
        float maxX = Math.Max(maxXBefore, maxXLater);
        float maxY = Math.Max(maxXBefore, maxXLater);
        float sizeX = maxX - minX;
        float sizeY = maxY - minY;
        boundingBoxes.Add(new BoundingBox(body, new Vector2(minX, minY), new Vector2(sizeX, sizeY)));                              // Switch to new body in simulation
      }
      for (int i = 0; i < boundingBoxes.Count; i++) { 
        BoundingBox boundingBox1 = boundingBoxes[i];
        for (int j = i + 1; j < boundingBoxes.Count; j++) {
          
        }
        
      }
      for (int i = 0; i < this._bodies.Length; i++) { 
        Body body = this._bodies[i];
        (Vector2, Vector2, Vector2) newState = futureState[body.ID];
        body = body.SetState(newState.Item1, newState.Item2, newState.Item3);
        this._bodies[i] = body;
      }
    }
    private void Collision(Body body1, Body body2){
      Vector2 v_a1 = body1.Velocity;
      Vector2 v_b1 = body2.Velocity;
      float m_a = body1.Mass;
      float m_b = body2.Mass;

      Vector2 v_a2 = (((2 * m_a) * v_a1) - (m_a * v_b1) + (m_b * v_b1)) / (m_a + m_b);
      Vector2 v_b2 = ((m_a * v_a1) - (m_b * v_a1) + ((2 * m_b) * v_b1)) / (m_a + m_b);
      
      body1.SetState(body1.Position, v_a2, body1.Acceleration);
      body2.SetState(body2.Position, v_b2, body2.Acceleration);
    }
  }
}
