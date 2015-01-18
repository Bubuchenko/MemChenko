using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemChenko
{
    public class Memory
    {
        public Process OpenedProcess { get; set; }

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

        public IntPtr ProcessHandle()
        {
            if(OpenedProcess.Handle != IntPtr.Zero)
            {
                return OpenedProcess.Handle;
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        public string ReadString(int MemoryAddress, uint TextLength)
        {
            IntPtr pointer;
            byte[] Buffer = new byte[TextLength];
            MemBase.ReadProcessMemory(ProcessHandle(), (IntPtr)MemoryAddress, Buffer, TextLength, out pointer);

            return Encoding.UTF8.GetString(Buffer);
        }

    }
}
