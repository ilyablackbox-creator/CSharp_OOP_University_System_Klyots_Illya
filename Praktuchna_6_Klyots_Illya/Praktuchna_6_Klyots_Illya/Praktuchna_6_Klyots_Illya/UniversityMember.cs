using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_6_Klyots_Illya
{
    internal abstract class UniversityMember : Person
    {
        protected UniversityMember(string fullName, DateTime birthDate, string email)
            : base(fullName, birthDate, email) { }

        public abstract decimal CalculateScholarship();

        public virtual void Enroll()
        {
            Console.WriteLine($"{FullName} зараховано.");
        }
    }
}
