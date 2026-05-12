using Newtonsoft.Json;
using Praktuchna_3_Klyots_Illya;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Praktuchna_3_Klyots_Illya
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            StudentGroup group = new StudentGroup { };
            PortMatrix matrix = new PortMatrix();
            PortLogger logger = new PortLogger();
            AdvancedLogger advancedLogger = new AdvancedLogger();

            string jsonPath = "group_data.json";

            while (true)
            {
                StringBuilder menu = new StringBuilder();
                menu.AppendLine($"\n======= МЕНЮ УПРАВЛІННЯ ГРУПОЮ П-21 (ПР № 3) =======");
                menu.AppendLine("1. Додати студента");
                menu.AppendLine("2. Видалити студента (за номером залікової)");
                menu.AppendLine("3. Вивести всіх студентів (пагінація по 10)");
                menu.AppendLine("4. Пошук студента (ПІБ або номер залікової)");
                menu.AppendLine("5. Редагування даних студента");
                menu.AppendLine("6. Відмінники / ті, хто має < 60 балів");
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
                menu.AppendLine("19. Стан матриці портів та симуляція (ПР №2)");
                menu.AppendLine("20. Характеристики студентів");
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
                            PortSubMenu(matrix, logger, group);
                            break;
                        default: Console.WriteLine("Невірний вибір!"); break;
                        case "20":
                            Console.Write("Введіть номер залікової студента для характеристики: ");
                            string id = Console.ReadLine();
                            var st = group.FindStudent(id);
                            if (st != null)
                            {
                                Console.WriteLine(TextProcessor.GenerateCharacterization(st));
                            }
                            else Console.WriteLine("Студента не знайдено.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nПОМИЛКА: {ex.Message}");
                }

                static void PortSubMenu(PortMatrix matrix, PortLogger logger, StudentGroup group)
                {
                    Console.WriteLine("\n--- ФУНКЦІЇ ПР №2 (МАТРИЦЯ ТА ПОРТИ) ---");
                    Console.WriteLine("1. Стан матриці");
                    Console.WriteLine("2. Керування портом (Відкрити/Закрити)");
                    Console.WriteLine("3. Симуляція лабораторної");
                    Console.WriteLine("4. Запис даних у порт");
                    Console.WriteLine("5. Пошук відкритих портів");
                    Console.WriteLine("6. Рейтинг студентів");
                    Console.Write("Вибір: ");

                    string c = Console.ReadLine();
                    switch (c)
                    {
                        case "1": Console.WriteLine(matrix.ScanMatrix()); break;
                        case "2": ManualPortControl(matrix, logger); break;
                        case "3": RunLabSimulation(group, matrix, logger); break;
                        case "4": TransferDataToPort(matrix, logger); break;
                        case "5": ShowOpenPortsAndAssignedStudents(matrix, group); break;
                        case "6": SortAndShowStudents(group); break;
                    }
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
                            DateOfBirth = DateTime.MinValue,
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
