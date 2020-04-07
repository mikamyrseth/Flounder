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
    private List<BoundingBoxAttribute> sortOrder;

    public BoundingBoxComparer(params BoundingBoxAttribute[] attributes) {
      this.sortOrder = new List<BoundingBoxAttribute>(attributes);
    }

    public int Compare(BoundingBox a, BoundingBox b) {
      foreach (BoundingBoxAttribute attribute in this.sortOrder) {
        switch (attribute) {
          case BoundingBoxAttribute.BodyID:
            if(a.Body.ID != b.Body.ID){
              return a.Body.ID.CompareTo(b.Body.ID);
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
        }
      }
      return 0;
    }

  }
} 