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
      if(a.MinX == b.MinX && a.MaxX == b.MaxX){
        return a.Body.ID.CompareTo(b.Body.ID);
      }
      else if(a.MinX == b.MinX) {
        return a.MaxX.CompareTo(b.MaxX);
      }
      else return a.MinX.CompareTo(b.MinX);
      
      
      // Greier
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