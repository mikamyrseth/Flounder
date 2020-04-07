using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Flounder
{
  public class BoundingBoxComparer : IComparer<BoundingBox> {
    public enum BoundingBoxAttribute {
      BodyID,
      MaxX,
      MaxY,
      MinX,
      MinY,
    }
    private List<BoundingBoxAttribute> _sortOrder;

    public BoundingBoxComparer(params BoundingBoxAttribute[] attributes) {
      this._sortOrder = new List<BoundingBoxAttribute>(attributes);
    }

    public int Compare(BoundingBox a, BoundingBox b) {
      foreach (BoundingBoxAttribute attribute in this._sortOrder) {
        switch (attribute) {
          case BoundingBoxAttribute.BodyID:
            if(a.Body.ID != b.Body.ID){
              return string.Compare(a.Body.ID, b.Body.ID, StringComparison.InvariantCulture);
            }
            break;
          case BoundingBoxAttribute.MaxX:
            if(a.MaxX != b.MaxX){
              return a.MaxX.CompareTo(b.MaxX);
            }
            break;
          case BoundingBoxAttribute.MaxY:
            if(a.MaxY != b.MaxY){
              return a.MaxY.CompareTo(b.MaxY);
            }
            break;
          case BoundingBoxAttribute.MinX:
            if(a.MinX != b.MinX){
              return a.MinX.CompareTo(b.MinX);
            }
            break;
          case BoundingBoxAttribute.MinY:
            if (a.MinY != b.MinY){
              return a.MinY.CompareTo(b.MinY);
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