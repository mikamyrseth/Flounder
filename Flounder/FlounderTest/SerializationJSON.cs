using System.Globalization;

using Flounder;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace FlounderTest
{
    public class SerializationJSON
    {
        
        private CultureInfo _culture;
        private readonly ITestOutputHelper _output;

        public SerializationJSON(ITestOutputHelper output)
        {
            this._culture = CultureInfo.InvariantCulture;
            this._output = output;
        }

        private void CircleCase(float radius) {
            string json = new Circle(radius).SerializeJSON(0);
            dynamic jso = JsonConvert.DeserializeObject(json);
            Circle circle = Flounder.Circle.ParseJSO(jso);
            Assert.Equal(radius, circle.Radius);
        }

        private void RectangleCase(float semiHeight, float semiWidth) {
            string json = new Rectangle(new Vector2(semiWidth, semiHeight)).SerializeJSON(0);
            dynamic jso = JsonConvert.DeserializeObject(json);
            Rectangle rectangle = Flounder.Rectangle.ParseJSO(jso);
            Assert.Equal(semiHeight, rectangle.SemiHeight);
            Assert.Equal(semiWidth, rectangle.SemiWidth);
        }
        
        private void Vector2Case(float x, float y) {
            string json = new Vector2(x, y).SerializeJSON(0);
            dynamic jso = JsonConvert.DeserializeObject(json);
            Vector2 vector = Flounder.Vector2.ParseJSO(jso);
            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
        }

        [Fact]
        public void Circle() {
            this.CircleCase(0f);
            this.CircleCase(1f);
            this.CircleCase(1.0f);
            this.CircleCase(3.14f);
            this.CircleCase(1234.5678f);
        }

        [Fact]
        public void Rectangle() {
            this.RectangleCase(0f, 0f);
            this.RectangleCase(1f, 2f);
            this.RectangleCase(4.2f, 2.5f);
            this.RectangleCase(52132.13523f, 123512.6431f);
        }
        
        [Fact]
        public void Vector2() {
            this.Vector2Case(0f, 0f);
            this.Vector2Case(2f, 7f);
            this.Vector2Case(2f, -7f);
            this.Vector2Case(-3f, -5f);
            this.Vector2Case(-3f, 5f);
            this.Vector2Case(-3.14f, 5.53f);
            this.Vector2Case(-3.5f, 5.1235f);
        }
    }
}