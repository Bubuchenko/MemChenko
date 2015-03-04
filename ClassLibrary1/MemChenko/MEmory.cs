using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemChenko
{
    public class Memory
    {
        public Process OpenedProcess { get; set; }
        public IntPtr OpenedProcessHandle
        {
            get { return OpenedProcess.Handle; }
        }
        public bool OpenProcess(string ProcessName)
        {
            Process processName = Process.GetProcessesByName(ProcessName).FirstOrDefault();

            if(processName != null)
            {
                OpenedProcess = processName;
                return true;
            }

            return false;
        }
        public bool OpenProcess(int ProcessID)
        {
            Process processName = Process.GetProcessById(ProcessID);

            if (processName != null)
            {
                OpenedProcess = processName;
                return true;
            }

            return false;
        }
        public int BaseAddress
        {
            get { return OpenedProcess.MainModule.BaseAddress.ToInt32(); }
        }
        public int EntryPoint
        {
            get { return OpenedProcess.MainModule.EntryPointAddress.ToInt32(); }
        }
        public string Name
        {
            get { return OpenedProcess.ProcessName; }
        }
        public int PID
        {
            get { return OpenedProcess.Id; }
        }
        public string StartTime
        {
            get { return OpenedProcess.StartTime.ToString(); }
        }
        public static int ConvertToDecimal(int iHex)
        {
            return int.Parse(iHex.ToString(), NumberStyles.HexNumber);
        }
        public static int ConvertToDecimal(string sHex)
        {
            return int.Parse(sHex, NumberStyles.HexNumber);
        }
        private string CreateAddress(byte[] Bytes)
        {
            Array.Reverse(Bytes);
            return BitConverter.ToString(Bytes).Replace("-", string.Empty);
        }
        public int CalculatePointer(int MemoryAddress, int[] Offsets)
        {
            byte[] buffer = new byte[4];
            IntPtr ptr;

            int Address = BaseAddress + MemoryAddress;

            for(int i = 0; i < Offsets.Length; i++)
            {
                MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)Address, buffer, sizeof(int), out ptr);
                Address = ConvertToDecimal(CreateAddress(buffer));
                
                Address += Offsets[i];
            }

            return Address;
        }

        public string ReadString(int MemoryAddress, uint TextLength, TextEncoding TextEncodingType = TextEncoding.ASCII)
        {
            IntPtr ptr;
            byte[] value = new byte[TextLength];
            MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)MemoryAddress, value, TextLength, out ptr);

            switch(TextEncodingType)
            {
                case TextEncoding.ASCII:
                    return Encoding.ASCII.GetString(value);
                case TextEncoding.UTF8:
                    return Encoding.UTF8.GetString(value);
                case TextEncoding.UNICODE:
                    return Encoding.Unicode.GetString(value);
                default:
                    return Encoding.ASCII.GetString(value);
            }
        }
        public string ReadString(int MemoryAddress, int[] Offsets, int StringLength, TextEncoding TextEncodingType = TextEncoding.ASCII)
        {
            IntPtr ptr;
            int NewAddress = CalculatePointer(MemoryAddress, Offsets);
            byte[] value = new byte[StringLength];
            MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)NewAddress, value, (uint)StringLength, out ptr);

            switch(TextEncodingType)
            {
                case TextEncoding.ASCII:
                    return Encoding.ASCII.GetString(value);
                case TextEncoding.UTF8:
                    return Encoding.UTF8.GetString(value);
                case TextEncoding.UNICODE:
                    return Encoding.Unicode.GetString(value);
                default:
                    return Encoding.ASCII.GetString(value);
            }
        }

        public int ReadInt32(int MemoryAddress, int[] Offsets)
        {
            IntPtr ptr;
            int NewAddress = CalculatePointer(MemoryAddress, Offsets);
            byte[] value = new byte[sizeof(Int32)];
            MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)NewAddress, value, (uint)sizeof(Int32), out ptr);

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(value);

            return BitConverter.ToInt32(value, 0);
        }
        public int ReadInt32(int MemoryAddress)
        {
            IntPtr ptr;
            byte[] value = new byte[sizeof(Int32)];
            MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)MemoryAddress, value, (uint)sizeof(Int32), out ptr);

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(value);

            return BitConverter.ToInt32(value, 0);
        }

        public bool WriteInt32(int MemoryAddress, int Value)
        {
            IntPtr ptr;
            byte[] bytes = BitConverter.GetBytes(Value);
            MemBase.WriteProcessMemory(OpenedProcessHandle, (IntPtr)MemoryAddress, bytes, sizeof(Int32), out ptr);
            return (ptr.ToInt32() == sizeof(Int32));
        }
        public bool WriteInt32(int MemoryAddress, int[] Offsets, int Value)
        {
            IntPtr ptr;
            int newAddress = CalculatePointer(MemoryAddress, Offsets);
            byte[] bytes = BitConverter.GetBytes(Value);
            MemBase.WriteProcessMemory(OpenedProcessHandle, (IntPtr)newAddress, bytes, sizeof(Int32), out ptr);
            return (ptr.ToInt32() == sizeof(Int32));
        }

        public float ReadFloat(int MemoryAddress, int[] Offsets)
        {
            IntPtr ptr;
            int NewAddress = CalculatePointer(MemoryAddress, Offsets);
            byte[] value = new byte[sizeof(float)];
            MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)NewAddress, value, (uint)sizeof(Int32), out ptr);

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(value);

            return BitConverter.ToSingle(value, 0);
        }
        public float ReadFloat(int MemoryAddress)
        {
            IntPtr ptr;
            byte[] value = new byte[sizeof(float)];
            MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)MemoryAddress, value, (uint)sizeof(Int32), out ptr);

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(value);

            return BitConverter.ToSingle(value, 0);
        }

        public bool WriteFloat(int MemoryAddress, float Value)
        {
            IntPtr ptr;
            byte[] bytes = BitConverter.GetBytes(Value);
            MemBase.WriteProcessMemory(OpenedProcessHandle, (IntPtr)MemoryAddress, bytes, sizeof(float), out ptr);
            return (ptr.ToInt32() == sizeof(float));
        }
        public bool WriteFloat(int MemoryAddress, int[] Offsets, float Value)
        {
            IntPtr ptr;
            int newAddress = CalculatePointer(MemoryAddress, Offsets);
            byte[] bytes = BitConverter.GetBytes(Value);
            MemBase.WriteProcessMemory(OpenedProcessHandle, (IntPtr)newAddress, bytes, sizeof(float), out ptr);
            return (ptr.ToInt32() == sizeof(float));
        }

        public double ReadDouble(int MemoryAddress, int[] Offsets)
        {
            IntPtr ptr;
            int NewAddress = CalculatePointer(MemoryAddress, Offsets);
            byte[] value = new byte[sizeof(double)];
            MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)NewAddress, value, (uint)sizeof(double), out ptr);

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(value);

            return BitConverter.ToDouble(value, 0);
        }
        public double ReadDouble(int MemoryAddress)
        {
            IntPtr ptr;
            byte[] value = new byte[sizeof(double)];
            MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)MemoryAddress, value, (uint)sizeof(double), out ptr);

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(value);

            return BitConverter.ToDouble(value, 0);
        }

        public bool WriteDouble(int MemoryAddress, double Value)
        {
            IntPtr ptr;
            byte[] bytes = BitConverter.GetBytes(Value);
            MemBase.WriteProcessMemory(OpenedProcessHandle, (IntPtr)MemoryAddress, bytes, sizeof(double), out ptr);
            return (ptr.ToInt32() == sizeof(double));
        }
        public bool WriteDouble(int MemoryAddress, int[] Offsets, double Value)
        {
            IntPtr ptr;
            int newAddress = CalculatePointer(MemoryAddress, Offsets);
            byte[] bytes = BitConverter.GetBytes(Value);
            MemBase.WriteProcessMemory(OpenedProcessHandle, (IntPtr)newAddress, bytes, sizeof(double), out ptr);
            return (ptr.ToInt32() == sizeof(double));
        }

        public enum TextEncoding
        {
            UTF8,
            UNICODE,
            ASCII
        }
    }
}
