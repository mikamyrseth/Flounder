using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace FlounderRender
{

  public class RenderObject
  {

    private readonly Transform _transform;
    private readonly GameObject _traceObject;
    private readonly LineRenderer _traceRenderer;
    private readonly List<Vector3> _positions;

    public RenderObject(GameObject shapeObject, GameObject traceObject) {
      this._transform = shapeObject.transform ? shapeObject.transform : throw new ArgumentNullException();
      this._traceObject = traceObject ? traceObject : throw new ArgumentNullException();
      this._traceRenderer = traceObject.GetComponent<LineRenderer>() ?? throw new ArgumentException("Trace object must have");
      this._positions = new List<Vector3>();
    }

    public void AddPositions(IEnumerable<Vector3> positions) {
      this._positions.AddRange(positions);
      this._traceRenderer.positionCount = this._positions.Count;
      this._traceRenderer.SetPositions(this._positions.ToArray());
    }

    public void SetTraceVisibility(bool isTraceVisible) {
      this._traceObject.SetActive(isTraceVisible);
    }

    public void SetTraceWidth(float traceWidth) {
      this._traceRenderer.startWidth = traceWidth;
    }

    public void ShowFrame(int frame) {
      this._transform.position = this._positions[frame];
    }

  }

}