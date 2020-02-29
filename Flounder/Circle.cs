using System;

namespace Flounder
{

  struct Circle : IShape
  {
      private int radius;

      public Circle(int radius) {
          this.radius = radius;
      }

      public override string ToString (){
          return "cool circle";
      }

  }

}