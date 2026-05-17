using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktuchna_6_Klyots_Illya
{
    public class PortAccessException : Exception
    {
        public PortAccessException(string message) : base(message) { }
    }

    internal class Port : ICloneable
    {
        public int PortNumber { get; }
        public byte[] DataBuffer { get; private set; } = new byte[64]; // 1.Б
        public bool IsOpen { get; private set; }
        public string DeviceName { get; set; }

        public Port(int number, string device = "Empty Slot")
        {
            PortNumber = number;
            DeviceName = device;
        }

        public void Open() => IsOpen = true;
        public void Close() => IsOpen = false;

        public void WriteData(byte[] data)
        {
            if (!IsOpen) throw new PortAccessException($"Порт №{PortNumber} закритий для запису!"); // 5
            Array.Clear(DataBuffer, 0, DataBuffer.Length);
            Array.Copy(data, 0, DataBuffer, 0, Math.Min(data.Length, 64));
        }

        public object Clone() 
        {
            var clone = (Port)this.MemberwiseClone();
            clone.DataBuffer = (byte[])this.DataBuffer.Clone(); 
            return clone;
        }
    }
}
