﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Modbus.Device;
using BaseClass.Communication;
using System.Net.Sockets;

namespace BaseClass
{
    public partial class Form1 : Form
    {
        TianhengPlc_Snap7 Client = new TianhengPlc_Snap7();
        byte[] buffer = new byte[2048];
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {
            TcpClient Client = new TcpClient();
            Client.BeginConnect("213.21.23.1", 234,null,null);
            
            
        
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
