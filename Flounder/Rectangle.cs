namespace Flounder
{

  public struct Rectangle : IShape
  {
      private int semiHeight;
      private int semiWidth;

      public Rectangle(int semiHeight, int semiWidth){
          this.semiHeight = semiHeight;
          this.semiWidth = semiWidth;
      }

      public override string ToString(){
          return "cool rectangle";
      }

  }

}