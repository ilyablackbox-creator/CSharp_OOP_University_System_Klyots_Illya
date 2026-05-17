using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_7_Klyots_Illya
{
    internal class PortLogger
    {
        private StringBuilder logs = new StringBuilder();

        public void LogOperation(string operation, int portNumber, string details)
        {
            logs.AppendLine($"[{DateTime.Now:HH:mm:ss}] Порт №{portNumber}: {operation} | Деталі: {details}");
        }

        public void SaveLogToFile() 
        {
            File.WriteAllText("port_logs.txt", logs.ToString());
        }

        public string GetFullLog() => logs.ToString();

        public void GenerateLargeReport() 
        {
            logs.AppendLine("--- ГЕНЕРАЦІЯ ВЕЛИКОГО ЗВІТУ ---");
            for (int i = 1; i <= 110; i++)
                logs.AppendLine($"Запис #{i}: Перевірка цілісності даних порту... OK");
        }
    }
}
