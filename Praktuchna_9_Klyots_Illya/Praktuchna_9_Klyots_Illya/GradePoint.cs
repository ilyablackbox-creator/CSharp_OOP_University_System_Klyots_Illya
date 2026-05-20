using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_9_Klyots_Illya
{
    public class GradePoint
    {
        private double value;

        public double Value
        {
            get => this.value;
            set => this.value = Math.Clamp(value, 0, 10);
        }

        public GradePoint(double value) => this.Value = value;

        public static GradePoint operator +(GradePoint a, GradePoint b) => new GradePoint(a.Value + b.Value);
        public static GradePoint operator ++(GradePoint g) => new GradePoint(g.Value + 1);
        public static GradePoint operator --(GradePoint g) => new GradePoint(g.Value - 1);

        public static bool operator >(GradePoint a, GradePoint b) => a.Value > b.Value;
        public static bool operator <(GradePoint a, GradePoint b) => a.Value < b.Value;
        public static bool operator >=(GradePoint a, GradePoint b) => a.Value >= b.Value;
        public static bool operator <=(GradePoint a, GradePoint b) => a.Value <= b.Value;

        public static bool operator true(GradePoint g) => g.Value >= 8;
        public static bool operator false(GradePoint g) => g.Value < 8;

        public static implicit operator double(GradePoint g) => g?.Value ?? 0;
        public static implicit operator GradePoint(double d) => new GradePoint(d);

        public override string ToString() => this.Value.ToString("F1");
    }
}
