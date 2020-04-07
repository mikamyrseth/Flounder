using System;
using System.Globalization;
using System.Text.RegularExpressions;
namespace Flounder
{
  /// <summary>
  ///   Struct that represent a fraction with implied denominator. Denominator is the same for all ImpliedFractions and is
  ///   set with the static property Precision.
  /// </summary>
  public readonly struct ImpliedFraction
  {
    public enum PrecisionLevel
    {
      Micro = 20,
      Milli = 10,
      Nano = 30
    }
    /// <summary>
    ///   Denominator of all ImpliedFractions. Determines how accurately a number can be approximated.
    /// </summary>
    public static PrecisionLevel Precision { get; set; } = PrecisionLevel.Nano;
    public static ImpliedFraction Zero {
      get { return new ImpliedFraction(0); }
    }
    public static ImpliedFraction operator +(ImpliedFraction a, ImpliedFraction b) {
      return new ImpliedFraction(a._numerator + b._numerator);
    }
    public static ImpliedFraction operator -(ImpliedFraction a, ImpliedFraction b) {
      return new ImpliedFraction(a._numerator - b._numerator);
    }
    public static ImpliedFraction operator *(ImpliedFraction a, ImpliedFraction b) {
      return new ImpliedFraction((a._numerator * b._numerator) >> (int)Precision);
    }
    public static ImpliedFraction operator *(int a, ImpliedFraction b) {
      return new ImpliedFraction(a * b._numerator);
    }
    public static ImpliedFraction operator /(ImpliedFraction a, ImpliedFraction b) {
      return new ImpliedFraction((a._numerator << (int)Precision) / b._numerator);
    }
    public static bool operator <(ImpliedFraction a, ImpliedFraction b) {
      return a._numerator < b._numerator;
    }
    public static bool operator >(ImpliedFraction a, ImpliedFraction b) {
      return a._numerator > b._numerator;
    }
    private readonly long _numerator;
    public double DoubleApproximation {
      get { return this._numerator / (double)(1L << (int)Precision); }
    }
    public float FloatApproximation {
      get { return this._numerator / (float)(1L << (int)Precision); }
    }
    /// <summary>
    ///   Instance of ImpliedFraction
    /// </summary>
    /// <param name="numerator">Numerator</param>
    public ImpliedFraction(long numerator) {
      this._numerator = numerator;
    }
    public static ImpliedFraction Parse(string text) {
      if (!TryParse(text, out ImpliedFraction result)) {
        throw new FormatException("Text could not be parsed into implied fraction!");
      }
      return result;
    }
    public string SerializeJSON() {
      return $"{this._numerator}";
    }
    public override string ToString() {
      return this.DoubleApproximation.ToString(CultureInfo.InvariantCulture);
    }
    public static bool TryParse(string text, out ImpliedFraction result) {
      if (!new Regex(@"\A-?\d+(\.\d+)?\z").IsMatch(text)) {
        result = new ImpliedFraction(0);
        return false;
      }
      string signLessText;
      bool isNegative;
      if (text[0] ==  '-') {
        signLessText = text.Substring(1);
        isNegative = true;
      } else {
        signLessText = text;
        isNegative = false;
      }
      long sum = 0;
      string integerText;
      int periodIndex = signLessText.IndexOf('.');
      if (periodIndex == -1) {
        integerText = signLessText;
      } else {
        integerText = signLessText.Substring(0, periodIndex);
        for (long i = periodIndex + 1, factor = 1, power = (int)Precision; i < signLessText.Length; i++) {
          sum += (long.Parse("" + signLessText[(int)i]) << (int)--power) / (factor *= 5);
        }
      }
      sum += long.Parse(integerText) << (int)Precision;
      result = new ImpliedFraction(isNegative ? -sum : sum);
      return true;
    }
  }
}