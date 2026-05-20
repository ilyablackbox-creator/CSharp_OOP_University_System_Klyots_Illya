using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_7_Klyots_Illya
{
    public class Fraction
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Знаменник не може дорівнювати нулю.");
            }

            int commonDivisor = GCD(Math.Abs(numerator), Math.Abs(denominator));

            this.Numerator = (denominator < 0) ? -numerator / commonDivisor : numerator / commonDivisor;
            this.Denominator = Math.Abs(denominator) / commonDivisor;
        }

        private static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(
                a.Numerator * b.Denominator + b.Numerator * a.Denominator,
                a.Denominator * b.Denominator
            );
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return new Fraction(
                a.Numerator * b.Denominator - b.Numerator * a.Denominator,
                a.Denominator * b.Denominator
            );
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.Numerator == 0) throw new DivideByZeroException("Ділення на нульовий дріб.");
            return new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }

        public static bool operator ==(Fraction a, Fraction b) => a.Numerator == b.Numerator && a.Denominator == b.Denominator;
        public static bool operator !=(Fraction a, Fraction b) => !(a == b);

        public override string ToString()
        {
            return $"{this.Numerator}/{this.Denominator}";
        }

        public override bool Equals(object obj) => obj is Fraction f && this == f;
        public override int GetHashCode() => HashCode.Combine(this.Numerator, this.Denominator);
    }
}
