using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_7_Klyots_Illya
{
    public interface IResizable
    {
        void Resize(double factor);
    }

    public interface IDrawable
    {
        void Draw();
    }

    public interface IPrintable
    {
        string GetPrintInfo();
    }

    public abstract class Shape : IResizable, IDrawable, IPrintable
    {
        public string Name { get; set; }
        public string Color { get; set; }

        protected Shape(string name, string color)
        {
            Name = name;
            Color = color;
        }

        public virtual double CalculateArea() => 0;
        public virtual double CalculatePerimeter() => 0;
        public abstract string GetDescription();
        public abstract void Resize(double factor);
        public abstract void Draw();

        public virtual string GetPrintInfo()
        {
            return $"Фігура: {Name}, Колір: {Color}, Площа: {CalculateArea():F2}, Периметр: {CalculatePerimeter():F2}";
        }
    }

    // --- Похідні класи ---

    // Коло
    public class Circle : Shape
    {
        public double Radius { get; set; }

        public Circle(string color, double radius) : base("Коло", color)
        {
            Radius = radius;
        }

        public override double CalculateArea() => Math.PI * Math.Pow(Radius, 2);
        public override double CalculatePerimeter() => 2 * Math.PI * Radius;
        public override string GetDescription() => $"Коло радіусом {Radius}";

        public override void Resize(double factor) => Radius *= factor;
        public override void Draw() => Console.WriteLine($" Малюємо {Color} коло з радіусом {Radius}");
    }

    // Прямокутник
    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(string color, double width, double height) : base("Прямокутник", color)
        {
            Width = width;
            Height = height;
        }

        protected Rectangle(string name, string color, double width, double height) : base(name, color)
        {
            Width = width;
            Height = height;
        }

        public override double CalculateArea() => Width * Height;
        public override double CalculatePerimeter() => 2 * (Width + Height);
        public override string GetDescription() => $"Прямокутник {Width}x{Height}";

        public override void Resize(double factor)
        {
            Width *= factor;
            Height *= factor;
        }

        public override void Draw() => Console.WriteLine($" Малюємо {Color} прямокутник {Width}x{Height}");
    }

    // Квадрат (успадковується від Rectangle)
    public class Square : Rectangle
    {
        public Square(string color, double side) : base("Квадрат", color, side, side) { }

        public override string GetDescription() => $"Квадрат зі стороною {Width}";

        public override void Draw() => Console.WriteLine($" Малюємо {Color} квадрат зі стороною {Width}");
    }

    // Трикутник
    public class Triangle : Shape
    {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }

        public Triangle(string color, double a, double b, double c) : base("Трикутник", color)
        {
            SideA = a; SideB = b; SideC = c;
        }

        public override double CalculateArea()
        {
            double p = CalculatePerimeter() / 2;
            return Math.Sqrt(p * (p - SideA) * (p - SideB) * (p - SideC));
        }

        public override double CalculatePerimeter() => SideA + SideB + SideC;
        public override string GetDescription() => $"Трикутник зі сторонами {SideA}, {SideB}, {SideC}";

        public override void Resize(double factor)
        {
            SideA *= factor; SideB *= factor; SideC *= factor;
        }

        public override void Draw() => Console.WriteLine($" Малюємо {Color} трикутник");
    }
}
