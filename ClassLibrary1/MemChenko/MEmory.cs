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
    }
}
