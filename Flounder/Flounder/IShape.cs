namespace Flounder
{
    public interface IShape : IIndentedLogger
    {
        public static IShape ParseJSON(dynamic JSON) {
            if (JSON.circle != null) return Circle.ParseJSO(JSON.circle);
            if (JSON.rectangle != null) return Rectangle.ParseJSO(JSON.rectangle);
            return null;
        }
    }
}