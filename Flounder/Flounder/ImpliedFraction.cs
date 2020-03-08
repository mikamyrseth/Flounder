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

    public static ImpliedFraction operator +(ImpliedFraction a, ImpliedFraction b) { return new ImpliedFraction(a._numerator + b._numerator); }

    public static ImpliedFraction operator -(ImpliedFraction a, ImpliedFraction b) { return new ImpliedFraction(a._numerator - b._numerator); }

    public static ImpliedFraction operator *(ImpliedFraction a, ImpliedFraction b) { return new ImpliedFraction((a._numerator * b._numerator) >> (int)Precision); }

    private readonly long _numerator;

    public double DoubleApproximation { get { return this._numerator / (double)(1L << (int)Precision); } }

    /// <summary>
    ///   Instance of ImpliedFraction
    /// </summary>
    /// <param name="numerator">Numerator</param>
    public ImpliedFraction(long numerator) { this._numerator = numerator; }

    public override string ToString() { return $"ImpliedFraction {{ Value: {this._numerator}, Precision: {Precision} }}"; }

  }

}