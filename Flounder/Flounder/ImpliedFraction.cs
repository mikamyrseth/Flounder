using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Flounder
{

    /// <summary>
    /// Struct that represent a fraction with implied denominator. Denominator is the same for all ImpliedFractions and is set with the static property Precision.
    /// </summary>
    public readonly struct ImpliedFraction
    {

        public enum PrecisionLevel
        {
            Micro = 20,
            Milli = 10,
            Nano = 30,
        }

        private static PrecisionLevel _precision = PrecisionLevel.Nano;

        /// <summary>
        /// Denominator of all ImpliedFractions. Determines how accurately a number can be approximated. 
        /// </summary>
        public static PrecisionLevel Precision {
            get { return _precision; }
            set { _precision = value; }
        }

        public static ImpliedFraction operator +(ImpliedFraction a, ImpliedFraction b) {
            return new ImpliedFraction(a._numerator + b._numerator);
        }

        public static ImpliedFraction operator -(ImpliedFraction a, ImpliedFraction b) {
            return new ImpliedFraction(a._numerator - b._numerator);
        }

        public static ImpliedFraction operator *(ImpliedFraction a, ImpliedFraction b) {
            return new ImpliedFraction((a._numerator >> ((int) _precision / 2)) * (b._numerator >> ((int) _precision / 2)));
        }

        public static ImpliedFraction operator /(ImpliedFraction dividend, ImpliedFraction divisor) {
            return new ImpliedFraction((dividend._numerator << ((int) _precision / 2)) / (divisor._numerator >> ((int) _precision / 2)));
        }

        private readonly long _numerator;

        public double DoubleApproximation {
            get { return this._numerator / (double) (1L << (int) _precision); }
        }

        /// <summary>
        /// Instance of ImpliedFraction
        /// </summary>
        /// <param name="numerator">Numerator</param>
        public ImpliedFraction(long numerator) {
            this._numerator = numerator;
        }

        public ImpliedFraction(string text) {
            if (new Regex(@"\A-?\d+(\.\d+)?\z").IsMatch(text)) {
                throw new NotImplementedException();
                bool minusFlag = false;
                ImpliedFraction integerIF = new ImpliedFraction(0),
                                decimalIF = new ImpliedFraction(0);
                if (text[0] == '-') { minusFlag = true; }
                if (text.Contains('.')) {
                    string decimalText = text.Substring(text.IndexOf('.') + 1);
                    long dividend = long.Parse(decimalText);
                    long divisor = 1;
                }
            } else {
                throw new ArgumentException("Could not parse format of string into ImpliedFraction!");
            }
        }
    
        public override string ToString() {
            return $"ImpliedFraction {{ Numerator: {this._numerator}, Precision: {_precision} }}";
        }
    
    }

}