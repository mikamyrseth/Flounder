using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Flounder
{
  public class BoundingBoxComparer : IComparer<BoundingBox> {
    private enum BoundingBoxAttribute {
      BodyID,
      MaxX,
      MaxY,
      MinX,
      MinY,
    }
    private List<BoundingBoxAttribute> sortOrder;

    BoundingBoxComparer(params BoundingBoxAttribute[] attributes) {
      this.sortOrder = new List<BoundingBoxAttribute>(attributes);
    }

    public int Compare(BoundingBox a, BoundingBox b) {
      return a.MinX.CompareTo(b.MinX);
      foreach (BoundingBoxAttribute attribute in this.sortOrder) {
        switch (attribute) {
          case BoundingBoxAttribute.BodyID:
            break;
          case BoundingBoxAttribute.MaxX:
            break;
          case BoundingBoxAttribute.MaxY:
            break;
          case BoundingBoxAttribute.MinX:
            break;
          case BoundingBoxAttribute.MinY:
            break;
        }
      }
    }

  }
} 