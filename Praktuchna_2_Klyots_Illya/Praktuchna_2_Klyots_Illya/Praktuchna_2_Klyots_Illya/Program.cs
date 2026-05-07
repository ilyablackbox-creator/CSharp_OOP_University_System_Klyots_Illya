using Newtonsoft.Json;
using Praktuchna_2_Klyots_Illya;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Praktuchna_2_Klyots_Illya
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            StudentGroup group = new StudentGroup { };
            PortMatrix matrix = new PortMatrix(); 
            PortLogger logger = new PortLogger();

            string jsonPath = "group_data.json";

            while (true)
            {
                StringBuilder menu = new StringBuilder();
                menu.AppendLine($"\n МЕНЮ УПРАВЛІННЯ ГРУПОЮ П-21 (ПР № 2) ");
                menu.AppendLine("1. Додати студента");
                menu.AppendLine("2. Видалити студента (за номером залікової)");
                menu.AppendLine("3. Вивести всіх студентів (пагінація по 10)");
                menu.AppendLine("4. Пошук студента (ПІБ або номер залікової)");
                menu.AppendLine("5. Редагування даних студента");
                menu.AppendLine("6. Вивести відмінників та боржників (< 60)");
                menu.AppendLine("7. Вивести cтатистику групи");
                menu.AppendLine("8. Зберегти / Завантажити дані");
                menu.AppendLine("9. Стан матриці портів (16x16)");
                menu.AppendLine("10. Відкрити/Закрити порт");
                menu.AppendLine("11. Симулювати лабораторну (Оцінка + Порт)");
                menu.AppendLine("12. Переглянути лог операцій (StringBuilder)");
                menu.AppendLine("13. Записати дані в порт");
                menu.AppendLine("14. Пошук відкритих портів у матриці");
                menu.AppendLine("15. Рейтинг студентів за оцінками лабораторних");
                menu.AppendLine("0. Вийти");
                menu.AppendLine("////////////////////////////////////////");
                
                Console.WriteLine(menu.ToString());
                Console.Write("Вибір: ");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1": AddStudentMenu(group); break;
                        case "2": RemoveStudentMenu(group); break;
                        case "3": ShowAllStudents(group); break;
                        case "4": FindAndShowStudent(group); break;
                        case "5": EditStudentStatus(group); break;
                        case "6": ShowExcellentAndFailing(group); break;
                        case "7": ShowGroupStats(group); break;
                        case "8": SaveOrLoadData(group, jsonPath); break;
                        case "9": Console.WriteLine(matrix.ScanMatrix()); break;
                        case "10": ManualPortControl(matrix, logger); break;
                        case "11": RunLabSimulation(group, matrix, logger); break;
                        case "12": ShowFullSystemLog(logger); break;
                        case "13": TransferDataToPort(matrix, logger); break;
                        case "14": ShowOpenPortsAndAssignedStudents(matrix, group); break;
                        case "15": SortAndShowStudents(group); break;
                        default: Console.WriteLine("Невірний вибір!"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nПОМИЛКА: {ex.Message}");
                }

                static void AddStudentMenu(StudentGroup group)
                {
                    Console.Write("1-Новий, 2-Клон: ");
                    string choice = Console.ReadLine();

                    if (choice == "2" && group.GetAllStudents().Any())
                    {
                        var cloned = (Student)group.GetAllStudents().Last().Clone();
                        Console.Write($"ПІБ (Enter для {cloned.FullName}): ");
                        string n = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(n)) cloned.FullName = n;
                        Console.Write("Новий номер залікової: ");
                        cloned.RecordBookNumber = Console.ReadLine();
                        group.AddStudent(cloned);
                        Console.WriteLine("Клон успішно доданий.");
                    }
                    else
                    {
                        Console.Write("Введіть ПІБ: "); string name = Console.ReadLine();
                        Console.Write("Введіть номер залікової (8 цифр): "); string id = Console.ReadLine();
                        Console.Write("Введіть Email: "); string email = Console.ReadLine();
                        Console.Write("Введіть дату народження (мм.дд.рррр): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out DateTime birthDate))
                            birthDate = new DateTime(2000, 1, 1);

                        Student newStudent = new Student
                        {
                            FullName = name,
                            RecordBookNumber = id,
                            PersonalEmail = email,
                            DateOfBirth = birthDate,
                            EnrollmentDate = DateTime.Now,
                            Status = StudentStatus.Active
                        };
                        Console.Write("Введіть середній бал: ");
                        if (double.TryParse(Console.ReadLine(), out double grade)) newStudent.UpdateAverageGrade(grade);

                        group.AddStudent(newStudent);
                        Console.WriteLine("Студента успішно додано.");
                    }
                }

                static void RemoveStudentMenu(StudentGroup group)
                {
                    Console.Write("Введіть номер залікової для видалення: ");
                    group.RemoveStudent(Console.ReadLine());
                }

                static void ShowAllStudents(StudentGroup group)
                {
                    var all = group.GetAllStudents();
                    Console.WriteLine($"\n Список студентів (Кількість: {group.GroupSize})");
                    for (int i = 0; i < all.Count; i++)
                    {
                        Console.WriteLine(all[i]);
                        if ((i + 1) % 10 == 0 && i != all.Count - 1) Console.ReadKey();
                    }
                }

                static void FindAndShowStudent(StudentGroup group)
                {
                    Console.Write("Введіть ПІБ або номер залікової: ");
                    var student = group.FindStudent(Console.ReadLine());
                    if (student != null)
                    {
                        student.ShowDetailedInfo();
                        Console.WriteLine("\n Журнал оцінок");
                        var grades = student.Journal.GetAllGrades();
                        if (grades.Count > 0) foreach (var g in grades) Console.WriteLine($"Лабораторна №{g.Key}: {g.Value}");
                        else Console.WriteLine("Оцінок ще немає.");
                    }
                    else Console.WriteLine("Не знайдено.");
                }

                static void EditStudentStatus(StudentGroup group)
                {
                    Console.Write("Введіть номер залікової студента: ");
                    var st = group.FindStudent(Console.ReadLine());
                    if (st != null)
                    {
                        Console.Write($"Новий ПІБ (теперішній: {st.FullName}): ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newName)) st.FullName = newName;
                        Console.WriteLine("1. Активний | 2. Академічна відпусткка | 3. Відрахований | 4. Випускник");
                        if (int.TryParse(Console.ReadLine(), out int sIdx)) st.Status = (StudentStatus)(sIdx - 1);
                        Console.Write("Додати нотатку?: ");
                        string note = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(note)) st.Notes = note;
                        Console.WriteLine("Дані оновлено.");
                    }
                    else Console.WriteLine("Студента не знайдено.");
                }

                static void ShowExcellentAndFailing(StudentGroup group)
                {
                    Console.WriteLine("\nВІДМІННИКИ (>= 90) ");
                    group.GetExcellentStudents().ForEach(s => Console.WriteLine(s));
                    Console.WriteLine("\nБОРЖНИКИ (< 60) ");
                    group.GetAllStudents().Where(s => s.IsFailing()).ToList().ForEach(s => Console.WriteLine(s));
                }

                static void ShowGroupStats(StudentGroup group)
                {
                    int total = group.GroupSize;
                    double perc = total > 0 ? (double)group.GetExcellentStudents().Count / total * 100 : 0;
                    Console.WriteLine($"Група: {total} чол. | Сер. бал: {group.AverageGroupGrade:F2} | Відмінники: {perc:F1}%");
                }

                static void SaveOrLoadData(StudentGroup group, string path)
                {
                    Console.WriteLine("1. Зберегти у JSON | 2. Завантажити з JSON");
                    if (Console.ReadLine() == "1") group.SaveToFile(path);
                    else group.LoadFromFile(path);
                }

                static void ManualPortControl(PortMatrix matrix, PortLogger logger)
                {
                    Console.Write("Ряд (0-15): "); int r = int.Parse(Console.ReadLine());
                    Console.Write("Стовпчик (0-15): "); int c = int.Parse(Console.ReadLine());
                    Console.Write("1-Відкрити, 2-Закрити: ");
                    bool op = Console.ReadLine() == "1";
                    if (op) matrix.OpenPort(r, c); else matrix.GetPort(r, c).Close();
                    logger.LogOperation(op ? "Open" : "Close", r * 16 + c, op ? "On" : "Off");
                    Console.WriteLine("Готово.");
                }

                static void RunLabSimulation(StudentGroup group, PortMatrix matrix, PortLogger logger)
                {
                    Console.Write("Номер залікової: ");
                    var st = group.FindStudent(Console.ReadLine());
                    if (st != null)
                    {
                        Console.Write("Номер лаби (1-10): "); int lNum = int.Parse(Console.ReadLine());
                        Console.Write("Оцінка (0-100): "); byte lGrade = byte.Parse(Console.ReadLine());
                        st.AddLabGrade(lNum, lGrade);
                        if (!st.AssignedPortRow.HasValue) group.AssignStudentToPort(st, 0, 0, matrix);
                        matrix.OpenPort(st.AssignedPortRow.Value, st.AssignedPortCol.Value);
                        matrix.WriteToPort(st.AssignedPortRow.Value, st.AssignedPortCol.Value, Encoding.UTF8.GetBytes($"Grade:{lGrade}"));
                        logger.LogOperation("LAB_SIM", lNum, $"Студент {st.FullName} виконав роботу.");
                        Console.WriteLine("Симуляція завершена успішно.");
                    }
                }

                static void ShowFullSystemLog(PortLogger logger)
                {
                    logger.GenerateLargeReport();
                    Console.WriteLine(logger.GetFullLog());
                    logger.SaveLogToFile();
                    Console.WriteLine("\n Лог збережено у файл.");
                }

                static void TransferDataToPort(PortMatrix matrix, PortLogger logger)
                {
                    Console.Write("Ряд (0-15): "); int rW = int.Parse(Console.ReadLine());
                    Console.Write("Стовпчик (0-15): "); int cW = int.Parse(Console.ReadLine());
                    Console.Write("Введіть дані: "); string input = Console.ReadLine();
                    matrix.WriteToPort(rW, cW, Encoding.UTF8.GetBytes(input));
                    logger.LogOperation("Write", rW * 16 + cW, $"Записано: {input}");
                    string confirmed = Encoding.UTF8.GetString(matrix.ReadFromPort(rW, cW)).TrimEnd('\0');
                    Console.WriteLine($"Підтвердження з буфера: {confirmed}");
                }

                static void ShowOpenPortsAndAssignedStudents(PortMatrix matrix, StudentGroup group)
                {
                    Console.WriteLine(matrix.FindOpenPorts());
                    Console.WriteLine("\n Студенти на ВІДКРИТИХ портах:");
                    var active = group.GetStudentsByPortStatus(matrix, true);
                    if (active.Any()) active.ForEach(s => Console.WriteLine($" - {s.FullName} (Порт: {s.AssignedPortRow}:{s.AssignedPortCol})"));
                    else Console.WriteLine(" Нікого не закріплено.");
                }

                static void SortAndShowStudents(StudentGroup group)
                {
                    Console.WriteLine("1. Сортувати за оцінками | 2. Сортувати за алфавітом");
                    string choice = Console.ReadLine();
                    var list = group.GetAllStudents();
                    if (choice == "2") { list.Sort(); Console.WriteLine("\n Список за алфавітом:"); }
                    else { list = list.OrderByDescending(s => s.GetAverageLabGrade()).ToList(); Console.WriteLine("\n Рейтинг за оцінками:"); }
                    foreach (var s in list) Console.WriteLine(s);
                }
            }
        }
    }
}
