using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_9_Klyots_Illya
{
    public abstract class UniversityMember : Person
    {
        protected UniversityMember() : base() { }
        protected UniversityMember(string fullName, DateTime birthDate, string email)
            : base(fullName, birthDate, email) { }

        public abstract decimal CalculateScholarship();

        public virtual void Enroll()
        {
            Console.WriteLine($"{FullName} зараховано.");
        }
    }
}
