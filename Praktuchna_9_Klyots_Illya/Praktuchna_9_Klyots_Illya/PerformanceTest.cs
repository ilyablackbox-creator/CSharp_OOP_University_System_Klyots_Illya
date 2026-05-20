using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_9_Klyots_Illya
{
    public class PerformanceTest
    {
        public static void Run(int count = 100000)
        {
            Console.WriteLine($"\n--- ПОРІВНЯННЯ ПРОДУКТИВНОСТІ ({count} елементів) ---");

            // 1. Тестування (Student)
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memStartClass = GC.GetTotalMemory(true);
            Stopwatch sw = Stopwatch.StartNew();

            Student[] classArray = new Student[count];
            for (int i = 0; i < count; i++)
            {
                classArray[i] = new Student($"Прізвище Ім'я {i}", new DateTime(2005, 1, 1), "test@zpfk.edu.ua", (10000000 + i).ToString());
            }

            sw.Stop();
            long memEndClass = GC.GetTotalMemory(true);
            long classTime = sw.ElapsedMilliseconds;

            // 2. Тестування (StudentRecord)
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memStartStruct = GC.GetTotalMemory(true);
            sw.Restart();

            StudentRecord[] structArray = new StudentRecord[count];
            for (int i = 0; i < count; i++)
            {
                structArray[i] = new StudentRecord($"Студент {i}", (10000000 + i).ToString());
            }

            sw.Stop();
            long memEndStruct = GC.GetTotalMemory(true);
            long structTime = sw.ElapsedMilliseconds;
            Console.WriteLine($"{"Тип (Об'єкт)",-15} | {"Час (мс)",-10} | {"Пам'ять (КБ)",-15}");
            Console.WriteLine(new string('-', 45));
            long classMem = (memEndClass - memStartClass) / 1024;
            long structMem = (memEndStruct - memStartStruct) / 1024;
            Console.WriteLine($"{"Class (Student)",-15} | {classTime,-10} | {Math.Max(0, classMem),-15}");
            Console.WriteLine($"{"Struct (Record)",-15} | {structTime,-10} | {Math.Max(0, structMem),-15}");
            Console.WriteLine(new string('-', 45));
            Console.WriteLine("Висновок: Структури зазвичай займають менше місця в Heap та швидше створюються.");
        }
    }
}