using System;
using System.Collections.Generic;
using System.Globalization;

using Flounder;

using TMPro;

using UnityEngine;

namespace FlounderRender
{

  public class Program : MonoBehaviour
  {

    private enum PlayingState
    {

      Finished,
      NoRender,
      Paused,
      Playing,

    }

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
    [SerializeField] private SVGImage replayButton = null;

    private PlayingState _playingState;
    private Render _render;
    private GameObject _renderGameObject;
    private float _time;

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
      this._playingState = PlayingState.NoRender;
      this._render = null;
      if (this._renderGameObject != null) {
        Destroy(this._renderGameObject);
      }
      this._time = 0;
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
      this._playingState = PlayingState.Paused;
      Vector2 center = this._render.Center;
      this.camera.transform.position = new Vector3(center.x, center.y, -10);
      this.camera.orthographicSize = this._render.Radius * 1.1f;
      this.UpdateVisuals();
    }
    public void NextFrame() {
      if (this._playingState != PlayingState.Paused) { return; }
      this._time = this._render.NextFrame();
      if (this._render.CurrentFrame == this._render.MaxFrame) {
        this._playingState = PlayingState.Finished;
      }
      this.UpdateVisuals();
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
      switch (this._playingState) {
        case PlayingState.NoRender:
        case PlayingState.Playing:
          return;
        case PlayingState.Finished:
          this._playingState = PlayingState.Paused;
          break;
        case PlayingState.Paused:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this._time = this._render.PreviousFrame();
      this.UpdateVisuals();
    }
    public void TogglePlaying() {
      switch (this._playingState) {
        case PlayingState.Finished:
          this._playingState = PlayingState.Playing;
          this._time = 0;
          this.UpdatePlaybackRow();
          break;
        case PlayingState.NoRender:
          break;
        case PlayingState.Paused:
          this._playingState = PlayingState.Playing;
          this.UpdatePlaybackRow();
          break;
        case PlayingState.Playing:
          this._playingState = PlayingState.Paused;
          this.UpdatePlaybackRow();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    private void Update() {
      if (this._render == null) { return; }
      if (Input.GetKeyDown(KeyCode.Period)) {
        this.NextFrame();
      }
      if (Input.GetKeyDown(KeyCode.Comma)) {
        this.PreviousFrame();
      }
      if (Input.GetKeyDown(KeyCode.K)) {
        this.TogglePlaying();
      }
      if (this._playingState == PlayingState.Playing) {
        this._time += Time.deltaTime;
        if (this._render.MaxTime <= this._time) {
          this._time = this._render.MaxTime;
          this._playingState = PlayingState.Finished;
        }
        this.UpdateVisuals();
      }
    }
    private void UpdateFrameNumber() {
      switch (this._playingState) {
        case PlayingState.Finished:
        case PlayingState.Paused:
        case PlayingState.Playing:
          this.frameNumberText.text = $"{this._render.CurrentFrame} / {this._render.MaxFrame}";
          break;
        case PlayingState.NoRender:
          this.frameNumberText.text = "";
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    private void UpdatePlaybackRow() {
      switch (this._playingState) {
        case PlayingState.Finished:
          this.nextFrameButton.gameObject.SetActive(false);
          this.pauseButton.gameObject.SetActive(false);
          this.playButton.gameObject.SetActive(false);
          this.previousFrameButton.gameObject.SetActive(true);
          this.replayButton.gameObject.SetActive(true);
          break;
        case PlayingState.Paused:
          this.nextFrameButton.gameObject.SetActive(true);
          this.pauseButton.gameObject.SetActive(false);
          this.playButton.gameObject.SetActive(true);
          this.previousFrameButton.gameObject.SetActive(true);
          this.replayButton.gameObject.SetActive(false);
          break;
        case PlayingState.Playing:
          this.nextFrameButton.gameObject.SetActive(false);
          this.pauseButton.gameObject.SetActive(true);
          this.playButton.gameObject.SetActive(false);
          this.previousFrameButton.gameObject.SetActive(false);
          this.replayButton.gameObject.SetActive(false);
          break;
        case PlayingState.NoRender:
          this.nextFrameButton.gameObject.SetActive(false);
          this.pauseButton.gameObject.SetActive(false);
          this.playButton.gameObject.SetActive(false);
          this.previousFrameButton.gameObject.SetActive(false);
          this.replayButton.gameObject.SetActive(false);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    private void UpdateRender() {
      this._render.ShowTime(this._time);
    }
    private void UpdateVisuals() {
      this.UpdateFrameNumber();
      this.UpdatePlaybackRow();
      this.UpdateRender();
    }

  }

}