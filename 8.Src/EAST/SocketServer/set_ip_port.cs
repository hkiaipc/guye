using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SocketServer
{
    public partial class set_ip_port : Form
    {
        public set_ip_port()
        {
            InitializeComponent();
        }

        private void set_Load(object sender, EventArgs e)
        {
            txtIP.Text = main.ip;
            numPort.Value = int.Parse(main.port);
            textBox1.Text = main.sql_con;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("config.xml");
                xDoc.DocumentElement.ChildNodes[0].Attributes.GetNamedItem("value").Value = txtIP.Text.Trim();
                main.ip = txtIP.Text.Trim();
                xDoc.DocumentElement.ChildNodes[1].Attributes.GetNamedItem("value").Value = numPort.Value.ToString();
                main.port = numPort.Value.ToString();
                xDoc.DocumentElement.ChildNodes[2].Attributes.GetNamedItem("value").Value = textBox1.Text.Trim();
                main.ip = textBox1.Text.Trim();
                xDoc.Save("config.xml");
                this.Close();
                MessageBox.Show("配置将在软件重新运行后生效！" , "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show("写配置文件出错！" , "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close() ;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

    }
}
