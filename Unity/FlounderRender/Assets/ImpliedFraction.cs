using System.Globalization;

namespace Flounder
{

  /// <summary>
  ///   Struct that represent a fraction with implied denominator. Denominator is the same for all ImpliedFractions and is
  ///   set with the static property Precision.
  /// </summary>
  public readonly struct ImpliedFraction
  {

    public enum PrecisionLevel { Micro = 20, Milli = 10, Nano = 30 }

    /// <summary>
    ///   Denominator of all ImpliedFractions. Determines how accurately a number can be approximated.
    /// </summary>
    public static PrecisionLevel Precision { get; set; } = PrecisionLevel.Nano;
    private readonly long _numerator;
    public double DoubleApproximation { get { return this._numerator / (double)(1L << (int)Precision); } }
    public float FloatApproximation { get { return this._numerator / (float)(1L << (int)Precision); } }
    /// <summary>
    ///   Instance of ImpliedFraction
    /// </summary>
    /// <param name="numerator">Numerator</param>
    public ImpliedFraction(long numerator) { this._numerator = numerator; }
    public override string ToString() { return this.DoubleApproximation.ToString(CultureInfo.InvariantCulture); }

  }

}