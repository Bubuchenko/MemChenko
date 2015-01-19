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
            mem.OpenProcess("steam");

            string x = mem.ReadString(0x03308350, 20);

            MessageBox.Show(x);
        }
    }
}
