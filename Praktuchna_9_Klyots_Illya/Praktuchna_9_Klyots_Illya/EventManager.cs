using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_9_Klyots_Illya
{
    internal class EventManager
    {
        public static event EventHandler<StudentEventArgs> StudentAdded;
        public static event EventHandler<StudentEventArgs> StudentRemoved;
        public static event EventHandler<GroupReportEventArgs> ReportGenerated;

        public static Action<string> Logger = message =>
        {
            string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}";
            EventHistory.Add(logEntry);
            Console.WriteLine($" Глобальний логер: {logEntry}");
        };

        public static List<string> EventHistory { get; } = new List<string>();

        public static void RaiseStudentAdded(object sender, Student student)
        {
            Logger?.Invoke($"Додано студента: {student.FullName}");
            StudentAdded?.Invoke(sender, new StudentEventArgs(student, "Студент зарахований."));
        }

        public static void RaiseStudentRemoved(object sender, Student student)
        {
            Logger?.Invoke($"Вилучено студента: {student.FullName}");
            StudentRemoved?.Invoke(sender, new StudentEventArgs(student));
        }

        public static void RaiseReportGenerated(object sender, string reportContent)
        {
            Logger?.Invoke("Сгенеровано новий аналітичний звіт групи.");
            ReportGenerated?.Invoke(sender, new GroupReportEventArgs(reportContent));
        }
    }

    public static class TeacherNotifier
    {
        public static void CheckStudentGrade(object sender, StudentEventArgs e)
        {
            if (e.Student.AverageGrade < 6.0 && e.Student.AverageGrade > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n Увага! {e.Student.FullName} має низький бал: {e.Student.AverageGrade:F1}/10!");
                Console.ResetColor();
                EventManager.Logger?.Invoke($"Сповіщення викладачу надіслано для: {e.Student.FullName}");
            }
        }
    }
}
