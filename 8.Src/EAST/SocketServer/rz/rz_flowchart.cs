using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rz
{
    public partial class rz_flowchart : Form
    {
        public static int stationid;
        public static int refsetdisplay;
        private Tool.XD100n x;

        public rz_flowchart()
        {
            InitializeComponent();
        }

        private int listidlast = -1;
        private int listid = -1;
        private int irid = -1;

        private int getlistid(int station_id)
        {
            int id = -1;
            for (int i = 0; i < SocketServer.main._XDRZInfoList.Length; i++)
            {
                if (station_id == SocketServer.main._XDRZInfoList[i]._station._StationId)
                {
                    id = i;
                    break;
                }
            }
            return id;
        }

        private int getirid(int listid)
        {
            int id = -1;
            for (int i = 0; i < SocketServer.main._ISocketRSlist.Length; i++)
            {
                if (SocketServer.main._ISocketRSlist[i]._ip == SocketServer.main._XDRZInfoList[listid]._station._IPAddress)
                {
                    id = i;
                    break;
                }
            }
            return id;
        }

        private void rz_flowchart_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("温度曲线+二次供温");
            comboBox1.Items.Add("温度设定+二次供温");
            comboBox1.Items.Add("调节阀开度给定");
            comboBox2.Items.Clear();
            comboBox2.Items.Add("本地");
            comboBox2.Items.Add("远程");
            listid = getlistid(stationid);
            irid = getirid(listid);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            
            listid = getlistid(stationid);
            irid = getirid(listid);

            if (listidlast != listid)
            {
                listidlast = listid;
                groupBox1.Visible = false;
                groupBox7.Visible = false;
                button20.Visible = false;
                button3.Visible = false;
                textclear();
            }

            x = new Tool.XD100n(SocketServer.main._XDRZInfoList[listid]._station._deviceAddress);
            showstation(listid);
        }

        private void textclear()
        {
            numsv.Value=0;
            txtot1.Value = 0;
            txtgt8.Value = 0;
            txtot8.Value = 0;
            txtgt7.Value = 0;
            txtot7.Value = 0;
            txtgt6.Value = 0;
            txtot6.Value = 0;
            txtgt5.Value = 0;
            txtot5.Value = 0;
            txtgt3.Value = 0;
            txtot3.Value = 0;
            txtgt4.Value = 0;
            txtot4.Value = 0;
            txtgt2.Value = 0;
            txtot2.Value = 0;
            txtgt1.Value = 0;
            txtot.Value = 0;
            txtV12.Value = 0;
            txtV8.Value = 0;
            txtV11.Value = 0;
            txtV7.Value = 0;
            txtV10.Value = 0;
            txtV6.Value = 0;
            txtV9.Value = 0;
            txtV5.Value = 0;
            txtV3.Value = 0;
            txtV4.Value = 0;
            txtV2.Value = 0;
            txtV1.Value = 0;
            txtlimit.Value = 0;
            txtmax.Value = 0;
            txtmin.Value = 0;
            txtwg.Value = 0;
            txtwd.Value = 0;
            txteggw.Value = 0;
            txtygdw.Value = 0;
            txtehg.Value = 0;
            txtehd.Value = 0;
            txtegg.Value = 0;
            txtygd.Value = 0;
            comboBox1.Text = "";
            comboBox2.Text = "";
            checkBox1.Checked = false;
        }

        private void showstation(int listid)
        {         
            if (listid == -1)
            {
                return;
            }
            labname.Text = SocketServer.main._XDRZInfoList[listid]._station._StationName.ToString();
            labdt.Text = SocketServer.main._XDRZInfoList[listid]._nowDatas._dt.ToString();

            labot.Text = "室外温度：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._outsideTemp.ToString() + " ℃";
            labdegree.Text = "阀位反馈：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._openDegree.ToString()+" %";
            labogt.Text = "一次供水温度：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._oneGiveTemp.ToString() + " ℃";
            labogp.Text = "一次供水压力：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._oneGivePress.ToString()+" MPa";
            labobt.Text = "一次回水温度：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._oneBackTemp.ToString() + " ℃";
            labobp.Text = "一次回水压力：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._oneBackPress.ToString() + " MPa";

            labof.Text = "一次瞬时流量：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._oneInstant.ToString() + " m3/h";
            laboaf.Text = "一次累积流量：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._oneAccum.ToString() + " m3";

            laboh.Text = "一次瞬时热量：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._oneHeat.ToString()+" GJ/h";
            laboah.Text = "一次累积热量：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._oneAddHeat.ToString()+" GJ";

            labtgt.Text = "二次供水温度：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._twoGiveTemp.ToString() + " ℃";
            labtgp.Text = "二次供水压力：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._twoGivePress.ToString() + " MPa";
            labtbt.Text = "二次回水温度：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._twoBackTemp.ToString() + " ℃";
            labtbp.Text = "二次回水压力：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._twoBackPress.ToString() + " MPa";
            labtgpb.Text = "二次供压基准：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._twoBackBasePress.ToString() + " MPa";
            labtgtb.Text = "二次供温基准：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._twoGiveBaseTemp.ToString() + " ℃";
            labcb.Text = "二次压差设定：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._twoPressCha.ToString() + " MPa";

            labtf.Text = "二次瞬时流量：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._twoInstant.ToString() + " m3/h";
            labtaf.Text = "二次累积流量：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._twoAccum.ToString() + " m3";

            labth.Text = "二次瞬时热量：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._twoHeat.ToString() + " GJ/h";
            labtah.Text = "二次累积热量：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._twoAddHeat.ToString() + " GJ";

            labsf.Text = "补水瞬时流量：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._subInstant.ToString() + " m3/h";
            labsaf.Text = "补水累积流量：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._subAccum.ToString() + " m3";

            labwl.Text = "水箱水位：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._WatBoxLevel.ToString()+" m";
            labx1.Text = "循环泵1状态：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._pump._cycPump1.ToString();
            labx2.Text = "循环泵2状态：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._pump._cycPump2.ToString();
            labx3.Text = "循环泵3状态：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._pump._cycPump3.ToString();
            labb1.Text = "补水泵1状态：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._pump._recruitPump1.ToString();
            labb2.Text = "补水泵2状态：" + SocketServer.main._XDRZInfoList[listid]._nowDatas._pump._recruitPump2.ToString();

            if (SocketServer.main._XDRZInfoList[listid].state == false)
            {
                labdevs.Text = "设备状态：故障";
            }
            else
            {
                labdevs.Text = "设备状态：正常";
            }

            for (int i=0; i < SocketServer.main._ISocketRSlist.Length; i++)
            {
                if (SocketServer.main._XDRZInfoList[listid]._station._IPAddress == SocketServer.main._ISocketRSlist[i]._ip)
                {
                    if (SocketServer.main._ISocketRSlist[i]._Iscon == false)
                    {
                        labcoms.Text = "通讯状态：未连接";
                        labdevs.Text = "设备状态：未知";
                    }
                    else
                    {
                        labcoms.Text = "通讯状态：已连接";
                    }
                    if (SocketServer.main._ISocketRSlist[i]._Isbusy == true)
                    {
                        labcoms.Text = "通讯状态：正在通讯";
                    }               
                }
            }

            

            if (refsetdisplay == 1)
            {
                refsetdisplay = 0;
                ref_set();
            }

        }

        private void ref_set()
        {
            if (SocketServer.main._XDRZInfoList[listid]._commandonce[0]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[0]._back = false;             
                switch (SocketServer.main._XDRZInfoList[listid]._Set._controltype)
                {
                    case 0: comboBox1.Text = "温度曲线+二次供温"; break;
                    case 1: comboBox1.Text = "温度曲线+二次回温"; break;
                    case 2: comboBox1.Text = "温度曲线+二次供回水温差"; break;
                    case 3: comboBox1.Text = "温度设定+二次供温"; break;
                    case 4: comboBox1.Text = "温度设定+二次回温"; break;
                    case 5: comboBox1.Text = "温度设定+二次供回水温差"; break;
                    case 6: comboBox1.Text = "调节阀开度给定"; break;
                    default: comboBox1.Text = ""; break;
                }
                showmessage_valve("调节阀控制方式读取成功！");
                button10.Enabled = true;
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[1]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[1]._back = false;
                numsv.Text = SocketServer.main._XDRZInfoList[listid]._Set._setvalue.ToString();
                showmessage_valve("调节阀设定值读取成功！");
                button2.Enabled = true;
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[2]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[2]._back = false;
                txtot1.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._ov1.ToString();
                txtot2.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._ov2.ToString();
                txtot3.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._ov3.ToString();
                txtot4.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._ov4.ToString();
                txtot5.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._ov5.ToString();
                txtot6.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._ov6.ToString();
                txtot7.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._ov7.ToString();
                txtot8.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._ov8.ToString();

                txtgt1.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._gv1.ToString();
                txtgt2.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._gv2.ToString();
                txtgt3.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._gv3.ToString();
                txtgt4.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._gv4.ToString();
                txtgt5.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._gv5.ToString();
                txtgt6.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._gv6.ToString();
                txtgt7.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._gv7.ToString();
                txtgt8.Text = SocketServer.main._XDRZInfoList[listid]._Set._line._gv8.ToString();
                showmessage_valve("温度曲线读取成功！");
                button8.Enabled = true;
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[3]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[3]._back = false;
                txtV1.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v1.ToString();
                txtV2.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v2.ToString();
                txtV3.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v3.ToString();
                txtV4.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v4.ToString();
                txtV5.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v5.ToString();
                txtV6.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v6.ToString();
                txtV7.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v7.ToString();
                txtV8.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v8.ToString();
                txtV9.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v9.ToString();
                txtV10.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v10.ToString();
                txtV11.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v11.ToString();
                txtV12.Text = SocketServer.main._XDRZInfoList[listid]._Set._timepace._v12.ToString();
                showmessage_valve("分时调整读取成功！");
                button6.Enabled = true;
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[4]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[4]._back = false;
                txtmax.Text = SocketServer.main._XDRZInfoList[listid]._Set._valvemm._valvemax.ToString();
                txtmin.Text = SocketServer.main._XDRZInfoList[listid]._Set._valvemm._valvemin.ToString();
                showmessage_valve("阀位上下限读取成功！");
                button12.Enabled = true;
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[5]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[5]._back = false;
                if (SocketServer.main._XDRZInfoList[listid]._Set._valvelimit._enable == 1)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                txtlimit.Text = SocketServer.main._XDRZInfoList[listid]._Set._valvelimit._valvelimit.ToString();
                showmessage_valve("流量限定读取成功！");
                button14.Enabled = true;
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[6]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[6]._back = false;
                txtygd.Text = SocketServer.main._XDRZInfoList[listid]._Set._alarm._yicigdiya.ToString();
                txtegg.Text = SocketServer.main._XDRZInfoList[listid]._Set._alarm._erciggaoya.ToString();
                txtehg.Text = SocketServer.main._XDRZInfoList[listid]._Set._alarm._ercihgaoya.ToString();
                txtehd.Text = SocketServer.main._XDRZInfoList[listid]._Set._alarm._ercihdiya.ToString();
                txtygdw.Text = SocketServer.main._XDRZInfoList[listid]._Set._alarm._yicigdiwen.ToString();
                txteggw.Text = SocketServer.main._XDRZInfoList[listid]._Set._alarm._erciggaowen.ToString();
                txtwd.Text = SocketServer.main._XDRZInfoList[listid]._Set._alarm._waterlow.ToString();
                txtwg.Text = SocketServer.main._XDRZInfoList[listid]._Set._alarm._waterhight.ToString();
                showmessage_alarm("报警参数读取成功！");
                button16.Enabled = true;
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[7]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[7]._back = false;
                switch (SocketServer.main._XDRZInfoList[listid]._Set._outtemp._outmode)
                {
                    case 0: comboBox2.Text = "本地"; break;
                    case 1: comboBox2.Text = "远程"; break;
                    default: break;
                }
                txtot.Text = SocketServer.main._XDRZInfoList[listid]._Set._outtemp._outtemp.ToString();
                showmessage_valve("室外温度设置读取成功！");
                button4.Enabled = true;
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[8]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[8]._back = false;
                int type=-1;
                if(comboBox1.Text=="温度曲线+二次供温")
                {
                    type=0;
                }
                if(comboBox1.Text=="温度设定+二次供温")
                {
                    type=3;
                }
                if(comboBox1.Text=="调节阀开度给定")
                {
                    type=6;
                }
                if(type==-1)
                {
                    return;
                }
                SocketServer.main._XDRZInfoList[listid]._Set._controltype=type;
                showmessage_valve("调节阀控制方式设置成功！");
            }
            if (SocketServer.main._XDRZInfoList[listid]._commandonce[9]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[9]._back = false;
                SocketServer.main._XDRZInfoList[listid]._Set._setvalue=Convert.ToSingle(numsv.Value);
                showmessage_valve("调节阀设定值设置成功！");
            }
            if (SocketServer.main._XDRZInfoList[listid]._commandonce[10]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[10]._back = false;
                SocketServer.main._XDRZInfoList[listid]._Set._line._ov1 = Convert.ToInt16(txtot1.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._ov2 = Convert.ToInt16(txtot2.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._ov3 = Convert.ToInt16(txtot3.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._ov4 = Convert.ToInt16(txtot4.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._ov5 = Convert.ToInt16(txtot5.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._ov6 = Convert.ToInt16(txtot6.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._ov7 = Convert.ToInt16(txtot7.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._ov8 = Convert.ToInt16(txtot8.Value);

                SocketServer.main._XDRZInfoList[listid]._Set._line._gv1 = Convert.ToInt16(txtot1.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._gv2 = Convert.ToInt16(txtot2.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._gv3 = Convert.ToInt16(txtot3.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._gv4 = Convert.ToInt16(txtot4.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._gv5 = Convert.ToInt16(txtot5.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._gv6 = Convert.ToInt16(txtot6.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._gv7 = Convert.ToInt16(txtot7.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._line._gv8 = Convert.ToInt16(txtot8.Value);
                showmessage_valve("温度曲线设置成功！");
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[11]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[11]._back = false;
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v1 = Convert.ToSingle(txtV1.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v2 = Convert.ToSingle(txtV2.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v3 = Convert.ToSingle(txtV3.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v4 = Convert.ToSingle(txtV4.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v5 = Convert.ToSingle(txtV5.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v6 = Convert.ToSingle(txtV6.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v7 = Convert.ToSingle(txtV7.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v8 = Convert.ToSingle(txtV8.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v9 = Convert.ToSingle(txtV9.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v10 = Convert.ToSingle(txtV10.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v11 = Convert.ToSingle(txtV11.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._timepace._v12 = Convert.ToSingle(txtV12.Value);
                showmessage_valve("分时调整设置成功！");
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[12]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[12]._back = false;
                SocketServer.main._XDRZInfoList[listid]._Set._valvemm._valvemax = Convert.ToInt16(txtmax.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._valvemm._valvemin = Convert.ToInt16(txtmin.Value);
                showmessage_valve("阀位上下限成功！");
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[13]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[13]._back = false;
                if (checkBox1.Checked == true)
                {
                    SocketServer.main._XDRZInfoList[listid]._Set._valvelimit._enable = 1;
                }
                else
                {
                    SocketServer.main._XDRZInfoList[listid]._Set._valvelimit._enable = 0;
                }
                SocketServer.main._XDRZInfoList[listid]._Set._valvelimit._valvelimit=Convert.ToInt16(txtlimit.Value);
                showmessage_valve("流量限定设置成功！");
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[14]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[14]._back = false;
                SocketServer.main._XDRZInfoList[listid]._Set._alarm._yicigdiya = Convert.ToSingle(txtygd.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._alarm._erciggaoya = Convert.ToSingle(txtegg.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._alarm._ercihgaoya = Convert.ToSingle(txtehg.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._alarm._ercihdiya = Convert.ToSingle(txtehd.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._alarm._yicigdiwen = Convert.ToInt16(txtygdw.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._alarm._erciggaowen = Convert.ToInt16(txtygdw.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._alarm._waterlow = Convert.ToSingle(txtwd.Value);
                SocketServer.main._XDRZInfoList[listid]._Set._alarm._waterhight = Convert.ToSingle(txtwg.Value);
                showmessage_alarm("报警参数设置成功！");
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[15]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[15]._back = false;
                if (comboBox2.Text == "本地")
                {
                    SocketServer.main._XDRZInfoList[listid]._Set._outtemp._outmode = 0;
                }
                if (comboBox2.Text == "远程")
                {
                    SocketServer.main._XDRZInfoList[listid]._Set._outtemp._outmode = 1;
                }
                SocketServer.main._XDRZInfoList[listid]._Set._outtemp._outtemp = Convert.ToSingle(txtot.Value);
                showmessage_valve("室外温度设置成功！");
            }

            if (SocketServer.main._XDRZInfoList[listid]._commandonce[17]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[17]._back = false;
                showmessage_valve("循环泵急停设置成功！");
            }
            if (SocketServer.main._XDRZInfoList[listid]._commandonce[18]._back == true)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[18]._back = false;
                showmessage_valve("补水泵急停设置成功！");
            }
        }

        //热网平衡模式下不能显示 阀位上下限 流量限定 提示信息
        private void showmessage_valve(string str)
        {
            MessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void showmessage_alarm(string str)
        {
            MessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        
        //读取控制形式
        private void button11_Click(object sender, EventArgs e)
        {
            SocketServer.main._XDRZInfoList[listid]._commandonce[0]._cmd = x.Get_controltype();
            SocketServer.main._XDRZInfoList[listid]._commandonce[0]._onoff = true;
        }
        //设置控制形式
        private void button10_Click(object sender, EventArgs e)
        {
            int type=-1;
            if(comboBox1.Text=="温度曲线+二次供温")
            {
                type=0;
            }
            if(comboBox1.Text=="温度设定+二次供温")
            {
                type=3;
            }
            if(comboBox1.Text=="调节阀开度给定")
            {
                type=6;
            }
            if(type==-1)
            {
                return;
            }
            SocketServer.main._XDRZInfoList[listid]._commandonce[8]._cmd = x.Set_controltype(type);
            SocketServer.main._XDRZInfoList[listid]._commandonce[8]._onoff = true;
        }

        //读取设置值
        private void button1_Click(object sender, EventArgs e)
        {
            SocketServer.main._XDRZInfoList[listid]._commandonce[1]._cmd = x.Get_setvalue();
            SocketServer.main._XDRZInfoList[listid]._commandonce[1]._onoff = true;
        }
        //设置设置值
        private void button2_Click(object sender, EventArgs e)
        {
            SocketServer.main._XDRZInfoList[listid]._commandonce[9]._cmd = x.Set_setvalue(Convert.ToSingle(numsv.Value));
            SocketServer.main._XDRZInfoList[listid]._commandonce[9]._onoff = true;
        }

        //读取温度曲线
        private void button9_Click(object sender, EventArgs e)
        {
            SocketServer.main._XDRZInfoList[listid]._commandonce[2]._cmd = x.Get_line();
            SocketServer.main._XDRZInfoList[listid]._commandonce[2]._onoff = true;
        }
        //设置温度曲线
        private void button8_Click(object sender, EventArgs e)
        {
            Tool.Set_line sl = new Tool.Set_line();
            sl._ov1=Convert.ToInt16(txtot1.Text);
            sl._ov2=Convert.ToInt16(txtot2.Text);
            sl._ov3=Convert.ToInt16(txtot3.Text);
            sl._ov4=Convert.ToInt16(txtot4.Text);
            sl._ov5=Convert.ToInt16(txtot5.Text);
            sl._ov6=Convert.ToInt16(txtot6.Text);
            sl._ov7=Convert.ToInt16(txtot7.Text);
            sl._ov8=Convert.ToInt16(txtot8.Text);

            sl._gv1=Convert.ToInt16(txtgt1.Text);
            sl._gv2=Convert.ToInt16(txtgt2.Text);
            sl._gv3=Convert.ToInt16(txtgt3.Text);
            sl._gv4=Convert.ToInt16(txtgt4.Text);
            sl._gv5=Convert.ToInt16(txtgt5.Text);
            sl._gv6=Convert.ToInt16(txtgt6.Text);
            sl._gv7=Convert.ToInt16(txtgt7.Text);
            sl._gv8=Convert.ToInt16(txtgt8.Text);

            SocketServer.main._XDRZInfoList[listid]._commandonce[10]._cmd = x.Set_line(sl);
            SocketServer.main._XDRZInfoList[listid]._commandonce[10]._onoff = true;
        }


        //读取分时调整
        private void button7_Click(object sender, EventArgs e)
        {
            SocketServer.main._XDRZInfoList[listid]._commandonce[3]._cmd = x.Get_timepace();
            SocketServer.main._XDRZInfoList[listid]._commandonce[3]._onoff = true;
        }
        //设置分时调整
        private void button6_Click(object sender, EventArgs e)
        {
            Tool.Set_timepace st = new Tool.Set_timepace();
            st._v1 = Convert.ToSingle(txtV1.Text);
            st._v2 = Convert.ToSingle(txtV2.Text);
            st._v3 = Convert.ToSingle(txtV3.Text);
            st._v4 = Convert.ToSingle(txtV4.Text);
            st._v5 = Convert.ToSingle(txtV5.Text);
            st._v6 = Convert.ToSingle(txtV6.Text);
            st._v7 = Convert.ToSingle(txtV7.Text);
            st._v8 = Convert.ToSingle(txtV8.Text);
            st._v9 = Convert.ToSingle(txtV9.Text);
            st._v10 = Convert.ToSingle(txtV10.Text);
            st._v11 = Convert.ToSingle(txtV11.Text);
            st._v12 = Convert.ToSingle(txtV12.Text);

            SocketServer.main._XDRZInfoList[listid]._commandonce[11]._cmd = x.Set_timepace(st);
            SocketServer.main._XDRZInfoList[listid]._commandonce[11]._onoff = true;
        }

        //读取阀位上下限
        private void button13_Click(object sender, EventArgs e)
        {
            SocketServer.main._XDRZInfoList[listid]._commandonce[4]._cmd = x.Get_valvemm();
            SocketServer.main._XDRZInfoList[listid]._commandonce[4]._onoff = true;
        }
        //设置阀位上下限
        private void button12_Click(object sender, EventArgs e)
        {
            Tool.Set_valvemm sv=new Tool.Set_valvemm();
            sv._valvemax=Convert.ToInt16(txtmax.Text);
            sv._valvemin=Convert.ToInt16(txtmin.Text);        
            SocketServer.main._XDRZInfoList[listid]._commandonce[12]._cmd = x.Set_valvemm(sv);
            SocketServer.main._XDRZInfoList[listid]._commandonce[12]._onoff = true;
        }

        //读取流量限定
        private void button15_Click(object sender, EventArgs e)
        {
            SocketServer.main._XDRZInfoList[listid]._commandonce[5]._cmd = x.Get_valvelimit();
            SocketServer.main._XDRZInfoList[listid]._commandonce[5]._onoff = true;
        }
        //设置流量限定
        private void button14_Click(object sender, EventArgs e)
        {
            Tool.Set_valvelimit svl=new Tool.Set_valvelimit();
            if(checkBox1.Checked==true)
            {
                svl._enable=1;
            }
            else
            {
                svl._enable=0;
            }
            svl._valvelimit=Convert.ToInt16(txtlimit.Text);        
            SocketServer.main._XDRZInfoList[listid]._commandonce[13]._cmd = x.Set_valvelimit(svl);
            SocketServer.main._XDRZInfoList[listid]._commandonce[13]._onoff = true;
        }

        //读取报警
        private void button17_Click(object sender, EventArgs e)
        {
            SocketServer.main._XDRZInfoList[listid]._commandonce[6]._cmd = x.Get_alarm();
            SocketServer.main._XDRZInfoList[listid]._commandonce[6]._onoff = true;
        }
        //设置报警
        private void button16_Click(object sender, EventArgs e)
        {
            Tool.Set_alarm sa = new Tool.Set_alarm();
            sa._yicigdiya = Convert.ToSingle(txtygd.Text);
            sa._erciggaoya = Convert.ToSingle(txtegg.Text);
            sa._ercihgaoya = Convert.ToSingle(txtehg.Text);
            sa._ercihdiya = Convert.ToSingle(txtehd.Text);
            sa._yicigdiwen = Convert.ToInt16(txtygdw.Text);
            sa._erciggaowen = Convert.ToInt16(txteggw.Text);
            sa._waterlow = Convert.ToSingle(txtwd.Text);
            sa._waterhight = Convert.ToSingle(txtwg.Text);

            SocketServer.main._XDRZInfoList[listid]._commandonce[14]._cmd = x.Set_alarm(sa);
            SocketServer.main._XDRZInfoList[listid]._commandonce[14]._onoff = true;
        }

        //读取室外温度
        private void button5_Click(object sender, EventArgs e)
        {
            SocketServer.main._XDRZInfoList[listid]._commandonce[7]._cmd = x.Get_outtemp();
            SocketServer.main._XDRZInfoList[listid]._commandonce[7]._onoff = true;
        }

        //设置室外温度
        private void button4_Click_1(object sender, EventArgs e)
        {
            Tool.Set_outtemp so = new Tool.Set_outtemp();
            so._outmode = -1;
            if (comboBox2.Text == "远程")
            {
                so._outmode = 1;
            }
            if (comboBox2.Text == "本地")
            {
                so._outmode = 0;
            }
            if (so._outmode == -1)
            {
                return;
            }
            so._outtemp = Convert.ToSingle(txtot.Text);
            SocketServer.main._XDRZInfoList[listid]._commandonce[15]._cmd = x.Set_outtemp(so);
            SocketServer.main._XDRZInfoList[listid]._commandonce[15]._onoff = true;
        }
        //报警设置
        private void button18_Click(object sender, EventArgs e)
        {
            if (SocketServer.main._ISocketRSlist[irid]._Iscon == false)
            {
                MessageBox.Show("该站点不在线！不能设置参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //if (SocketServer.main._ISocketRSlist[irid]._Isbusy == true)
            //{
            //    MessageBox.Show("该站点正在通讯！请稍候再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            if (groupBox1.Visible == false)
            {
                groupBox1.Visible = true;
            }
            else
            {
                groupBox1.Visible = false;
            }
            button2.Enabled = false;
            button10.Enabled = false;
            button12.Enabled = false;
            button14.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;
            button8.Enabled = false;
        }
        //调节阀设置
        private void button19_Click(object sender, EventArgs e)
        {
            if (SocketServer.main._ISocketRSlist[irid]._Iscon == false)
            {
                MessageBox.Show("该站点不在线！不能设置参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (groupBox7.Visible == false)
            {
                groupBox7.Visible = true;
            }
            else
            {
                groupBox7.Visible = false;
            }
            button16.Enabled = false;
        }
        //关闭
        private void label25_Click(object sender, EventArgs e)
        {
            groupBox7.Visible = false;
        }
        //关闭
        private void label24_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }
        //循环泵急停
        private void button20_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定急停循环泵？","提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[19]._cmd = x.Set_xunhuan_start();
                SocketServer.main._XDRZInfoList[listid]._commandonce[19]._onoff = true;
                MessageBox.Show("成功添加循环泵急停任务！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //补水泵急停
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定急停补水泵？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SocketServer.main._XDRZInfoList[listid]._commandonce[20]._cmd = x.Set_bushui_start();
                SocketServer.main._XDRZInfoList[listid]._commandonce[20]._onoff = true;
                MessageBox.Show("成功添加补水泵急停任务！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //泵控制
        private void button21_Click(object sender, EventArgs e)
        {
            if (button3.Visible == true || button20.Visible==true)
            {
                button20.Visible = false;
                button3.Visible = false;
                return;
            }
            
            if (SocketServer.main._ISocketRSlist[irid]._Iscon == false)
            {
                MessageBox.Show("该站点不在线！不能设置参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            

            Form f = new SocketServer.message();
            if (f.ShowDialog() == DialogResult.OK)
            {
                groupBox1.Visible = false;
                button20.Visible = true;
                button3.Visible = true;
            }  
        }

    }
}
