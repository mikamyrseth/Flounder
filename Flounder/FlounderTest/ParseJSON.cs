using Flounder;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace FlounderTest
{
    public class ParseJSON
    {
        
        private readonly ITestOutputHelper _output;

        public ParseJSON(ITestOutputHelper output)
        {
            this._output = output;
        }
        
        private void Vector2Case(float x, float y) {
            string json = $"{{ \"x\": {x}, \"y\": {y} }}";
            dynamic jso = JsonConvert.DeserializeObject(json);
            Vector2 vector = new Vector2((float) jso.x, (float)jso.y);
            Assert.Equal(x, vector.X);
            Assert.Equal(y, vector.Y);
        }
        
        [Fact]
        public void Vector2() {
            this.Vector2Case(0f, 0f);
            this.Vector2Case(2f, 7f);
            this.Vector2Case(2f, -7f);
            this.Vector2Case(-3f, -5f);
            this.Vector2Case(-3f, 5f);
        }
    }
}