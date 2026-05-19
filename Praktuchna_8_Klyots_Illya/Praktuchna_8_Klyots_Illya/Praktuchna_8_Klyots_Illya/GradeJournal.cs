using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_8_Klyots_Illya
{
    public class GradeJournal
    {
        private Dictionary<int, byte> grades = new Dictionary<int, byte>();

        public void AddGrade(int labNumber, byte grade)
        {
            grades[labNumber] = grade;
        }

        public Dictionary<int, byte> GetAllGrades()
        {
            return new Dictionary<int, byte>(grades);
        }
    }
}
