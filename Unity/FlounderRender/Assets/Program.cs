using System;
using System.Globalization;

using Flounder;

using TMPro;

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
    [Header("User interface")]
    [SerializeField] private TMP_Text errorMessage = null;
    [SerializeField] private TMP_Text frameNumberText = null;
    [SerializeField] private TMP_InputField inputField = null;
    [SerializeField] private SVGImage nextFrameButton = null;
    [SerializeField] private SVGImage pauseButton = null;
    [SerializeField] private SVGImage playButton = null;
    [SerializeField] private SVGImage previousFrameButton = null;

    private Render _render;
    private GameObject _renderGameObject;

    public Transform CreateShape(string csvLine) {
      Transform newTransform;
      if (csvLine.StartsWith("Circle")) {
        Circle circle = Circle.ParseCSV(csvLine);
        newTransform = Instantiate(this.spherePrefab).transform;
        newTransform.localScale = Vector3.one * circle.Radius;
      } else if (csvLine.StartsWith("Rectangle")) {
        Rectangle rectangle = Rectangle.ParseCSV(csvLine);
        newTransform = Instantiate(this.cubePrefab).transform;
        newTransform.localScale = new Vector3(rectangle.SemiSize.x, rectangle.SemiSize.y, 1);
      } else {
        throw new ArgumentException("Could not parse line to shape!");
      }
      newTransform.parent = this._renderGameObject.transform;
      return newTransform;
    }
    public void LoadRender() {
      Destroy(this._renderGameObject);
      this._render = null;
      this.errorMessage.text = "";
      this.nextFrameButton.gameObject.SetActive(false);
      this.previousFrameButton.gameObject.SetActive(false);

      try {
        this._renderGameObject = new GameObject();
        this._render = new Render(this.inputField.text, this);
      } catch (Exception exception) {
        this.errorMessage.text = exception.Message;
        throw;
      }
      
      if (this._render == null) { return; }
      Vector2 center = this._render.Center;
      this.camera.transform.position = new Vector3(center.x, center.y, -10);
      this.camera.orthographicSize = this._render.Radius * 1.1f;
      this.UpdateFrameNumber();
      this.nextFrameButton.gameObject.SetActive(true);
      this.previousFrameButton.gameObject.SetActive(true);
    }
    public void NextFrame() {
      if (this._render == null) { return; }
      this._render.NextFrame();
      this.UpdateFrameNumber();
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
          Debug.Log(csvLine);
          return new Vector3(
            float.Parse(parts[0], CultureInfo.InvariantCulture),
            float.Parse(parts[1], CultureInfo.InvariantCulture),
            float.Parse(parts[2], CultureInfo.InvariantCulture)
          );
        default:
          throw new FormatException("Could not parse Vector3 from CSV!");
      }
    }
    public void PreviousFrame() {
      if (this._render == null) { return; }
      this._render.PreviousFrame();
      this.UpdateFrameNumber();
    }
    private void Update() {
      if (this._render == null) { return; }
      if (Input.GetKey(KeyCode.RightArrow)) {
        this.NextFrame();
      }
      if (Input.GetKey(KeyCode.LeftArrow)) {
        this.PreviousFrame();
      }
      if (Input.GetKeyDown(KeyCode.Period)) {
        this._render.NextFrame();
        this.UpdateFrameNumber();
      }
      if (Input.GetKeyDown(KeyCode.Comma)) {
        this._render.PreviousFrame();
        this.UpdateFrameNumber();
      }
    }
    private void UpdateFrameNumber() {
      if (this._render == null) { return; }
      this.frameNumberText.text = $"{this._render.CurrentFrame} / {this._render.MaxFrame}";
    }

  }

}