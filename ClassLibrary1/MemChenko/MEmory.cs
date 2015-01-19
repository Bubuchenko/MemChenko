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

        public int BaseAddress()
        {
            return OpenedProcess.MainModule.BaseAddress.ToInt32();
        }
        
        public int EntryPoint()
        {
            return OpenedProcess.MainModule.EntryPointAddress.ToInt32();
        }

        public string Name()
        {
            return OpenedProcess.ProcessName;
        }

        public int PID()
        {
            return OpenedProcess.Id;
        }

        public string StartTime()
        {
            return OpenedProcess.StartTime.ToString();
        }


        public int ConvertToDecimal(int iHex)
        {
            return int.Parse(iHex.ToString(), NumberStyles.HexNumber);
        }
        public int ConvertToDecimal(string sHex)
        {
            return int.Parse(sHex, NumberStyles.HexNumber);
        }

        private string CreateAddress(byte[] Bytes)
        {
            string str = "";
            for (int i = 0; i < Bytes.Length; i++)
            {
                if (Convert.ToInt16(Bytes[i]) < 10)
                {
                    str = "0" + Bytes[i].ToString("X") + str;
                }
                else
                {
                    str = Bytes[i].ToString("X") + str;
                }
            }
            return str;
        }

        private int CalculatePointer(int iMemoryAddress, int[] iOffsets)
        {
            int num = iOffsets.Length - 1;
            byte[] bBuffer = new byte[4];
            int num2 = 0;
            if (num == 0)
            {
                num2 = iMemoryAddress;
            }
            for (int i = 0; i <= num; i++)
            {
                IntPtr ptr;
                if (i == num)
                {
                    MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)num2, bBuffer, 4, out ptr);
                    return (ConvertToDecimal(CreateAddress(bBuffer)) + iOffsets[i]);
                }
                if (i == 0)
                {
                    MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)iMemoryAddress, bBuffer, 4, out ptr);
                    num2 = ConvertToDecimal(CreateAddress(bBuffer)) + iOffsets[0];
                }
                else
                {
                    MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)num2, bBuffer, 4, out ptr);
                    num2 = ConvertToDecimal(CreateAddress(bBuffer)) + iOffsets[i];
                }
            }
            return 0;
        }

 


        public string ReadString(int MemoryAddress, uint TextLength, TextEncoding TextEncodingType = TextEncoding.ASCII)
        {
            IntPtr ptr;
            byte[] Buffer = new byte[TextLength];
            MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)MemoryAddress, Buffer, TextLength, out ptr);

            switch(TextEncodingType)
            {
                case TextEncoding.ASCII:
                    return Encoding.ASCII.GetString(Buffer);
                case TextEncoding.UTF8:
                    return Encoding.UTF8.GetString(Buffer);
                case TextEncoding.UNICODE:
                    return Encoding.Unicode.GetString(Buffer);
                default:
                    return Encoding.ASCII.GetString(Buffer);
            }
        }

        public string ReadString(int MemoryAddress, int[] Offsets, uint StringLength, TextEncoding TextEncodingType = TextEncoding.ASCII)
        {
            IntPtr ptr;
            int num = CalculatePointer(MemoryAddress, Offsets);
            byte[] Buffer = new byte[1];
            MemBase.ReadProcessMemory(OpenedProcessHandle, (IntPtr)MemoryAddress, Buffer, StringLength, out ptr);

            switch(TextEncodingType)
            {
                case TextEncoding.ASCII:
                    return Encoding.ASCII.GetString(Buffer);
                case TextEncoding.UTF8:
                    return Encoding.UTF8.GetString(Buffer);
                case TextEncoding.UNICODE:
                    return Encoding.Unicode.GetString(Buffer);
                default:
                    return Encoding.ASCII.GetString(Buffer);
            }
        }

        public enum TextEncoding
        {
            UTF8,
            UNICODE,
            ASCII
        }

        
 

    }
}
