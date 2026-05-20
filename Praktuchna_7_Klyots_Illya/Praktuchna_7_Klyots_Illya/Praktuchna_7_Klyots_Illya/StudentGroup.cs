using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Praktuchna_7_Klyots_Illya
{
    internal class StudentGroup
    {
        private List<UniversityMember> members = new List<UniversityMember>();

        private Point[] labSeats;
        private GradeRecord[] groupGradeHistory;
        public int GroupSize => members.Count;

        public double AverageGroupGrade => members.OfType<Student>().Any()
            ? Math.Round(members.OfType<Student>().Average(s => s.GetAverageLabGrade()), 2)
            : 0;

        // Практична 7

        // 1. GetAllRecords
        public StudentRecord[] GetAllRecords()
        {
            var studentsOnly = this.members.OfType<Student>().ToList();
            if (!studentsOnly.Any()) return Array.Empty<StudentRecord>();

            StudentRecord[] records = new StudentRecord[studentsOnly.Count];
            for (int i = 0; i < studentsOnly.Count; i++)
            {
                records[i] = studentsOnly[i].GetRecord();
            }
            return records;
        }

        // 2. OptimizeStorage
        public void OptimizeStorage()
        {
            var studentsOnly = this.members.OfType<Student>().ToList();
            this.labSeats = new Point[studentsOnly.Count];
            for (int i = 0; i < studentsOnly.Count; i++)
            {
                this.labSeats[i] = new Point(i + 1, 1);
                studentsOnly[i].AssignedPortRow = i + 1;
                studentsOnly[i].AssignedPortCol = 1;
            }

            this.groupGradeHistory = studentsOnly
                .SelectMany(s => s.GetGradeHistory())
                .ToArray();

            Console.WriteLine("\n Дані успішно перетворені у структури:");
            Console.WriteLine($" - Створено масив Point[{this.labSeats.Length}] для місць.");
            Console.WriteLine($" - Оптимізовано масив GradeRecord[{this.groupGradeHistory.Length}] для оцінок.");
        }

        // ------------------------------------------------

        //  Практична 4
        public Student this[string recordBookNumber]
        {
            get => members.OfType<Student>().FirstOrDefault(s => s.RecordBookNumber == recordBookNumber);
        }

        // Оператор для об'єднання двох груп студентів
        public static StudentGroup operator +(StudentGroup a, StudentGroup b)
        {
            var newGroup = new StudentGroup();
            foreach (var m in a.members) newGroup.AddMember(m);
            foreach (var m in b.members) newGroup.AddMember(m);
            return newGroup;
        }

        //Для практичної 5
        public void AddMember(UniversityMember member)
        {
            if (member == null) return;
            members.Add(member);
        }

        public void PrintAllMembers()
        {
            if (!members.Any())
            {
                Console.WriteLine("Група порожня.");
                return;
            }
            Console.WriteLine("\nСписок членів групи:");
            foreach (var m in members)
            {
                Console.WriteLine(m.GetInfo());
            }
        }

        public decimal GetTotalScholarship()
        {
            return members.Sum(m => m.CalculateScholarship());
        }

        public List<T> GetMembersByType<T>() where T : UniversityMember
        {
            return members.OfType<T>().ToList();
        }

        // --------------------------------

        public Student BestStudent()
        {
            var studentsList = members.OfType<Student>().ToList();
            if (!studentsList.Any()) return null;

            Student best = studentsList[0];
            foreach (var s in studentsList)
            {
                if (s > best) best = s;
            }
            return best;
        }

        public StudentGroup MergeGroups(StudentGroup other)
        {
            return this + other; 
        }
        //**************

        // Практична 6

        public double GetTotalAreaOfAllShapes()
        {
            double total = 0;
            foreach (var person in members)
            {
                if (person is Student student)
                {
                    foreach (var shape in student.Shapes)
                        total += shape.CalculateArea();
                }
            }
            return total;
        }

        public void DrawAllShapes()
        {
            Console.WriteLine("\n--- Візуалізація фігур групи ---");
            bool hasShapes = false;
            foreach (var member in members)
            {
                if (member is Student student && student.Shapes.Count > 0)
                {
                    hasShapes = true;
                    Console.WriteLine($"\nСтудент: {student.FullName} починає малювання:");

                    foreach (var shape in student.Shapes)
                    {
                        if (shape is IDrawable drawable)
                        {
                            Console.Write("  > ");
                            drawable.Draw(); 
                        }
                    }
                }
            }

            if (!hasShapes)
            {
                Console.WriteLine("У групі поки немає студентів із доданими фігурами.");
            }
            Console.WriteLine("===========================================");
        }

        public void ResizeAllShapes(double factor)
        {
            foreach (var member in members)
            {
                if (member is Student student)
                {
                    foreach (var shape in student.Shapes)
                    {
                        if (shape is IResizable resizable)
                        {
                            resizable.Resize(factor);
                        }
                    }
                }
            }
            Console.WriteLine($"Розмір усіх фігур змінено у {factor} разів.");
        }

        //***************

        public void AddStudent(Student s)
        {
            AddMember(s);
        }

        public void RemoveStudent(string recordBookNumber)
        {
            int removedCount = members.RemoveAll(m => m is Student s && s.RecordBookNumber == recordBookNumber);
            if (removedCount > 0)
                Console.WriteLine($"Студента з ID {recordBookNumber} видалено.");
            else
                Console.WriteLine("Студента з таким ID не знайдено.");
        }

        public Student FindStudent(string recordBookNumber)
        {
            return members.OfType<Student>().FirstOrDefault(s => s.RecordBookNumber == recordBookNumber);
        }

        public List<Student> FindStudent(string namePart, bool isNameSearch)
        {
            return members.OfType<Student>().Where(s => s.FullName.Contains(namePart, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Student> GetExcellentStudents()
        {
            return members.OfType<Student>().Where(s => s.IsExcellent()).ToList();
        }

        public List<Student> GetStudentsByStatus(StudentStatus status)
        {
            return members.OfType<Student>().Where(s => s.Status == status).ToList();
        }

        public List<Student> GetAllStudents() => members.OfType<Student>().ToList();

        public void AssignStudentToPort(Student s, int row, int col, PortMatrix matrix)
        {
            var port = matrix.GetPort(row, col);
            port.DeviceName = $"Робоче місце: {s.FullName}";
            s.AssignedPortRow = row;
            s.AssignedPortCol = col;
        }

        public List<Student> GetStudentsByPortStatus(PortMatrix matrix, bool isOpen)
        {
            return members.OfType<Student>()
                          .Where(s => s.AssignedPortRow.HasValue && matrix.GetPort(s.AssignedPortRow.Value, s.AssignedPortCol.Value).IsOpen == isOpen)
                          .ToList();
        }

        public void SaveToFile(string fileName)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.All
                };
                string json = JsonConvert.SerializeObject(members, settings);
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
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    string json = File.ReadAllText(fileName);
                    members = JsonConvert.DeserializeObject<List<UniversityMember>>(json, settings) ?? new List<UniversityMember>();
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
            var found = members.Where(m => m.FullName.Contains(fragment, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!found.Any()) return "Нічого не знайдено.";

            StringBuilder sb = new StringBuilder("Результати пошуку:\n");
            foreach (var m in found)
            {
                sb.AppendLine(m.GetInfo());
            }
            return sb.ToString();
        }

        public string ExportToCsv()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Type,FullName,RecordBook,AvgGrade");
            foreach (var m in members)
            {
                if (m is Student s)
                    sb.AppendLine($"Student,{s.FullName},{s.RecordBookNumber},{s.GetAverageGrade():F2}");
                else
                    sb.AppendLine($"{m.GetType().Name},{m.FullName},-, -");
            }
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
                        string name = p[0].Trim();
                        string record = p[1].Trim();
                        string email = (p.Length > 2 && p[2].Contains("@")) ? p[2].Trim() : "student@zpfk.edu.ua";

                        Student newStud = new Student(name, new DateTime(2005, 1, 1), email, record);

                        if (p.Length > 3) newStud.Notes = p[3].Trim();

                        AddStudent(newStud);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Помилка імпорту рядка: {e.Message}");
                    }
                }
            }
        }
    }
}
