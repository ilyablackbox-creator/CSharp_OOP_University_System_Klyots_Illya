using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_9_Klyots_Illya
{
    public delegate void StudentOperation(Student student);
    public delegate void GroupOperation(StudentGroup group);

    public class StudentEventArgs : EventArgs
    {
        public Student Student { get; }
        public string Message { get; }

        public StudentEventArgs(Student student, string message = "")
        {
            Student = student;
            Message = message;
        }
    }

    public class GroupReportEventArgs : EventArgs
    {
        public string ReportContent { get; }
        public DateTime GeneratedAt { get; }

        public GroupReportEventArgs(string reportContent)
        {
            ReportContent = reportContent;
            GeneratedAt = DateTime.Now;
        }
    }
}
