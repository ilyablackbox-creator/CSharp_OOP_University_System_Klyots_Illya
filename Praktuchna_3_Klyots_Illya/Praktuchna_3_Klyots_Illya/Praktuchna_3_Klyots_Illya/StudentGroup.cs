using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Praktuchna_3_Klyots_Illya
{
    internal class StudentGroup
    {

        private List<Student> students = new List<Student>();

        public int GroupSize => students.Count;

        public double AverageGroupGrade => students.Any()
            ? Math.Round(students.Average(s => s.AverageGrade), 2)
            : 0;

        public void AddStudent(Student s)
        {
            if (s == null) return;
            students.Add(s);
        }

        public void RemoveStudent(string recordBookNumber)
        {
            int removedCount = students.RemoveAll(s => s.RecordBookNumber == recordBookNumber);
            if (removedCount > 0)
                Console.WriteLine($"Студента з ID {recordBookNumber} видалено.");
            else
                Console.WriteLine("Студента з таким ID не знайдено.");
        }

        public Student FindStudent(string recordBookNumber)
        {
            return students.FirstOrDefault(s => s.RecordBookNumber == recordBookNumber);
        }

        public List<Student> FindStudent(string namePart, bool isNameSearch)
        {
            return students.Where(s => s.FullName.Contains(namePart, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Student> GetExcellentStudents()
        {
            return students.Where(s => s.IsExcellent()).ToList();
        }

        public List<Student> GetStudentsByStatus(StudentStatus status)
        {
            return students.Where(s => s.Status == status).ToList();
        }

        public List<Student> GetAllStudents() => students;

        public void AssignStudentToPort(Student s, int row, int col, PortMatrix matrix)
        {
            var port = matrix.GetPort(row, col);
            port.DeviceName = $"Робоче місце: {s.FullName}";
            s.AssignedPortRow = row;
            s.AssignedPortCol = col;
        }

        public List<Student> GetStudentsByPortStatus(PortMatrix matrix, bool isOpen)
        {
            return students.Where(s => s.AssignedPortRow.HasValue && matrix.GetPort(s.AssignedPortRow.Value, s.AssignedPortCol.Value).IsOpen == isOpen).ToList();
        }

        public void SaveToFile(string fileName)
        {
            try
            {
                string json = JsonConvert.SerializeObject(students, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(fileName, json);
                Console.WriteLine("Дані успішно збережено у файл.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні: {ex.Message}");
            }
        }

        public void LoadFromFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    string json = File.ReadAllText(fileName);
                    students = JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
                    Console.WriteLine("Дані успішно завантажено з файлу.");
                }
                else
                {
                    Console.WriteLine("Файл не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні: {ex.Message}");
            }
        }

        public string SearchByNameFragment(string fragment)
        {
            var found = this.students.Where(s => s.FullName.Contains(fragment, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!found.Any()) return "Нічого не знайдено.";

            StringBuilder sb = new StringBuilder("Результати пошуку:\n");
            foreach (var s in found) sb.Append(s.GetFormattedInfo());
            return sb.ToString();
        }

        public string ExportToCsv()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FullName,RecordBook,AvgGrade");
            foreach (var s in this.students)
                sb.AppendLine($"{s.FullName},{s.RecordBookNumber},{s.GetAverageGrade():F2}");
            return sb.ToString();
        }

        public void ImportStudentsFromText(string rawText)
        {
            string[] lines = rawText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                string[] p = line.Split(';');
                if (p.Length >= 2)
                {
                    try
                    {
                        AddStudent(new Student { 
                            FullName = p[0].Trim(), 
                            RecordBookNumber = p[1].Trim(),
                            DateOfBirth = DateTime.MinValue,
                            EnrollmentDate = DateTime.Now,
                            PersonalEmail = "n/a",
                            Notes = p.Length > 2 ? p[2].Trim() : "" });
                    }
                    catch (Exception e) { Console.WriteLine($"Помилка імпорту рядка: {e.Message}"); }
                }
            }
        }

        public string GetGroupAnalytics()
        {
            if (students.Count == 0) return "Група порожня.";

            int totalLetters = 0;
            foreach (var student in students)
            {
                totalLetters += student.FullName.Replace(" ", "").Length;
            }

            return $"Аналітика групи: Студентів - {students.Count}, Всього літер у ПІБ - {totalLetters}";
        }
    }
}
