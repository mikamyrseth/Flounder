using System;
namespace Flounder
{
  public readonly struct BoundingBox 
  {
    public static BoundingBox operator +(Vector2 a, BoundingBox b) {
      return new BoundingBox(this.Position + a, this.Size));
    }
    public static BoundingBox operator +(BoundingBox b, Vector2 a) {
      return new BoundingBox(this.Position + a, this.Size));
    }
    public static BoundingBox operator +(BoundingBox a, BoundingBox b) {
      Vector2 position = new Vector2(MathF.Min(a.MinX, b.MinX), MathF.Min(a.MinY, b.MinY));
      Vector2 originSize = new Vector2(MathF.Max(a.MaxX, b.MaxX), MathF.Max(a.MaxY, b.MaxY));
      return new BoundingBox(position, originSize - position);
    }
    public readonly float MaxX { get { return this.Position.X + this.Size.X; } }
    public readonly float MaxY { get { return this.Position.Y + this.Size.Y; } }
    public readonly float MinX { get { return this.Position.X; } }
    public readonly float MinY { get { return this.Position.Y; } }
    public readonly Vector2 Position { get; }
    public readonly Vector2 Size { get; }

    public BoundingBox(Vector2 position, Vector2 size) {
      this.Position = position;
      this.Size = size;
    }
  }
}