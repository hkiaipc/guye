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
    public partial class branch : Form
    {
        public branch()
        {
            InitializeComponent();
        }

        private int distype=0;

        private void branch_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            
            label12.Text = SocketServer.main.outtemp.dtlast.ToString() ;
            label10.Text = SocketServer.main.outtemp.value.ToString() + " ℃";

            if (SocketServer.main.outtemp.value == 0)
            {
                label10.Text = "--";
            }

            if (SocketServer.main.outtemp.dtlast.ToString()=="0001-1-1 0:00:00")
            {
                label12.Text = "--";
            }
            
            switch (distype)
            {
                case 0: //显示一次供回压
                    labst1g.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._oneGivePress.ToString() + " MPa";
                    labst2g.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._oneGivePress.ToString() + " MPa";
                    labst3g.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._oneGivePress.ToString() + " MPa";
                    labst4g.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._oneGivePress.ToString() + " MPa";
                    labst5g.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._oneGivePress.ToString() + " MPa";
                    labst6g.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._oneGivePress.ToString() + " MPa";
                    labst7g.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._oneGivePress.ToString() + " MPa";
                //    labst8g.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._oneGivePress.ToString() + " MPa";
                    labst9g.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._oneGivePress.ToString() + " MPa";
                    labst10g.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._oneGivePress.ToString() + " MPa";
                    labst11g.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._oneGivePress.ToString() + " MPa";

                    labst1b.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._oneBackPress.ToString() + " MPa";
                    labst2b.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._oneBackPress.ToString() + " MPa";
                    labst3b.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._oneBackPress.ToString() + " MPa";
                    labst4b.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._oneBackPress.ToString() + " MPa";
                    labst5b.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._oneBackPress.ToString() + " MPa";
                    labst6b.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._oneBackPress.ToString() + " MPa";
                    labst7b.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._oneBackPress.ToString() + " MPa";
                    //labst8b.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._oneBackPress.ToString() + " MPa";
                    labst9b.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._oneBackPress.ToString() + " MPa";
                    labst10b.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._oneBackPress.ToString() + " MPa";
                    labst11b.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._oneBackPress.ToString() + " MPa";
                break;
                case 1: //显示一次供回温
                    labst1g.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._oneGiveTemp.ToString() + " ℃";
                    labst2g.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._oneGiveTemp.ToString() + " ℃";
                    labst3g.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._oneGiveTemp.ToString() + " ℃";
                    labst4g.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._oneGiveTemp.ToString() + " ℃";
                    labst5g.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._oneGiveTemp.ToString() + " ℃";
                    labst6g.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._oneGiveTemp.ToString() + " ℃";
                    labst7g.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._oneGiveTemp.ToString() + " ℃";
                    //labst8g.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._oneGiveTemp.ToString() + " ℃";
                    labst9g.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._oneGiveTemp.ToString() + " ℃";
                    labst10g.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._oneGiveTemp.ToString() + " ℃";
                    labst11g.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._oneGiveTemp.ToString() + " ℃";

                    labst1b.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._oneBackTemp.ToString() + " ℃";
                    labst2b.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._oneBackTemp.ToString() + " ℃";
                    labst3b.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._oneBackTemp.ToString() + " ℃";
                    labst4b.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._oneBackTemp.ToString() + " ℃";
                    labst5b.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._oneBackTemp.ToString() + " ℃";
                    labst6b.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._oneBackTemp.ToString() + " ℃";
                    labst7b.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._oneBackTemp.ToString() + " ℃";
                    //labst8b.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._oneBackTemp.ToString() + " ℃";
                    labst9b.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._oneBackTemp.ToString() + " ℃";
                    labst10b.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._oneBackTemp.ToString() + " ℃";
                    labst11b.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._oneBackTemp.ToString() + " ℃";
                break;
                case 2: //一次流量
                    labst1g.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._oneInstant.ToString() + " m3/h";
                    labst2g.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._oneInstant.ToString() + " m3/h";
                    labst3g.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._oneInstant.ToString() + " m3/h";
                    labst4g.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._oneInstant.ToString() + " m3/h";
                    labst5g.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._oneInstant.ToString() + " m3/h";
                    labst6g.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._oneInstant.ToString() + " m3/h";
                    labst7g.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._oneInstant.ToString() + " m3/h";
                    //labst8g.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._oneInstant.ToString() + " m3/h";
                    labst9g.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._oneInstant.ToString() + " m3/h";
                    labst10g.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._oneInstant.ToString() + " m3/h";
                    labst11g.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._oneInstant.ToString() + " m3/h";

                    labst1b.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._oneAccum.ToString() + " m3";
                    labst2b.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._oneAccum.ToString() + " m3";
                    labst3b.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._oneAccum.ToString() + " m3";
                    labst4b.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._oneAccum.ToString() + " m3";
                    labst5b.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._oneAccum.ToString() + " m3";
                    labst6b.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._oneAccum.ToString() + " m3";
                    labst7b.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._oneAccum.ToString() + " m3";
                    //labst8b.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._oneAccum.ToString() + " m3";
                    labst9b.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._oneAccum.ToString() + " m3";
                    labst10b.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._oneAccum.ToString() + " m3";
                    labst11b.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._oneAccum.ToString() + " m3";
                break;
                case 3: //一次热量
                    labst1g.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._oneHeat.ToString() + " GJ/h";
                    labst2g.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._oneHeat.ToString() + " GJ/h";
                    labst3g.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._oneHeat.ToString() + " GJ/h";
                    labst4g.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._oneHeat.ToString() + " GJ/h";
                    labst5g.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._oneHeat.ToString() + " GJ/h";
                    labst6g.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._oneHeat.ToString() + " GJ/h";
                    labst7g.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._oneHeat.ToString() + " GJ/h";
                    //labst8g.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._oneHeat.ToString() + " GJ/h";
                    labst9g.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._oneHeat.ToString() + " GJ/h";
                    labst10g.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._oneHeat.ToString() + " GJ/h";
                    labst11g.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._oneHeat.ToString() + " GJ/h";

                    labst1b.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._oneAddHeat.ToString() + " GJ";
                    labst2b.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._oneAddHeat.ToString() + " GJ";
                    labst3b.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._oneAddHeat.ToString() + " GJ";
                    labst4b.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._oneAddHeat.ToString() + " GJ";
                    labst5b.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._oneAddHeat.ToString() + " GJ";
                    labst6b.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._oneAddHeat.ToString() + " GJ";
                    labst7b.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._oneAddHeat.ToString() + " GJ";
                    //labst8b.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._oneAddHeat.ToString() + " GJ";
                    labst9b.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._oneAddHeat.ToString() + " GJ";
                    labst10b.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._oneAddHeat.ToString() + " GJ";
                    labst11b.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._oneAddHeat.ToString() + " GJ";
                break;
                case 4: //显示二次供回压
                    labst1g.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._twoGivePress.ToString() + " MPa";
                    labst2g.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._twoGivePress.ToString() + " MPa";
                    labst3g.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._twoGivePress.ToString() + " MPa";
                    labst4g.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._twoGivePress.ToString() + " MPa";
                    labst5g.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._twoGivePress.ToString() + " MPa";
                    labst6g.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._twoGivePress.ToString() + " MPa";
                    labst7g.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._twoGivePress.ToString() + " MPa";
                    //labst8g.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._twoGivePress.ToString() + " MPa";
                    labst9g.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._twoGivePress.ToString() + " MPa";
                    labst10g.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._twoGivePress.ToString() + " MPa";
                    labst11g.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._twoGivePress.ToString() + " MPa";

                    labst1b.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._twoBackPress.ToString() + " MPa";
                    labst2b.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._twoBackPress.ToString() + " MPa";
                    labst3b.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._twoBackPress.ToString() + " MPa";
                    labst4b.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._twoBackPress.ToString() + " MPa";
                    labst5b.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._twoBackPress.ToString() + " MPa";
                    labst6b.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._twoBackPress.ToString() + " MPa";
                    labst7b.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._twoBackPress.ToString() + " MPa";
                    //labst8b.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._twoBackPress.ToString() + " MPa";
                    labst9b.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._twoBackPress.ToString() + " MPa";
                    labst10b.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._twoBackPress.ToString() + " MPa";
                    labst11b.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._twoBackPress.ToString() + " MPa";
                break;
                case 5: //显示二次供回温
                    labst1g.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._twoGiveTemp.ToString() + " ℃";
                    labst2g.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._twoGiveTemp.ToString() + " ℃";
                    labst3g.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._twoGiveTemp.ToString() + " ℃";
                    labst4g.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._twoGiveTemp.ToString() + " ℃";
                    labst5g.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._twoGiveTemp.ToString() + " ℃";
                    labst6g.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._twoGiveTemp.ToString() + " ℃";
                    labst7g.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._twoGiveTemp.ToString() + " ℃";
                    //labst8g.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._twoGiveTemp.ToString() + " ℃";
                    labst9g.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._twoGiveTemp.ToString() + " ℃";
                    labst10g.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._twoGiveTemp.ToString() + " ℃";
                    labst11g.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._twoGiveTemp.ToString() + " ℃";

                    labst1b.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._twoBackTemp.ToString() + " ℃";
                    labst2b.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._twoBackTemp.ToString() + " ℃";
                    labst3b.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._twoBackTemp.ToString() + " ℃";
                    labst4b.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._twoBackTemp.ToString() + " ℃";
                    labst5b.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._twoBackTemp.ToString() + " ℃";
                    labst6b.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._twoBackTemp.ToString() + " ℃";
                    labst7b.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._twoBackTemp.ToString() + " ℃";
                    //labst8b.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._twoBackTemp.ToString() + " ℃";
                    labst9b.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._twoBackTemp.ToString() + " ℃";
                    labst10b.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._twoBackTemp.ToString() + " ℃";
                    labst11b.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._twoBackTemp.ToString() + " ℃";
                break;                
                case 6: //补水流量
                    labst1g.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._subInstant.ToString() + " m3/h";
                    labst2g.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._subInstant.ToString() + " m3/h";
                    labst3g.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._subInstant.ToString() + " m3/h";
                    labst4g.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._subInstant.ToString() + " m3/h";
                    labst5g.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._subInstant.ToString() + " m3/h";
                    labst6g.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._subInstant.ToString() + " m3/h";
                    labst7g.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._subInstant.ToString() + " m3/h";
                    //labst8g.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._subInstant.ToString() + " m3/h";
                    labst9g.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._subInstant.ToString() + " m3/h";
                    labst10g.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._subInstant.ToString() + " m3/h";
                    labst11g.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._subInstant.ToString() + " m3/h";

                    labst1b.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._subAccum.ToString() + " m3";
                    labst2b.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._subAccum.ToString() + " m3";
                    labst3b.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._subAccum.ToString() + " m3";
                    labst4b.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._subAccum.ToString() + " m3";
                    labst5b.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._subAccum.ToString() + " m3";
                    labst6b.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._subAccum.ToString() + " m3";
                    labst7b.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._subAccum.ToString() + " m3";
                    //labst8b.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._subAccum.ToString() + " m3";
                    labst9b.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._subAccum.ToString() + " m3";
                    labst10b.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._subAccum.ToString() + " m3";
                    labst11b.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._subAccum.ToString() + " m3";
                break;
                case 7: //阀位反馈
                    labst1g.Text = SocketServer.main._XDRZInfoList[0]._nowDatas._openDegree.ToString() + " %";
                    labst2g.Text = SocketServer.main._XDRZInfoList[1]._nowDatas._openDegree.ToString() + " %";
                    labst3g.Text = SocketServer.main._XDRZInfoList[3]._nowDatas._openDegree.ToString() + " %";
                    labst4g.Text = SocketServer.main._XDRZInfoList[6]._nowDatas._openDegree.ToString() + " %";
                    labst5g.Text = SocketServer.main._XDRZInfoList[8]._nowDatas._openDegree.ToString() + " %";
                    labst6g.Text = SocketServer.main._XDRZInfoList[9]._nowDatas._openDegree.ToString() + " %";
                    labst7g.Text = SocketServer.main._XDRZInfoList[10]._nowDatas._openDegree.ToString() + " %";
                    //labst8g.Text = SocketServer.main._XDRZInfoList[7]._nowDatas._openDegree.ToString() + " %";
                    labst9g.Text = SocketServer.main._XDRZInfoList[11]._nowDatas._openDegree.ToString() + " %";
                    labst10g.Text = SocketServer.main._XDRZInfoList[12]._nowDatas._openDegree.ToString() + " %";
                    labst11g.Text = SocketServer.main._XDRZInfoList[13]._nowDatas._openDegree.ToString() + " %";

                    labst1b.Text = "--";
                    labst2b.Text = "--";
                    labst3b.Text = "--";
                    labst4b.Text = "--";
                    labst5b.Text = "--";
                    labst6b.Text = "--";
                    labst7b.Text = "--";
                   // labst8b.Text = "--";
                    labst9b.Text = "--";
                    labst10b.Text = "--";
                    labst11b.Text = "--";
                break;
                default: break;
            }
        }
        //一次供回压
        private void button1_Click(object sender, EventArgs e)
        {
            distype = 0;
        }
        //一次供回温
        private void button2_Click(object sender, EventArgs e)
        {
            distype = 1;
        }
        //一次流量
        private void button5_Click(object sender, EventArgs e)
        {
            distype = 2;
        }
        //一次热量
        private void button6_Click(object sender, EventArgs e)
        {
            distype =3;
        }

        //二次供回压
        private void button3_Click(object sender, EventArgs e)
        {
            distype = 4;
        }
        //二次供回温
        private void button4_Click(object sender, EventArgs e)
        {
            distype = 5;
        }

        //补水流量
        private void button7_Click(object sender, EventArgs e)
        {
            distype = 6;
        }
        //阀位反馈
        private void button8_Click(object sender, EventArgs e)
        {
            distype = 7;
        }


        //关闭
        private void label2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }
        //室外温度按钮
        private void button9_Click(object sender, EventArgs e)
        {
            load_tasklist();
            if (groupBox1.Visible == false)
            {
                groupBox1.Visible = true;
                this.radioButton1.Checked = true;

                string sql = "select StationId,StationName from tb_rzStations order by [StationId]";
                DataTable dt = Tool.DB.getDt(sql);
                comboBox1.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBox1.Items.Add(dt.Rows[i]["StationName"].ToString());
                }
                comboBox1.Text = SocketServer.main.outtemp.station;
                if (SocketServer.main.outtemp.enble == true)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                numericUpDown1.Value = (decimal)SocketServer.main.outtemp.cycle;
                return;
            }
            else
            {
                groupBox1.Visible = false;
            }
            
            
        }
        //获取当前站点的室外温度
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("config.xml");
            //修改基准站点
            xDoc.DocumentElement.ChildNodes[3].Attributes.GetNamedItem("value").Value = comboBox1.Text.Trim();
            SocketServer.main.outtemp.station = comboBox1.Text.Trim();
            xDoc.Save("config.xml");

            for (int i = 0; i < SocketServer.main._XDRZInfoList.Length; i++)
            {
                if (comboBox1.Text == SocketServer.main._XDRZInfoList[i]._station._StationName)
                {
                    label7.Text = SocketServer.main._XDRZInfoList[i]._nowDatas._outsideTemp.ToString()+" ℃";
                    break;
                }
            }        
        }

        //切换室外温度获取模式
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                numericUpDown2.Enabled = false;
                numericUpDown1.Enabled = true;
                comboBox1.Enabled = true;
                checkBox1.Enabled = true;
                button10.Enabled = false;
            }
            else
            {
                numericUpDown2.Enabled = true;
                numericUpDown1.Enabled = false;
                comboBox1.Enabled = false;
                checkBox1.Enabled = false;
                button10.Enabled = true;
            }
        }
        //实时下传
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("config.xml");
            if (checkBox1.Checked == true)
            {
                SocketServer.main.outtemp.enble = true;
                xDoc.DocumentElement.ChildNodes[3].Attributes.GetNamedItem("enble").Value = "true";
                dateTimePicker1.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;
                listBox1.Enabled = false;
                radioButton2.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown1.Enabled = true;
            }
            else
            {
                SocketServer.main.outtemp.enble = false;
                xDoc.DocumentElement.ChildNodes[3].Attributes.GetNamedItem("enble").Value = "false";
                dateTimePicker1.Enabled = true;
                button11.Enabled = true;
                button12.Enabled = true;
                listBox1.Enabled = true;
                radioButton2.Enabled = true;
                numericUpDown2.Enabled = true;
                numericUpDown1.Enabled = false;
            }
            xDoc.Save("config.xml");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("config.xml");
            //修改循环时间
            SocketServer.main.outtemp.cycle = Convert.ToInt16(numericUpDown1.Value);
            xDoc.DocumentElement.ChildNodes[3].Attributes.GetNamedItem("cycle").Value = SocketServer.main.outtemp.cycle.ToString();
            xDoc.Save("config.xml");
        }
        

        //即时下设
        private void button10_Click(object sender, EventArgs e)
        {
            for (int i=0; i < main._XDRZInfoList.Length; i++)
            {
                Tool.XD100n x = new Tool.XD100n(SocketServer.main._XDRZInfoList[i]._station._deviceAddress);
                SocketServer.main._XDRZInfoList[i]._commandonce[16]._cmd = x.Set_outtemp_value(Convert.ToSingle(numericUpDown2.Value));
                SocketServer.main._XDRZInfoList[i]._commandonce[16]._onoff = true;
            }
        }
        //添加任务
        private void button11_Click(object sender, EventArgs e)
        {
            string dt = dateTimePicker1.Value.Hour.ToString().PadLeft(2, '0') + ":" + dateTimePicker1.Value.Minute.ToString().PadLeft(2, '0');
            string val = "";
            if (radioButton1.Checked == true)
            {
                val = "基准点温度";
            }
            else
            {
                val = Convert.ToSingle(numericUpDown2.Value).ToString();
            }
            string sql = "INSERT into tb_outtempdown_task ([dt], [value]) VALUES('" + dt + "', '" + val + "')";
            Tool.DB.runCmd(sql);
            load_tasklist();

        }
        //删除任务
        private void button12_Click(object sender, EventArgs e)
        {       
            if (listBox1.SelectedIndex == -1)
            {
                return;
            }
            string dt = listBox1.SelectedItem.ToString().Substring(0,listBox1.SelectedItem.ToString().IndexOf(' '));
            string sql = "DELETE FROM tb_outtempdown_task where [dt]='" + dt + "'";
            Tool.DB.runCmd(sql);
            load_tasklist();
        }
        //加载任务列表
        private void load_tasklist()
        {
            string sql = "select [dt],[value] from tb_outtempdown_task order by [dt] ";
            DataTable dt = Tool.DB.getDt(sql);
            listBox1.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listBox1.Items.Add(Convert.ToDateTime(dt.Rows[i]["dt"]).Hour.ToString().PadLeft(2, '0') + ":" + Convert.ToDateTime(dt.Rows[i]["dt"]).Minute.ToString().PadLeft(2, '0') + " 下置 " + dt.Rows[i]["value"].ToString());
            }
        }






        


    }
}
