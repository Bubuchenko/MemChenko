using MemChenko;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemReadPlayground
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Memory mem = new Memory();
            mem.OpenProcess("DarkSoulsII");

            while(true)
            {
                mem.WriteInt32(0x11593F4, new int[3] { 0xcc4, 0xa0, 0x60 }, 99);
            }

        }
    }
}
