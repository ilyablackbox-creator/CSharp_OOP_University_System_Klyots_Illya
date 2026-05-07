using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Praktuchna_2_Klyots_Illya
{
    public enum StudentStatus { Active, AcademicLeave, Expelled, Graduated } 
    internal class Student : IComparable<Student>, ICloneable
    {
        private string fullName;
        private string recordBookNumber;
        private double averageGrade;
        private string personalEmail;

        private byte[] labGrades = new byte[10];

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
                if (string.IsNullOrWhiteSpace(value) || value.Length < 5)
                    throw new ArgumentException("ПІБ: не менше 5 символів.");
                fullName = value;
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
        public GradeJournal Journal { get; set; } = new GradeJournal();
        public int? AssignedPortRow { get; set; }
        public int? AssignedPortCol { get; set; }

        public int Age => CalculateAge();

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

        public bool IsExcellent() => AverageGrade >= 90;
        public bool IsFailing() => AverageGrade < 60;

        public object Clone()
        {
            var clone = (Student)this.MemberwiseClone();
            clone.labGrades = (byte[])this.labGrades.Clone(); 
            return clone;
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
