using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_8_Klyots_Illya
{
    public class PortMatrix
    {
        private Port[,] matrix = new Port[16, 16];

        public PortMatrix()
        {
            for (int i = 0; i < 256; i++) matrix[i / 16, i % 16] = new Port(i);
        }

        public Port GetPort(int r, int c)
        {
            if (r < 0 || r >= 16 || c < 0 || c >= 16)
                throw new IndexOutOfRangeException("Координати матриці мають бути від 0 до 15.");
            return matrix[r, c];
        }

        public void OpenPort(int r, int c) => GetPort(r, c).Open(); 

        public void WriteToPort(int r, int c, byte[] data) => GetPort(r, c).WriteData(data); 

        public byte[] ReadFromPort(int r, int c) => GetPort(r, c).DataBuffer;

        public string ScanMatrix()
        {
            var sb = new StringBuilder();
            sb.AppendLine("\n Карта матриці портів (16x16)");
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    sb.Append(matrix[i, j].IsOpen ? "[X] " : "[. ] ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public string FindOpenPorts() 
        {
            StringBuilder sb = new StringBuilder("Відкриті порти: ");
            bool found = false;
            foreach (var p in matrix)
            {
                if (p.IsOpen) { sb.Append($"{p.PortNumber} "); found = true; }
            }
            return found ? sb.ToString() : "Відкритих портів не знайдено.";
        }
    }
}
