using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_5_Klyots_Illya
{
    public class AdvancedLogger
    {
        private StringBuilder _logs = new StringBuilder();
        private StringBuilder _logAccumulator = new StringBuilder();

        public static void LogInfo(string message) => Console.WriteLine($"[INFO] {DateTime.Now}: {message}");

        public static void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[SUCCESS]: {message}");
            Console.ResetColor();
        }

        public void Log(string level, string message)
        {
            _logAccumulator.AppendLine($"[{level}] {message}");
        }

        public void SaveToFile(string path) => System.IO.File.WriteAllText(path, _logs.ToString());

        public string GetLogsByLevel(string level)
        {
            var all = _logs.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return string.Join(Environment.NewLine, all.Where(l => l.Contains($"[{level.ToUpper()}]")));
        }

        public void Clear() => _logs.Clear();

        public string GetLast(int count)
        {
            var lines = _logs.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(Environment.NewLine, lines.Skip(Math.Max(0, lines.Length - count)));
        }

        public string GetFullLog()
        {
            return _logAccumulator.ToString();
        }
    }
}
