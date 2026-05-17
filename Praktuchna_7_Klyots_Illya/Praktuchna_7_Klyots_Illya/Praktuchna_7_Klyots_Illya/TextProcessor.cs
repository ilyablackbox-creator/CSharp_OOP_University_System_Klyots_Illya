using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Praktuchna_7_Klyots_Illya
{
    public static class TextProcessor
    {
        public static string Reverse(string input) => new string(input.ToCharArray().Reverse().ToArray());

        public static int CountWords(string text) =>
            string.IsNullOrWhiteSpace(text) ? 0 : text.Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;

        public static int CountCharacters(string text, bool ignoreWhitespace = true) =>
            ignoreWhitespace ? text.Replace(" ", "").Length : text.Length;

        public static string Normalize(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            return string.Join(" ", text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static bool IsPalindrome(string text, bool ignoreCase = true, bool ignoreSpaces = true)
        {
            string t = text;
            if (ignoreSpaces) t = t.Replace(" ", "");
            if (ignoreCase) t = t.ToLower();
            return t == Reverse(t);
        }

        public static string ReplaceMultiple(string text, Dictionary<string, string> replacements)
        {
            StringBuilder sb = new StringBuilder(text);
            foreach (var r in replacements) sb.Replace(r.Key, r.Value);
            return sb.ToString();
        }

        public static string[] SplitIntoSentences(string text) =>
            text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();

        internal static string BuildGroupReport(StudentGroup group)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ЗВЯГЕЛЬСЬКИЙ ПОЛІТЕХНІЧНИЙ ФАХОВИЙ КОЛЕДЖ");
            sb.AppendLine($"ЗВІТ ГРУПИ ВІД {DateTime.Now:dd.MM.yyyy}");
            sb.AppendLine(new string('=', 40));
            foreach (var s in group.GetAllStudents())
            {
                sb.Append(s.GetFormattedInfo(true));
                sb.AppendLine("----------------------------------------");
            }
            return sb.ToString();
        }

        public static string ComparePerformance(int iterations)
        {
            Stopwatch sw = Stopwatch.StartNew();
            string s = "";
            for (int i = 0; i < iterations; i++) s += "a";
            sw.Stop();
            long stringTime = sw.ElapsedMilliseconds;
            //ок
            sw.Restart();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iterations; i++) sb.Append("a");
            sw.Stop();
            long sbTime = sw.ElapsedMilliseconds;

            return $"Результат ({iterations} іт.):\nString: {stringTime}ms\nStringBuilder: {sbTime}ms";
        }

        internal static string GenerateCharacterization(Student student)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"--- ХАРАКТЕРИСТИКА СТУДЕНТА ---");
            sb.AppendLine($"ПІБ: {student.FullName}");
            sb.AppendLine($"Номер залікової книжки: {student.RecordBookNumber}");
            sb.AppendLine($"Статус: {student.Status}");

            double avg = student.GetAverageLabGrade();
            sb.Append($"За час навчання показав ");

            if (avg >= 90) sb.Append("високий рівень знань та відмінні успіхи у навчанні. ");
            else if (avg >= 7) sb.Append("добрі результати та сумлінне ставлення до виконання завдань. ");
            else if (avg >= 6) sb.Append("достатній рівень володіння матеріалом. ");
            else sb.Append("недостатній рівень успішності, що потребує посиленого контролю. ");

            sb.AppendLine($"\nСередній бал з лабораторних робіт: {avg:F2}");

            if (!string.IsNullOrEmpty(student.Notes))
            {
                sb.AppendLine($"Додаткові відомості: {student.Notes}");
            }
            //ок
            sb.AppendLine("\nДата видачі: " + DateTime.Now.ToShortDateString());
            return sb.ToString();
        }
    }
}