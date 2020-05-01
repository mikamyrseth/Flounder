using System.Collections.Generic;

namespace Flounder{

  public class ImmovableObject : FlounderObject {

    public IShape Shape { get; }
    public Vector2 Position { get; }
    
    public static ImmovableObject ParseJSO(dynamic jso) {
      string id = (string)(jso.id ?? throw new KeyNotFoundException("Key \"id\" was expected as a key in \"immovableObject\" in input JSON file!"));
      IShape shape = IShape.ParseJSO(jso.shape ?? throw new KeyNotFoundException("Key \"shape\" was expected in \"immovableObject\" input JSON file!"));
      Vector2 position = Vector2.ParseJSO(jso.position ?? throw new KeyNotFoundException("Key \"position\" was expected in \"immovableObject\" input JSON file!"));
      return new ImmovableObject(id, shape, position);
    }
    
    public ImmovableObject(string ID, IShape shape, Vector2 position)
      : base(ID)
    {
      this.Shape = shape;
      this.Position = position;
    }

  }

}
