namespace Flounder
{

  public interface IShape : IIndentedLogger
  {

      public static IShape ParseJSON(dynamic JSON) {
          if (JSON.circle != null) {
              return Circle.ParseJSON(JSON.circle);
          }
          if (JSON.rectangle != null) {
              return Rectangle.ParseJSON(JSON.rectangle);
          }
          return null;
      }
      
  }

}