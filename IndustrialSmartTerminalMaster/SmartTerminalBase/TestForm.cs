using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SmartTerminalBase.FileEditor;
using SmartTerminalBase.Communication;

namespace SmartTerminalBase
{

    public partial class TestForm : Form
    {
        private SerialPort port;
        delegate void UpdateTextEventHandler(byte[] text);
        string a = "";
        UpdateTextEventHandler updateText;
        public TestForm()
        {
            InitializeComponent();
            initport();
            updateText = new UpdateTextEventHandler(UpdateTextBox);
            port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(spReceive_DataReceived);  
        }

        void UpdateTextBox(byte[] text)
        {
            string plainvalue="";
            byte[] b = new byte[text.Length-6];
            Array.Copy(text, 5, b, 0, text.Length-6);
            for (int i = 0; i < text.Length - 6; i = i + 2)
            {
                a=a+((BitConverter.ToInt16(new byte[] {b[i+1], b[i]}, 0)/65525.0d*2*10).ToString("0.0000"))+" ";
            }
            foreach (var item in b)
            {
                plainvalue = plainvalue + item.ToString("X").PadLeft(2, '0') + " ";
            }
            richTextBox1.Text = a + "\n" + plainvalue;
        }
        private void initport()
        {
            port = new SerialPort();
            port.BaudRate = 9600;
            port.PortName = "COM6";
            port.DataBits = 8;
            port.Parity = Parity.None;       
            port.Open();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            a = "";
            byte[] buf = new byte[6];
            buf = HexToByte("ff0400010106");
            port.Write(buf,0,6);
        }
        public void spReceive_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            
            int i = port.BytesToRead;
            Thread.Sleep(50);
            while (i != port.BytesToRead)
            {
                i = port.BytesToRead;
                Thread.Sleep(100);
            }
            Trace.WriteLine(i);
            byte[] readBuffer = new byte[i];
            port.Read(readBuffer, 0,i);         
            this.Invoke(updateText, readBuffer );
            // string readString = port.ReadExisting();
            // Trace.WriteLine(readString);
            // this.Invoke(updateText, new string[] { readString });
        }

        private static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 2)
            {
                textEdit2.Enabled = true;
            }
            else
            {
                textEdit2.Enabled = false;
            }
        }
    }
}
