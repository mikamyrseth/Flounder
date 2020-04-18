using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Flounder
{
  public class BodyBoundingBoxComparer : IComparer<Tuple<Body, BoundingBox>> {
    public enum BodyBoundingBoxAttribute {
      BodyID,
      MaxX,
      MaxY,
      MinX,
      MinY,
    }
    private List<BodyBoundingBoxAttribute> _sortOrder;

    public BodyBoundingBoxComparer(params BodyBoundingBoxAttribute[] attributes) {
      this._sortOrder = new List<BodyBoundingBoxAttribute>(attributes);
    }

    public int Compare(Tuple<Body, BoundingBox> a, Tuple<Body, BoundingBox> b) {
      foreach (BodyBoundingBoxAttribute attribute in this._sortOrder) {
        switch (attribute) {
          case BodyBoundingBoxAttribute.BodyID:
            if(a.Item1.ID != b.Item1.ID){
              return string.Compare(a.Item1.ID, b.Item1.ID, StringComparison.InvariantCulture);
            }
            break;
          case BodyBoundingBoxAttribute.MaxX:
            if(a.Item2.MaxX != b.Item2.MaxX){
              return a.Item2.MaxX.CompareTo(b.Item2.MaxX);
            }
            break;
          case BodyBoundingBoxAttribute.MaxY:
            if(a.Item2.MaxY != b.Item2.MaxY){
              return a.Item2.MaxY.CompareTo(b.Item2.MaxY);
            }
            break;
          case BodyBoundingBoxAttribute.MinX:
            if(a.Item2.MinX != b.Item2.MinX){
              return a.Item2.MinX.CompareTo(b.Item2.MinX);
            }
            break;
          case BodyBoundingBoxAttribute.MinY:
            if (a.Item2.MinY != b.Item2.MinY){
              return a.Item2.MinY.CompareTo(b.Item2.MinY);
            }
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      return 0;
    }

  }
} 