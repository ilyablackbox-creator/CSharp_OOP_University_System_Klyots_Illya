using Praktuchna_7_Klyots_Illya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Praktuchna_7_Klyots_Illya
{
    public enum StudentStatus { Active, AcademicLeave, Expelled, Graduated }
    internal class Student : UniversityMember, IComparable<Student>, ICloneable
    {
        private string recordBookNumber;
        private double averageGrade;
        public decimal Scholarship { get; set; } = 2000;
        private byte[] labGrades = new byte[10];
        private int courseProgress;

        private List<GradeRecord> gradeHistory = new List<GradeRecord>();

        public int CourseProgress
        {
            get => courseProgress;
            set { courseProgress = (value < 0 || value > 100) ? 0 : value; }
        }
        public List<GradePoint> GradesPoints { get; set; } = new List<GradePoint>();
        public DateTime DateOfBirth { get; init; }
        public DateTime EnrollmentDate { get; init; }

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

        // Пpактична7: методи для роботи зі структурами

        public StudentRecord GetRecord()
        {
            return new StudentRecord(this.FullName, this.RecordBookNumber);
        }

        public void AddGradeToHistory(string subject, int score)
        {
            this.gradeHistory.Add(new GradeRecord(subject, score));
        }

        // 3. Отримання всієї історії
        public IEnumerable<GradeRecord> GetGradeHistory()
        {
            return this.gradeHistory;
        }

        // ------------------------------------------------

        public IVehicle PersonalVehicle { get; set; }
        public List<Shape> Shapes { get; set; } = new List<Shape>();
        public StudentStatus Status { get; set; }
        public string Notes { get; set; }
        public List<double> Grades { get; set; } = new List<double>();
        public GradeJournal Journal { get; set; } = new GradeJournal();
        public int? AssignedPortRow { get; set; }
        public int? AssignedPortCol { get; set; }

        public int Age => CalculateAge();

        public Student(string fullName, DateTime birthDate, string email, string recordBook)
            : base(fullName, birthDate, email)
        {
            this.RecordBookNumber = recordBook;
            this.EnrollmentDate = DateTime.Now;
            this.Status = StudentStatus.Active;
        }

        public override decimal CalculateScholarship()
        {
            return GetAverageLabGrade() >= 9 ? Scholarship : 0m;
        }

        public override string GetInfo()
        {
            return $"[Студент] {FullName} | Вік: {CalculateAge()} | Залікова: {RecordBookNumber}";
        }

        public void AddLabGrade(int labNumber, byte grade)
        {
            if (labNumber < 1 || labNumber > 10)
                throw new IndexOutOfRangeException("Номер лабораторної роботи має бути від 1 до 10.");
            if (grade > 10) throw new ArgumentException("Оцінка не може перевищувати 10.");
            labGrades[labNumber - 1] = grade;
        }

        public double GetAverageLabGrade()
        {
            int activeLabs = labGrades.Count(g => g > 0);
            return activeLabs > 0 ? Math.Round(labGrades.Sum(g => (double)g) / activeLabs, 2) : 0;
        }

        public double GetAverageGrade()
        {
            if (this.GradesPoints.Count > 0)
                return this.GradesPoints.Average(g => (double)g);
            return 0;
        }

        // Перевантаження операторів

        public static bool operator >(Student a, Student b)
        {
            if (a.GetAverageLabGrade() != b.GetAverageLabGrade())
                return a.GetAverageLabGrade() > b.GetAverageLabGrade();
            return a.CourseProgress > b.CourseProgress;
        }

        public static bool operator <(Student a, Student b) => !(a > b) && a != b;
        public static bool operator ==(Student a, Student b) => a?.RecordBookNumber == b?.RecordBookNumber;
        public static bool operator !=(Student a, Student b) => !(a == b);

        public static string operator +(Student a, Student b)
        {
            return $"[КОМАНДА]: {a.FullName} та {b.FullName} | Спільний прогрес: {(a.CourseProgress + b.CourseProgress) / 2}%";
        }

        public void ShowDetailedInfo()
        {
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"СТУДЕНТ: {FullName}");
            Console.WriteLine($"Залікова: {RecordBookNumber} | Статус: {Status}");
            Console.WriteLine($"Email: {PersonEmail}"); // Використовуємо PersonEmail з базового класу
            Console.WriteLine($"Сер. бал лаб: {GetAverageLabGrade()}");
            Console.WriteLine(new string('=', 40));
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
            clone.GradesPoints = new List<GradePoint>(this.GradesPoints);
            clone.gradeHistory = new List<GradeRecord>(this.gradeHistory);
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
            return $"[{RecordBookNumber}] {FullName.PadRight(20)} | Лаби: {GetAverageLabGrade():F2}";
        }
    }

    // Похідні класи

    // 1. Відмінник
    internal class ExcellentStudent : Student
    {
        public ExcellentStudent(string fullName, DateTime birthDate, string email, string recordBook)
            : base(fullName, birthDate, email, recordBook) { }

        public override decimal CalculateScholarship() => 3500m; // Підвищена стипендія

        public override string GetInfo() => $"[ВІДМІННИК] {FullName} | Спец. стипендія: {CalculateScholarship()} грн.";
    }

    // 2. Іноземний студент
    internal class ForeignStudent : Student
    {
        public string Country { get; set; } = "Україна";
        public string NativeLanguage { get; set; }

        public ForeignStudent(string fullName, DateTime birthDate, string email, string recordBook)
            : base(fullName, birthDate, email, recordBook) { }

        public override string GetInfo() => $"[ІНОЗЕМЕЦЬ] {FullName} | Країна: {Country} | Мова: {NativeLanguage}";
    }

    // 3. Працюючий студент
    internal class WorkingStudent : Student
    {
        public string CompanyName { get; set; }
        public string Position { get; set; }

        public WorkingStudent(string fullName, DateTime birthDate, string email, string recordBook)
            : base(fullName, birthDate, email, recordBook) { }

        public override string GetInfo() => $"[ПРАЦЮЮЧИЙ] {FullName} | Компанія: {CompanyName} | Посада: {Position}";
    }

    // 4. Випускник 
    sealed internal class GraduateStudent : Student
    {
        public string ThesisTitle { get; set; }
        public double FinalQualificationScore { get; set; }

        public GraduateStudent(string fullName, DateTime birthDate, string email, string recordBook)
            : base(fullName, birthDate, email, recordBook)
        {
            Status = StudentStatus.Graduated;
        }

        public override string GetInfo() => $"[ВИПУСКНИК] {FullName} | Тема диплому: {ThesisTitle}";
    }
}

