using Aletta_s_Kitchen.BotRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aletta_s_Kitchen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var tr = new ControlWriter(TextBoxConsole);

            Console.SetOut(tr);
            Console.SetError(tr);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ButtonBotStart_Click(object sender, EventArgs e)
        {
            BotHandler.bot.RunAsync();
            ButtonBotStart.Enabled = false;
        }
    }

    public class ControlWriter : TextWriter
    {
        private TextBox textBox;
        public ControlWriter(TextBox tb)
        {
            this.textBox = tb;
        }

        public override void Write(string value)
        {
            if (this.textBox.InvokeRequired)
            {
                this.textBox.Invoke(new Action<string>(Write), new object[] { value });
            }
            else
            {
                this.textBox.Text += value + Environment.NewLine;
            }
        }
        public override void WriteLine(string value)
        {
            if (this.textBox.InvokeRequired)
            {
                this.textBox.Invoke(new Action<string>(Write), new object[] { value });
            }
            else
            {
                this.textBox.Text += value + Environment.NewLine;
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }
}
