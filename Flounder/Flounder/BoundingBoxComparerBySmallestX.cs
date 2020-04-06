using System.Collections.Generic;
namespace Flounder
{
  public class BoundingBoxComparer : IComparer<BoundingBoxComparer> {
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

  }
}