using Newtonsoft.Json;
using Praktuchna_4_Klyots_Illya;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Praktuchna_4_Klyots_Illya
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            StudentGroup group = new StudentGroup ();
            StudentGroup group1 = new StudentGroup();
            SeedData(group1);
            PortMatrix matrix = new PortMatrix();
            PortLogger logger = new PortLogger();
            AdvancedLogger advancedLogger = new AdvancedLogger();

            string jsonPath = "group_data.json";

            while (true)
            {
                StringBuilder menu = new StringBuilder();
                menu.AppendLine($"\n======= МЕНЮ УПРАВЛІННЯ ГРУПОЮ (ПР № 4) =======");
                menu.AppendLine("1. Додати студента");
                menu.AppendLine("2. Видалити студента (за номером залікової)");
                menu.AppendLine("3. Вивести всіх студентів (пагінація по 10)");
                menu.AppendLine("4. Пошук студента (ПІБ або номер залікової)");
                menu.AppendLine("5. Редагування даних студента");
                menu.AppendLine("6. Відмінники / ті, хто має < 6 балів");
                menu.AppendLine("7. Статистика групи");
                menu.AppendLine("8. Зберегти дані");
                menu.AppendLine("9. Завантажити дані");
                menu.AppendLine("10. Пошук за фрагментом ПІБ");
                menu.AppendLine("11. Згенерувати повний звіт групи (StringBuilder)");
                menu.AppendLine("12. Нормалізувати нотатки всіх студентів");
                menu.AppendLine("13. Перевірити паліндроми в нотатках");
                menu.AppendLine("14. Експорт групи у CSV");
                menu.AppendLine("15. Імпорт студентів з текстового блоку");
                menu.AppendLine("16. Переглянути логи системи (AdvancedLogger)");
                menu.AppendLine("17. Порівняти продуктивність string vs StringBuilder");
                menu.AppendLine("18. Обробка тексту (реверс, підрахунок слів тощо)");
                menu.AppendLine("19. Характеристики студентів");
                menu.AppendLine("Практична робота №4");
                menu.AppendLine("20. Порівняти двох студентів (оператори >, <, ==)");
                menu.AppendLine("21. Об’єднати дві групи (оператор +)");
                menu.AppendLine("22. Продемонструвати роботу з класом Vector");
                menu.AppendLine("23. Продемонструвати роботу з GradePoint");
                menu.AppendLine("24. Знайти найкращого студента (оператори порівняння)");
                menu.AppendLine("25. Тестування перевантажених операторів (Дроби)");
                menu.AppendLine("0. Вийти");
                menu.AppendLine("====================================================");

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
                        case "8": group.SaveToFile(jsonPath); Console.WriteLine("Дані збережено."); break;
                        case "9": group.LoadFromFile(jsonPath); Console.WriteLine("Дані завантажено."); break;
                        case "10":
                            Console.Write("Введіть фрагмент ПІБ: ");
                            Console.WriteLine(group.SearchByNameFragment(Console.ReadLine()));
                            break;
                        case "11":
                            Console.WriteLine(TextProcessor.BuildGroupReport(group));
                            break;
                        case "12":
                            foreach (var s in group.GetAllStudents())
                                s.Notes = TextProcessor.Normalize(s.Notes ?? "");
                            Console.WriteLine("Всі нотатки нормалізовано.");
                            break;
                        case "13":
                            foreach (var s in group.GetAllStudents().Where(st => !string.IsNullOrEmpty(st.Notes)))
                                if (TextProcessor.IsPalindrome(s.Notes))
                                    Console.WriteLine($"Паліндром знайдено у {s.FullName}: {s.Notes}");
                            break;
                        case "14":
                            File.WriteAllText("export_group.csv", group.ExportToCsv(), Encoding.UTF8);
                            Console.WriteLine("Дані експортовано в export_group.csv");
                            break;
                        case "15":
                            Console.WriteLine("Введіть текстовий блок (ПІБ;Номер;Нотатки). Для завершення введіть 'END':");
                            StringBuilder importSb = new StringBuilder();
                            string input;
                            while ((input = Console.ReadLine()) != "END") importSb.AppendLine(input);
                            group.ImportStudentsFromText(importSb.ToString());
                            break;
                        case "16":
                            Console.WriteLine(advancedLogger.GetFullLog());
                            break;
                        case "17":
                            Console.Write("Кількість ітерацій: ");
                            if (int.TryParse(Console.ReadLine(), out int iter))
                                Console.WriteLine(TextProcessor.ComparePerformance(iter));
                            break;
                        case "18":
                            Console.Write("Введіть текст для обробки: ");
                            string rawTxt = Console.ReadLine();
                            Console.WriteLine($"Реверс: {TextProcessor.Reverse(rawTxt)}");
                            Console.WriteLine($"Кількість слів: {TextProcessor.CountWords(rawTxt)}");
                            break;
                        case "19":
                            Console.Write("Введіть номер залікової студента для характеристики: ");
                            string id = Console.ReadLine();
                            var st = group.FindStudent(id);
                            if (st != null)
                            {
                                Console.WriteLine(TextProcessor.GenerateCharacterization(st));
                            }
                            else Console.WriteLine("Студента не знайдено.");
                            break;
                        case "20":
                            CompareStudentsDemo(group1);
                            break;
                        case "21":
                            MergeGroupsDemo(group, group1);
                            break;
                        case "22":
                            VectorDemo();
                            break;
                        case "23":
                            GradePointDemo();
                            break;
                        case "24":
                            ShowBestStudent(group1);
                            break;
                        case "25":
                            FractionDemo();
                            break;
                        case "text":
                            Console.WriteLine("Введіть текст:");
                            string txt = Console.ReadLine();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nПОМИЛКА: {ex.Message}");
                }
            }
        }

        // Практична 4
        static void SeedData(StudentGroup g)
        {
            g.AddStudent(new Student
            {
                FullName = "Кльоц Ілля Романович", 
                RecordBookNumber = "20260001",    
                DateOfBirth = new DateTime(2007, 1, 1),
                EnrollmentDate = DateTime.Now,
                PersonalEmail = "illya@example.com",
                Status = StudentStatus.Active,
                GradesPoints = new List<GradePoint> { new(10), new(9) }
            });
        }

        static void MergeGroupsDemo(StudentGroup g1, StudentGroup g2)
        {
            var mergedGroup = g1 + g2;
            Console.WriteLine($"Групи об'єднано. Новий розмір: {mergedGroup.GroupSize}");
        }

        static void CompareStudentsDemo(StudentGroup group)
        {
            var students = group.GetAllStudents();
            if (students.Count < 2)
            {
                Console.WriteLine("Додайте хоча б двох студентів для порівняння.");
                return;
            }
            Student s1 = students[0];
            Student s2 = students[1];

            Console.WriteLine($"Порівнюємо {s1.FullName} та {s2.FullName}:");
            Console.WriteLine($"{s1.FullName} > {s2.FullName}: {s1 > s2}");
            Console.WriteLine($"{s1.FullName} == {s2.FullName}: {s1 == s2}");
        }

        static void ShowBestStudent(StudentGroup group)
        {
            var best = group.BestStudent();
            Console.WriteLine(best != null ? $"Найкращий у групі: {best}" : "Список порожній.");
        }

        static void VectorDemo()
        {
            Vector v1 = new Vector(1, 2, 3);
            Vector v2 = new Vector(4, 5, 6);
            Console.WriteLine($"Вектор 1: {v1}, Вектор 2: {v2}");
            Console.WriteLine($"Сума: {v1 + v2}");
        }

        static void GradePointDemo()
        {
            GradePoint gp = new GradePoint(8.5);
            Console.WriteLine($"Поточна оцінка: {gp}");
            gp++;
            Console.WriteLine($"Після інкременту (++): {gp}");
        }

        static void FractionDemo()
        {
            Fraction f1 = new Fraction(1, 2);
            Fraction f2 = new Fraction(1, 3);
            Console.WriteLine($"{f1} * {f2} = {f1 * f2}");
        }

        //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


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
                    PersonalEmail = email, // Додайте це
                    DateOfBirth = birthDate, // Додайте це
                    EnrollmentDate = DateTime.Now, // Додайте це
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
