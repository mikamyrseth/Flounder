using System.Security.Cryptography;
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
    private readonly List<ImmovableObject> _immovableObjects = new List<ImmovableObject>();
    private readonly List<Tuple<ImmovableObject, BoundingBox>> _immovableObjectBoundingBoxes = new List<Tuple<ImmovableObject, BoundingBox>>();
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
      //Immovable object
      dynamic immpovableObjectsJSO = jso.immovableObjects ?? throw new KeyNotFoundException("Key \"immovableObjects\" was expected in input JSON file!");
      foreach (dynamic immovableObjectJSO in immpovableObjectsJSO) {
        ImmovableObject immovableObject = ImmovableObject.ParseJSO(immovableObjectJSO);
        this._immovableObjects.Add(immovableObject);
        this._immovableObjectBoundingBoxes.Add(
          new Tuple<ImmovableObject, BoundingBox>(immovableObject, immovableObject.Shape.OffsetBoundingBox + immovableObject.Position)
        );
      }
      this._immovableObjectBoundingBoxes.Sort(
        new ObjectBoundingBoxComparer<ImmovableObject>(
          ObjectBoundingBoxComparer<ImmovableObject>.ObjectBoundingBoxAttribute.MinX,
          ObjectBoundingBoxComparer<ImmovableObject>.ObjectBoundingBoxAttribute.MaxX,
          ObjectBoundingBoxComparer<ImmovableObject>.ObjectBoundingBoxAttribute.ObjectID
        )
      );

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
      List<Tuple<Body, BoundingBox>> bodyBoundingBoxes = new List<Tuple<Body, BoundingBox>>();
      for (int i = 0; i < this._bodies.Length; i++) { // For every body
        Body body = this._bodies[i];
        body.CalculateNextPosition(timeInterval);
        bodyBoundingBoxes.Add(new Tuple<Body, BoundingBox>(body, body.BoundingBox));
      }
      bodyBoundingBoxes.Sort(new ObjectBoundingBoxComparer<Body>(
        ObjectBoundingBoxComparer<Body>.ObjectBoundingBoxAttribute.MinX,
        ObjectBoundingBoxComparer<Body>.ObjectBoundingBoxAttribute.MaxX,
        ObjectBoundingBoxComparer<Body>.ObjectBoundingBoxAttribute.ObjectID
      ));
      float lowestCollisionTime = timeInterval;
      Vector2 lowerCollisionNormal = new Vector2();
      Body collidingBody1 = null;
      Body collidingBody2 = null;
      for (int i = 0; i < bodyBoundingBoxes.Count; i++) {
        Tuple<Body, BoundingBox> boundingBox1 = bodyBoundingBoxes[i];
        for (
          int j = i + 1;
          j < bodyBoundingBoxes.Count && bodyBoundingBoxes[j].Item2.MinX <= bodyBoundingBoxes[i].Item2.MaxX;
          j++
        ) {
          bool checkForCollision = false;
          if (bodyBoundingBoxes[i].Item2.MinY < bodyBoundingBoxes[j].Item2.MinY) {
            if (bodyBoundingBoxes[j].Item2.MinY <= bodyBoundingBoxes[i].Item2.MaxY) {
              checkForCollision = true;
            }
          } else if (bodyBoundingBoxes[j].Item2.MinY < bodyBoundingBoxes[i].Item2.MinY) {
            if (bodyBoundingBoxes[i].Item2.MinY <= bodyBoundingBoxes[j].Item2.MaxY) {
              checkForCollision = true; 
            }
          } else {
            checkForCollision = true; 
          }

          if (checkForCollision) {
            Console.WriteLine("Oh wow, it turns out that something might be colliding at t=" + this._time + ". Do not worry. I'll check!:)");

            Body body1 = bodyBoundingBoxes[i].Item1;
            Body body2 = bodyBoundingBoxes[j].Item1;
            Vector2 startDifference = body2.Position - body1.Position;
            Vector2 endDifference = body2.NextPosition - body1.NextPosition;
            bool isColliding = body1.Shape.DoesCollide(body2.Shape, startDifference, endDifference, out float timeFactor, out Vector2 collisionNormal);
            float collisionTime = timeFactor * timeInterval;
            if (isColliding) {
              Console.WriteLine("YIKES! Yup, that's a collision 😬");
              if(collisionTime < lowestCollisionTime) {
                lowestCollisionTime = collisionTime;
                lowerCollisionNormal = collisionNormal;
                collidingBody1 = body1;
                collidingBody2 = body2;
              }
              /*
              Collision(ref body1, ref body2);
              futureState[body1.ID] = (body1.Position, body1.Velocity, body1.Acceleration);
              futureState[body2.ID] = (body2.Position, body2.Velocity, body2.Acceleration);
              */
            } else {
              Console.WriteLine("Phew!! Thankfully, there were no collision :))");
            }
          }
        }
        for (
          int k = 0;
          k < this._immovableObjectBoundingBoxes.Count
              && this._immovableObjectBoundingBoxes[k].Item2.MinX <= bodyBoundingBoxes[i].Item2.MaxX;
          k++
        ) {
          if (this._immovableObjectBoundingBoxes[k].Item2.MaxX < bodyBoundingBoxes[i].Item2.MinX) {
            continue;
          }

          bool checkForCollision = false;
          if (bodyBoundingBoxes[i].Item2.MinY < this._immovableObjectBoundingBoxes[k].Item2.MinY) {
            if (this._immovableObjectBoundingBoxes[k].Item2.MinY <= bodyBoundingBoxes[i].Item2.MaxY) {
              checkForCollision = true;
            }
          } else if (this._immovableObjectBoundingBoxes[k].Item2.MinY < bodyBoundingBoxes[i].Item2.MinY) {
            if (bodyBoundingBoxes[i].Item2.MinY <= this._immovableObjectBoundingBoxes[k].Item2.MaxY) {
              checkForCollision = true; 
            }
          } else {
            checkForCollision = true; 
          }

          if (checkForCollision) {
            Console.WriteLine("Oh wow, it turns out that something might be colliding at t=" + this._time + ". Do not worry. I'll check!:)");

            Body body = bodyBoundingBoxes[i].Item1;
            ImmovableObject immovableObject = this._immovableObjectBoundingBoxes[k].Item1;
            Vector2 startDifference = body.Position - immovableObject.Position;
            Vector2 endDifference = body.NextPosition - immovableObject.Position;
            bool isColliding = immovableObject.Shape.DoesCollide(body.Shape, startDifference, endDifference, out float timeFactor, out Vector2 collisionNormal);
            float collisionTime = timeFactor * timeInterval;
            if (isColliding) {
              Console.WriteLine("YIKES! Yup, that's a collision 😬");
              if(collisionTime < lowestCollisionTime) {
                lowestCollisionTime = collisionTime;
                lowerCollisionNormal = collisionNormal;
                collidingBody1 = body;
                collidingBody2 = null;
              }
              /*
              Collision(ref body1, ref body2);
              futureState[body1.ID] = (body1.Position, body1.Velocity, body1.Acceleration);
              futureState[body2.ID] = (body2.Position, body2.Velocity, body2.Acceleration);
              */
            } else {
              Console.WriteLine("Phew!! Thankfully, there were no collision :))");
            }
          }
        }
        
      }

      if(lowestCollisionTime != timeInterval) {
        Tick(lowestCollisionTime * 0.98f);
        if (collidingBody2 == null) {
          CollisionWithImmovableObject(collidingBody1, lowerCollisionNormal);
        } else {
          Collision(collidingBody1, collidingBody2, lowerCollisionNormal);
        }
      } else {
        foreach (Body body in this._bodies) {
          body.Commit();
        }
      }
    }
    private void Collision(Body body1, Body body2, Vector2 normal) {
      //Console.WriteLine("Ohhh boy. Yup it's a collision :((");
      float m_1 = body1.Mass;
      float m_2 = body2.Mass;
      
      Vector2 nv_1s = body1.Velocity.ToBaseSpace(normal);
      Vector2 nv_2s = body2.Velocity.ToBaseSpace(normal);

      float nv_1fx = (m_1*nv_1s.X - m_2*nv_1s.X + 2*m_2*nv_2s.X)/(m_1+m_2);
      float nv_2fx = (2*m_1*nv_1s.X - m_1*nv_2s.X + m_2*nv_2s.X)/(m_1+m_2);

      Vector2 nv_1f = new Vector2(nv_1fx, nv_1s.Y);
      Vector2 nv_2f = new Vector2(nv_2fx, nv_2s.Y);
      
      body1.Velocity = nv_1f.FromBaseSpace(normal);
      body2.Velocity = nv_2f.FromBaseSpace(normal);
    }
    private void CollisionWithImmovableObject(Body body, Vector2 normal) {
      //Console.WriteLine("Ohhh boy. Yup it's a collision :((");
      body.Velocity = this.Bounce(body.Velocity, normal);
    }
    private Vector2 Bounce(Vector2 original, Vector2 normal) {
      Vector2 normalSpaceOriginal = original.ToBaseSpace(normal);
      Vector2 normalSpaceReflection = new Vector2(-normalSpaceOriginal.X, normalSpaceOriginal.Y);
      return normalSpaceReflection.FromBaseSpace(normal);
    }
  }
}