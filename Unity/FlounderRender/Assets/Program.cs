using System;
using System.Globalization;

using Flounder;

using UnityEngine;

namespace FlounderRender
{

  public class Program : MonoBehaviour
  {

    [Header("Shapes")]
    [SerializeField] private GameObject cubePrefab = null;
    [SerializeField] private GameObject spherePrefab = null;
    [Header("Simulation")]
    [SerializeField] private new Camera camera = null;
    [SerializeField] private string outputFilePath = "";

    private Render _render;

    private void Start() {
      this._render = new Render(this.outputFilePath, this);
      Vector2 center = this._render.Center;
      this.camera.transform.position = new Vector3(center.x, center.y, -10);
      this.camera.orthographicSize = this._render.Radius;
    }

    public Transform CreateShape(string csvLine) {
      if (csvLine.StartsWith("Circle")) {
        Circle circle = Circle.ParseCSV(csvLine);
        Transform newTransform = Instantiate(this.spherePrefab).transform;
        newTransform.localScale = Vector3.one * circle.Radius;
        return newTransform;
      }
      if (csvLine.StartsWith("Rectangle")) {
        Rectangle rectangle = Rectangle.ParseCSV(csvLine);
        Transform newTransform = Instantiate(this.cubePrefab).transform;
        newTransform.localScale = new Vector3(rectangle.SemiSize.x, rectangle.SemiSize.y, 1);
        return newTransform;
      }
      throw new ArgumentException("Could not parse line to shape!");
    }

    public Vector3 ParseVector3FromCSV(string csvLine) {
      string[] parts = csvLine.Split(',');
      for (int i = 0; i < parts.Length; i++) { parts[i] = parts[i].Trim(); }
      switch (parts.Length) {
        case 2:
          return new Vector3(
            float.Parse(parts[0], CultureInfo.InvariantCulture),
            float.Parse(parts[1], CultureInfo.InvariantCulture)
          );
        case 3:
          return new Vector3(
            float.Parse(parts[0], CultureInfo.InvariantCulture),
            float.Parse(parts[1], CultureInfo.InvariantCulture),
            float.Parse(parts[2], CultureInfo.InvariantCulture)
          );
        default:
          throw new FormatException("Could not parse Vector3 from CSV!");
      }
    }

    private void Update() {
      if (Input.GetKey(KeyCode.RightArrow)) {
        this._render.NextFrame();
      }
      if (Input.GetKey(KeyCode.LeftArrow)) {
        this._render.PreviousFrame();
      }
      if (Input.GetKeyDown(KeyCode.Period)) {
        this._render.NextFrame();
      }
      if (Input.GetKeyDown(KeyCode.Comma)) {
        this._render.PreviousFrame();
      }
    }

  }

}