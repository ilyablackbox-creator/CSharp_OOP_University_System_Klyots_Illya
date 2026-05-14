using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_5_Klyots_Illya
{
    internal abstract class Person
    {
        protected string fullName;
        public DateTime DateOfBirth { get; set; }
        protected string personEmail;
        public string Notes { get; set; }

        public string FullName
        {
            get => fullName;
            set
            {
                string[] parts = value.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 3)
                    throw new ArgumentException("Помилка: ПІБ має містити як мінімум Прізвище, Ім'я та По батькові.");
                this.fullName = string.Join(" ", parts);
            }
        }

        public string PersonEmail
        {
            get => personEmail;
            set
            {
                if (!value.Contains("@")) throw new ArgumentException("Невірно написано Email");
                personEmail = value;
            }
        }

        public Person(string fullName, DateTime birthDate, string Email)
        {
            FullName = fullName;
            DateOfBirth = birthDate;
            PersonEmail = Email;
        }

        public int CalculateAge()
        {
            int age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }

        public virtual string GetInfo()
        {
            return $"ПІБ: {FullName} | Вік: {CalculateAge()}";
        }
    }
}
