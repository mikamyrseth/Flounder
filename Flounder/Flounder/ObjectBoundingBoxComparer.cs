using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Flounder
{
  public class ObjectBoundingBoxComparer<T> : IComparer<Tuple<T, BoundingBox>>
    where T : FlounderObject
  {
    public enum ObjectBoundingBoxAttribute {
      ObjectID,
      MaxX,
      MaxY,
      MinX,
      MinY,
    }
    private List<ObjectBoundingBoxAttribute> _sortOrder;

    public ObjectBoundingBoxComparer(params ObjectBoundingBoxAttribute[] attributes) {
      this._sortOrder = new List<ObjectBoundingBoxAttribute>(attributes);
    }

    public int Compare(Tuple<T, BoundingBox> a, Tuple<T, BoundingBox> b) {
      foreach (ObjectBoundingBoxAttribute attribute in this._sortOrder) {
        switch (attribute) {
          case ObjectBoundingBoxAttribute.ObjectID:
            if(a.Item1.ID != b.Item1.ID){
              return string.Compare(a.Item1.ID, b.Item1.ID, StringComparison.InvariantCulture);
            }
            break;
          case ObjectBoundingBoxAttribute.MaxX:
            if(a.Item2.MaxX != b.Item2.MaxX){
              return a.Item2.MaxX.CompareTo(b.Item2.MaxX);
            }
            break;
          case ObjectBoundingBoxAttribute.MaxY:
            if(a.Item2.MaxY != b.Item2.MaxY){
              return a.Item2.MaxY.CompareTo(b.Item2.MaxY);
            }
            break;
          case ObjectBoundingBoxAttribute.MinX:
            if(a.Item2.MinX != b.Item2.MinX){
              return a.Item2.MinX.CompareTo(b.Item2.MinX);
            }
            break;
          case ObjectBoundingBoxAttribute.MinY:
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