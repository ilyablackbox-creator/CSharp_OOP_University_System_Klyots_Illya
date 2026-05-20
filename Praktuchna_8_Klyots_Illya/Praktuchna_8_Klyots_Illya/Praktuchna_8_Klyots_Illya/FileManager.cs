using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Praktuchna_8_Klyots_Illya
{
    public class FileManager
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true,
            PropertyNameCaseInsensitive = true, 
            IncludeFields = true               
        };

        public void SaveToJson<T>(T data, string filePath)
        {
            try
            {
                string json = JsonSerializer.Serialize(data, _options);
                using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    sw.Write(json);
                }
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Помилка серіалізації в JSON: {ex.Message}", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException($"Немає доступу до файлу {filePath}: {ex.Message}", ex);
            }
        }

        public T LoadFromJson<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл JSON не знайдено", filePath);

            try
            {
                using (var sr = new StreamReader(filePath, Encoding.UTF8))
                {
                    string json = sr.ReadToEnd();
                    return JsonSerializer.Deserialize<T>(json, _options);
                }
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Помилка десеріалізації JSON: {ex.Message}", ex);
            }
        }

        public void SaveToText(string content, string filePath)
        {
            try
            {
                using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    sw.Write(content);
                }
            }
            catch (IOException ex)
            {
                throw new IOException($"Помилка запису в текстовий файл: {ex.Message}", ex);
            }
        }

        public string ReadFromText(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Текстовий файл не знайдено", filePath);

            using (var sr = new StreamReader(filePath, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        public void ExportToCsv(StudentGroup group, string filePath)
        {
            try
            {
                using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    sw.WriteLine("ПІБ;Номер залікової;Середній бал");
                    foreach (var s in group.GetAllStudents())
                    {
                        sw.WriteLine($"{s.FullName};{s.RecordBookNumber};{s.GetAverageLabGrade():F2}");
                    }
                }
            }
            catch (IOException ex)
            {
                throw new IOException($"Помилка експорту в CSV: {ex.Message}", ex);
            }
        }

        public void CreateBackup(string sourcePath)
        {
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Помилка: Файл {sourcePath} не знайдено для створення бекапу.");
                return;
            }

            string backupDir = "Backups";
            if (!Directory.Exists(backupDir)) Directory.CreateDirectory(backupDir);

            string fileName = Path.GetFileNameWithoutExtension(sourcePath);
            string ext = Path.GetExtension(sourcePath);
            string destPath = Path.Combine(backupDir, $"{fileName}_backup_{DateTime.Now:yyyyMMdd_HHmmss}{ext}");

            File.Copy(sourcePath, destPath, true);
        }

        public void CleanOldBackups(int daysOld)
        {
            string backupDir = "Backups";
            if (!Directory.Exists(backupDir)) return;

            var files = Directory.GetFiles(backupDir);
            foreach (var file in files)
            {
                if (File.GetCreationTime(file) < DateTime.Now.AddDays(-daysOld))
                    File.Delete(file);
            }
        }

        // Dаріант 2: sмпорт студентів з CSV
        public List<Student> ImportFromCsv(string filePath)
        {
            if (Path.GetExtension(filePath).ToLower() != ".csv")
                throw new InvalidFileFormatException("Дозволено лише .csv файли для імпорту!");

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл CSV для імпорту не знайдено!", filePath);

            var students = new List<Student>();

            try
            {
                using (var sr = new StreamReader(filePath, Encoding.UTF8))
                {
                    string header = sr.ReadLine(); 
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var parts = line.Split(';');
                        if (parts.Length >= 2)
                        {
                            string name = parts[0].Trim();
                            string recordBook = parts[1].Trim();
                            students.Add(new Student(name, new DateTime(2005, 1, 1), "imported@zpfk.edu.ua", recordBook));
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                throw new IOException($"Помилка читання під час імпорту: {ex.Message}", ex);
            }

            return students;
        }
    }
}
