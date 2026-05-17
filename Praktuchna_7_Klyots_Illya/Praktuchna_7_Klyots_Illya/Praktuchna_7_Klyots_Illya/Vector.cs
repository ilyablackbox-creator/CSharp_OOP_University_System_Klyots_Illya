using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_7_Klyots_Illya
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector operator +(Vector a, Vector b) => new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static double operator *(Vector a, Vector b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        public static bool operator ==(Vector a, Vector b) => (double)a == (double)b;
        public static bool operator !=(Vector a, Vector b) => !(a == b);
        public static bool operator >(Vector a, Vector b) => (double)a > (double)b;
        public static bool operator <(Vector a, Vector b) => (double)a < (double)b;

        public static Vector operator ++(Vector v) => new Vector(v.X + 1, v.Y + 1, v.Z + 1);
        public static Vector operator --(Vector v) => new Vector(v.X - 1, v.Y - 1, v.Z - 1);

        public static explicit operator double(Vector v) => Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);

        public override string ToString() => $"Vector({this.X}; {this.Y}; {this.Z})";
    }
}
