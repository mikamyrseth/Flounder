
namespace Flounder
{
  public readonly struct BoundingBox 
  {
    public readonly Body Body { get; }
    public readonly float MaxX { get { return this.Position.X + this.Size.X; } }
    public readonly float MaxY { get { return this.Position.Y + this.Size.Y; } }
    public readonly float MinX { get { return this.Position.X; } }
    public readonly float MinY { get { return this.Position.Y; } }
    public readonly Vector2 Position { get; }
    public readonly Vector2 Size { get; }

    public BoundingBox(Body body, Vector2 position, Vector2 size) {
      this.Body = body;
      this.Position = position;
      this.Size = size;
    }
  }
}