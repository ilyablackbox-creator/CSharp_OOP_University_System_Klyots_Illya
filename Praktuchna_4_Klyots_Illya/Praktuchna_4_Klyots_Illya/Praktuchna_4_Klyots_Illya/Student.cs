using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Praktuchna_4_Klyots_Illya
{
    public enum StudentStatus { Active, AcademicLeave, Expelled, Graduated } 
    internal class Student : IComparable<Student>, ICloneable
    {
        private string fullName;
        private string recordBookNumber;
        private double averageGrade;
        private string personalEmail;
        private byte[] labGrades = new byte[10];

        private int courseProgress;
        public int CourseProgress
        {
            get => courseProgress;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Прогрес має бути від 0 до 100%");
                courseProgress = value;
            }
        }
        public List<GradePoint> GradesPoints { get; set; } = new List<GradePoint>();
        public required DateTime DateOfBirth { get; init; }
        public required DateTime EnrollmentDate { get; init; }

        public required string PersonalEmail
        {
            get => personalEmail;
            init
            {
                if (!value.Contains("@")) throw new ArgumentException("Невірно написано Email");
                personalEmail = value;
            }
        }

        public string FullName
        {
            get => fullName;
            set
            {
                string[] parts = value.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 3) 
                {
                    throw new ArgumentException("Помилка: ПІБ має містити як мінімум Прізвище та Ім'я.");
                }

                this.fullName = string.Join(" ", parts);
            }
        }

        public string RecordBookNumber
        {
            get => recordBookNumber;
            set
            {
                if (!Regex.IsMatch(value, @"^\d{8}$"))
                    throw new ArgumentException("Номер залікової: 8 цифр");
                recordBookNumber = value;
            }
        }

        public double AverageGrade
        {
            get => averageGrade;
            private set => averageGrade = Math.Round(value, 2);
        }

        public StudentStatus Status { get; set; }
        public string Notes { get; set; }
        public List<double> Grades { get; set; } = new List<double>();
        public GradeJournal Journal { get; set; } = new GradeJournal();
        public int? AssignedPortRow { get; set; }
        public int? AssignedPortCol { get; set; }

        public int Age => CalculateAge();

        public double GetAverageGrade()
        {
            if (this.GradesPoints.Count > 0)
                return this.GradesPoints.Average(g => (double)g);
            return 0;
        }

        // Перевантаження операторів

        public static bool operator >(Student a, Student b)
        {
            if (a.GetAverageGrade() != b.GetAverageGrade())
                return a.GetAverageGrade() > b.GetAverageGrade();
            return a.CourseProgress > b.CourseProgress;
        }

        public static bool operator <(Student a, Student b)
        {
            if (a.GetAverageGrade() != b.GetAverageGrade())
                return a.GetAverageGrade() < b.GetAverageGrade();
            return a.CourseProgress < b.CourseProgress;
        }

        public static bool operator >=(Student a, Student b)
        {
            if (a.GetAverageGrade() != b.GetAverageGrade())
                return a.GetAverageGrade() >= b.GetAverageGrade();
            return a.CourseProgress >= b.CourseProgress;
        }

        public static bool operator <=(Student a, Student b)
        {
            if (a.GetAverageGrade() != b.GetAverageGrade())
                return a.GetAverageGrade() <= b.GetAverageGrade();
            return a.CourseProgress <= b.CourseProgress;
        }

        public static bool operator ==(Student a, Student b) => a?.RecordBookNumber == b?.RecordBookNumber;
        public static bool operator !=(Student a, Student b) => !(a == b);

        // Оператор + для командного профілю
        public static string operator +(Student a, Student b)
        {
            return $"[КОМАНДА]: {a.FullName} та {b.FullName} | Спільний прогрес: {(a.CourseProgress + b.CourseProgress) / 2}%";
        }

        public void AddLabGrade(int labNumber, byte grade)
        {
            if (labNumber < 1 || labNumber > 10)
                throw new IndexOutOfRangeException("Номер лабораторної роботи має бути від 1 до 10.");

            if (grade > 100) throw new ArgumentException("Оцінка не може перевищувати 100.");

            labGrades[labNumber - 1] = grade;
        }

        public double GetAverageLabGrade()
        {
            int activeLabs = labGrades.Count(g => g > 0);
            return activeLabs > 0 ? Math.Round(labGrades.Sum(g => (double)g) / activeLabs, 2) : 0;
        }

        public void ShowDetailedInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\n" + new string('=', 40));
            sb.AppendLine($"СТУДЕНТ: {FullName}");
            sb.AppendLine($"Залікова книжка: {RecordBookNumber}");
            sb.AppendLine($"Вік: {Age} | Статус: {Status}");
            sb.AppendLine($"Email: {PersonalEmail}");
            sb.AppendLine(new string('-', 20));
            sb.Append("Оцінки за лаби: ");

            for (int i = 0; i < labGrades.Length; i++)
            {
                sb.Append(labGrades[i] == 0 ? "[_] " : $"[{labGrades[i]}] ");
            }

            sb.AppendLine($"\nСер. бал лаб: {GetAverageLabGrade()}");
            sb.AppendLine(new string('=', 40));

            Console.WriteLine(sb.ToString());
        }

        public int CalculateAge()
        {
            int age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }

        public void UpdateAverageGrade(double newGrade)
        {
            if (newGrade < 0 || newGrade > 100) throw new ArgumentException("Бал від 0 до 100.");
            this.AverageGrade = newGrade;
        }

        public bool IsExcellent() => AverageGrade >= 9;
        public bool IsFailing() => AverageGrade < 6;

        public object Clone()
        {
            var clone = (Student)this.MemberwiseClone();
            clone.labGrades = (byte[])this.labGrades.Clone(); 
            return clone;
        }

        public string GetFormattedInfo(bool detailed = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"[Студент]: {FullName}");
            sb.AppendLine($"[Квиток]: {RecordBookNumber}");
            if (detailed)
            {
                sb.AppendLine($"[Середній бал]: {GetAverageGrade():F2}");
                sb.AppendLine($"[Нотатки]: {(string.IsNullOrEmpty(Notes) ? "немає" : Notes)}");
            }
            return sb.ToString();
        }

        public bool ContainsKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return false;
            return FullName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                   (Notes != null && Notes.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }


        public int CompareTo(Student other) => string.Compare(FullName, other?.FullName);

        public override string ToString() 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"[{RecordBookNumber}] {FullName.PadRight(20)} | Лаби: {GetAverageLabGrade():F2}");
            return sb.ToString();
        }
    }
}
