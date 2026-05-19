using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_8_Klyots_Illya
{
    // 1. Point — координати лабораторного місця
    public readonly struct Point : IEquatable<Point>
    {
        public double X { get; }
        public double Y { get; }
        public Point(double x, double y) => (X, Y) = (x, y);

        public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);

        public override string ToString() => $"({X:F1}; {Y:F1})";

        public bool Equals(Point other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is Point other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public static bool operator ==(Point left, Point right) => left.Equals(right);
        public static bool operator !=(Point left, Point right) => !left.Equals(right);
    }

    // 2. GradeRecord — запис про оцінку
    public readonly struct GradeRecord : IEquatable<GradeRecord>
    {
        public string Subject { get; }
        public int Score { get; }
        public GradeRecord(string subject, int score) => (Subject, Score) = (subject, score);

        public void Deconstruct(out string sub, out int sc) => (sub, sc) = (Subject, Score);

        public override string ToString() => $"{Subject}: {Score}";

        public bool Equals(GradeRecord other) => Subject == other.Subject && Score == other.Score;
        public override bool Equals(object obj) => obj is GradeRecord other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Subject, Score);
        public static bool operator ==(GradeRecord l, GradeRecord r) => l.Equals(r);
        public static bool operator !=(GradeRecord l, GradeRecord r) => !l.Equals(r);
    }

    // 3. StudentRecord — копія даних студента
    public readonly struct StudentRecord : IEquatable<StudentRecord>
    {
        public string FullName { get; }
        public string TicketNumber { get; }
        public StudentRecord(string name, string ticket) => (FullName, TicketNumber) = (name, ticket);

        public void Deconstruct(out string n, out string t) => (n, t) = (FullName, TicketNumber);

        public override string ToString() => $"{FullName} (№{TicketNumber})";

        public bool Equals(StudentRecord other) => TicketNumber == other.TicketNumber;
        public override bool Equals(object obj) => obj is StudentRecord other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(TicketNumber);
        public static bool operator ==(StudentRecord l, StudentRecord r) => l.Equals(r);
        public static bool operator !=(StudentRecord l, StudentRecord r) => !l.Equals(r);
    }

    // 4. ComplexNumber — для математичних обчислень
    public readonly struct ComplexNumber : IEquatable<ComplexNumber>
    {
        public double Real { get; }
        public double Imaginary { get; }
        public ComplexNumber(double r, double i) => (Real, Imaginary) = (r, i);

        public void Deconstruct(out double r, out double i) => (r, i) = (Real, Imaginary);

        public override string ToString() => $"{Real} + {Imaginary}i";

        public bool Equals(ComplexNumber other) => Real == other.Real && Imaginary == other.Imaginary;
        public override bool Equals(object obj) => obj is ComplexNumber other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Real, Imaginary);
        public static bool operator ==(ComplexNumber l, ComplexNumber r) => l.Equals(r);
        public static bool operator !=(ComplexNumber l, ComplexNumber r) => !l.Equals(r);
    }

    // Варіант 1: DateRange (Діапазон дат)
    public readonly struct DateRange : IEquatable<DateRange>
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public DateRange(DateTime start, DateTime end)
        {
            if (end < start) throw new ArgumentException("Кінець не може бути раніше початку");
            Start = start;
            End = end;
        }

        public override string ToString() => $"{Start.ToShortDateString()} - {End.ToShortDateString()}";

        public bool Equals(DateRange other) => Start == other.Start && End == other.End;
        public override bool Equals(object obj) => obj is DateRange other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Start, End);
        public static bool operator ==(DateRange l, DateRange r) => l.Equals(r);
        public static bool operator !=(DateRange l, DateRange r) => !l.Equals(r);
    }
}
