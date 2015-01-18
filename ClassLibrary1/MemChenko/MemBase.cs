using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MemChenko
{
    internal class MemBase
    {
        //Call basic external win functions for mem reading
        [DllImport("kernel32.dll")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] bBuffer, uint size, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] bBuffer, uint size, out IntPtr lpNumberOfBytesWritten);
    }
}
