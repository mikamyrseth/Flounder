using Flounder;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace FlounderTest
{
    public class SerializationJSON
    {
        
        private readonly ITestOutputHelper _output;

        public SerializationJSON(ITestOutputHelper output)
        {
            this._output = output;
        }

        private void CircleCase(float radius) {
            this.CircleDeserializeCase(new Circle(radius).SerializeJSON(), radius);
            this.CircleDeserializeCase(new Circle(radius).SerializeJSON(singleLine: true), radius);
        }

        private void CircleDeserializeCase(string json, float radius) {
            dynamic jso = JsonConvert.DeserializeObject(json);
            Circle circle = Flounder.Circle.ParseJSO(jso);
            Assert.Equal(radius, circle.Radius);
        }

        private void ConstantForceCase(string id, Vector2 force) {
            string json = new ConstantForce(id, force).SerializeJSON(0);
            dynamic jso = JsonConvert.DeserializeObject(json);
            ConstantForce constantForce = Flounder.ConstantForce.ParseJSO(jso);
            Assert.Equal(id, constantForce.ID);
            Assert.Equal(force, constantForce.Force);
        }

        private void RectangleCase(Vector2 semiSize) {
            this.RectangleDeserializeCase(new Rectangle(semiSize).SerializeJSON(), semiSize);
            this.RectangleDeserializeCase(new Rectangle(semiSize).SerializeJSON(singleLine: true), semiSize);
        }
        
        private void RectangleCase(float semiHeight, float semiWidth) {
            this.RectangleCase(new Vector2(semiHeight, semiWidth));
        }

        private void RectangleDeserializeCase(string json, Vector2 semiSize) {
            dynamic jso = JsonConvert.DeserializeObject(json);
            Rectangle rectangle = Flounder.Rectangle.ParseJSO(jso);
            Assert.Equal(semiSize, rectangle.SemiSize);
        }
        
        private void ShapeCircleCase(float radius) {
            IShape shape = new Circle(radius);
            this.ShapeCircleDeserializationCase(shape.SerializeJSON(), radius);
            this.ShapeCircleDeserializationCase(shape.SerializeJSON(singleLine: true), radius);
        }

        private void ShapeCircleDeserializationCase(string json, float radius) {
            dynamic jso = JsonConvert.DeserializeObject(json);
            Circle circle = (Circle)IShape.ParseJSO(jso);
            Assert.Equal(radius, circle.Radius);
        }

        private void ShapeRectangleCase(Vector2 semiSize) {
            IShape shape = new Rectangle(semiSize);
            this.ShapeRectangleDeserializationCase(shape.SerializeJSON(), semiSize);
            this.ShapeRectangleDeserializationCase(shape.SerializeJSON(singleLine: true), semiSize);
        }

        private void ShapeRectangleCase(float semiHeight, float semiWidth) {
            this.ShapeRectangleCase(new Vector2(semiWidth, semiHeight));
        }

        private void ShapeRectangleDeserializationCase(string json, Vector2 semiSize) {
            dynamic jso = JsonConvert.DeserializeObject(json);
            Rectangle rectangle = (Rectangle)IShape.ParseJSO(jso);
            Assert.Equal(semiSize, rectangle.SemiSize);
        }
        
        private void Vector2Case(float x, float y) {
            this.Vector2DeserializationCase(new Vector2(x, y).SerializeJSON(), x, y);
            this.Vector2DeserializationCase(new Vector2(x, y).SerializeJSON(singleLine: true), x, y);
        }

        private void Vector2DeserializationCase(string json, float x, float y) {
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
        public void ConstantForce() {
            this.ConstantForceCase("gravity", new Vector2(-9.81f, 0f));
            this.ConstantForceCase("gravity", new Vector2(9.81f, 0f));
            this.ConstantForceCase("gravity", new Vector2(0f, -9.81f));
            this.ConstantForceCase("gravity", new Vector2(0f, 9.81f));
            this.ConstantForceCase("force", new Vector2(2f, -9.81f));
            this.ConstantForceCase("force", new Vector2(2f, 9.81f));
        }

        [Fact]
        public void Rectangle() {
            this.RectangleCase(0f, 0f);
            this.RectangleCase(1f, 2f);
            this.RectangleCase(4.2f, 2.5f);
            this.RectangleCase(52132.13523f, 123512.6431f);
        }

        [Fact]
        public void Shape() {
            this.ShapeCircleCase(1f);
            this.ShapeCircleCase(0.5f);
            this.ShapeCircleCase(1235.1235f);
            this.ShapeRectangleCase(2f, 3f);
            this.ShapeRectangleCase(0.1f, 0.2f);
            this.ShapeRectangleCase(6812.648f, 1236.6593f);
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