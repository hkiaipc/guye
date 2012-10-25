using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace rz
{
    public partial class rz_main : Form
    {
        public rz_main()
        {
            InitializeComponent();
        }

        private void rz_main_Load(object sender, EventArgs e)
        {
            //实时数据相关 
            real_load();
            //历史数据相关
            dateTimePicker1.Value = dateTimePicker2.Value.AddDays(-1);
            load_field();
            //报警数据相关
            dateTimePicker5.Value = dateTimePicker6.Value.AddDays(-1);
            load_field_alarm();

            timer1.Interval = 1000;
            timer1.Enabled = true;
        }

        #region real
        //加载站点列表
        private void real_load()
        {
            string sql = "select StationId,StationName from tb_rzStations order by [StationId]";
            DataTable dt = Tool.DB.getDt(sql);
            sql = "SELECT [StationId],[StationName], [DT], [oneGiveTemp], [oneBackTemp], [twoGiveTemp], [twoBackTemp], [outsideTemp], [twoGiveBaseTemp], [oneGivePress], [oneBackPress], [WatBoxLevel], [twoGivePress], [twoBackPress], [twoBackBasePress], [oneInstant], [oneAccum], [oneHeat], [oneAddHeat],[twoInstant], [twoAccum],[twoHeat], [twoAddHeat],[subInstant], [subAccum], [twoPressCha], [openDegree], [pumpState1], [pumpState2], [pumpState3], [addPumpState1], [addPumpState2] FROM [GYDB].[dbo].[v_rzreallast] ";
            DataTable dt2 = Tool.DB.getDt(sql);
            
            dt2.Rows.Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt2.Rows.Add();
                dt2.Rows[i]["StationId"] = dt.Rows[i]["StationId"];
                dt2.Rows[i]["StationName"] = dt.Rows[i]["StationName"];
            }

            dt2.Columns.Add("alarm",typeof(Image));

            dataGridView1.DataSource = dt2;
            dataGridView1.Columns["StationId"].Visible=false;
            dataGridView1.Columns["alarm"].DisplayIndex = 0;

            string[] dgv_columns = {"StationId","alarm","StationName", "DT", "oneGiveTemp", "oneBackTemp", "twoGiveTemp","twoBackTemp", "outsideTemp",
                                       "twoGiveBaseTemp", "oneGivePress", "oneBackPress", "WatBoxLevel", "twoGivePress", "twoBackPress", "twoBackBasePress", 
                                       "oneInstant", "oneAccum", "oneHeat", "oneAddHeat",
                                       "twoInstant", "twoAccum", "twoHeat", "twoAddHeat",
                                       "subInstant", "subAccum",  
                                       "twoPressCha", "openDegree", "pumpState1", "pumpState2", "pumpState3", "addPumpState1", "addPumpState2"};
            string[] dgv_showname = {"站点标识","报警","站点名称","时间","一次供温","一次回温","二次供温","二次回温","室外温度",
                                        "二次供温基准","一次供压","一次回压","水箱水位","二次供压","二次回压","二次供压基准",
                                        "一次瞬时流量","一次累积流量","一次瞬时热量","一次累积热量",
                                        "二次瞬时流量","二次累积流量","二次瞬时热量","二次累积热量",
                                        "补水瞬时流量","补水累积流量",
                                        "二次压差设定","调节阀反馈","循环泵1状态","循环泵2状态","循环泵3状态","补水泵1状态","补水泵2状态"};
            int[] dgv_columnswide = {10,35,100,120,70,70,70,70,70,
                                        80,70,70,70,70,70,80,
                                    80,100,80,100,
                                    80,100,80,100,
                                    80,100,
                                    80,70,80,80,80,80,80};
            for (int i = 0; i < dgv_columns.Length; i++)
            {
                dataGridView1.Columns[dgv_columns[i]].HeaderText = dgv_showname[i];
                dataGridView1.Columns[dgv_columns[i]].Width = dgv_columnswide[i];
            }

            DataGridViewImageColumn imageColumn = (DataGridViewImageColumn)dataGridView1.Columns["alarm"];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.DefaultCellStyle.NullValue = null; 
        }
        //定时更新数据
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < SocketServer.main._XDRZInfoList.Length; i++)
            {
                if (SocketServer.main._XDRZInfoList[i]._refDisplay == true)
                {
                    SocketServer.main._XDRZInfoList[i]._refDisplay = false;
                    DataTable dt = dataGridView1.DataSource as DataTable;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (SocketServer.main._XDRZInfoList[i]._station._StationId == Convert.ToInt32(dt.Rows[j]["StationId"]))
                        {
                             dt.Rows[j]["DT"] = SocketServer.main._XDRZInfoList[i]._nowDatas._dt;
                             dt.Rows[j]["oneGiveTemp"] = SocketServer.main._XDRZInfoList[i]._nowDatas._oneGiveTemp; 
                             dt.Rows[j]["oneBackTemp"] = SocketServer.main._XDRZInfoList[i]._nowDatas._oneBackTemp; 
                             dt.Rows[j]["twoGiveTemp"] = SocketServer.main._XDRZInfoList[i]._nowDatas._twoGiveTemp; 
                             dt.Rows[j]["twoBackTemp"] = SocketServer.main._XDRZInfoList[i]._nowDatas._twoBackTemp; 
                             dt.Rows[j]["outsideTemp"] = SocketServer.main._XDRZInfoList[i]._nowDatas._outsideTemp; 
                             dt.Rows[j]["twoGiveBaseTemp"] = SocketServer.main._XDRZInfoList[i]._nowDatas._twoGiveBaseTemp; 
                             dt.Rows[j]["oneGivePress"] = SocketServer.main._XDRZInfoList[i]._nowDatas._oneGivePress; 
                             dt.Rows[j]["oneBackPress"] = SocketServer.main._XDRZInfoList[i]._nowDatas._oneBackPress; 
                             dt.Rows[j]["WatBoxLevel"] = SocketServer.main._XDRZInfoList[i]._nowDatas._WatBoxLevel; 
                             dt.Rows[j]["twoGivePress"] = SocketServer.main._XDRZInfoList[i]._nowDatas._twoGivePress; 
                             dt.Rows[j]["twoBackBasePress"] = SocketServer.main._XDRZInfoList[i]._nowDatas._twoBackBasePress; 
                             dt.Rows[j]["twoBackPress"] = SocketServer.main._XDRZInfoList[i]._nowDatas._twoBackPress; 
                             dt.Rows[j]["oneInstant"] = SocketServer.main._XDRZInfoList[i]._nowDatas._oneInstant; 
                             dt.Rows[j]["oneAccum"] = SocketServer.main._XDRZInfoList[i]._nowDatas._oneAccum; 
                             dt.Rows[j]["subAccum"] = SocketServer.main._XDRZInfoList[i]._nowDatas._subAccum; 
                             dt.Rows[j]["subInstant"] = SocketServer.main._XDRZInfoList[i]._nowDatas._subInstant; 
                             dt.Rows[j]["oneHeat"] = SocketServer.main._XDRZInfoList[i]._nowDatas._oneHeat; 
                             dt.Rows[j]["oneAddHeat"] = SocketServer.main._XDRZInfoList[i]._nowDatas._oneAddHeat;
                             dt.Rows[j]["twoInstant"] = SocketServer.main._XDRZInfoList[i]._nowDatas._twoInstant;
                             dt.Rows[j]["twoAccum"] = SocketServer.main._XDRZInfoList[i]._nowDatas._twoAccum;
                             dt.Rows[j]["twoHeat"] = SocketServer.main._XDRZInfoList[i]._nowDatas._twoHeat;
                             dt.Rows[j]["twoAddHeat"] = SocketServer.main._XDRZInfoList[i]._nowDatas._twoAddHeat;
                             dt.Rows[j]["openDegree"] = SocketServer.main._XDRZInfoList[i]._nowDatas._openDegree; 
                             dt.Rows[j]["twoPressCha"] = SocketServer.main._XDRZInfoList[i]._nowDatas._twoPressCha; 
                             dt.Rows[j]["pumpState1"] = SocketServer.main._XDRZInfoList[i]._nowDatas._pump._cycPump1; 
                             dt.Rows[j]["pumpState2"] = SocketServer.main._XDRZInfoList[i]._nowDatas._pump._cycPump2; 
                             dt.Rows[j]["pumpState3"] = SocketServer.main._XDRZInfoList[i]._nowDatas._pump._cycPump3; 
                             dt.Rows[j]["addPumpState1"] = SocketServer.main._XDRZInfoList[i]._nowDatas._pump._recruitPump1; 
                             dt.Rows[j]["addPumpState2"] = SocketServer.main._XDRZInfoList[i]._nowDatas._pump._recruitPump2;
                             try
                             {
                                 if (SocketServer.main._XDRZInfoList[i]._nowDatas._alarm._word > 0)
                                 {
                                     dt.Rows[j]["alarm"] = Image.FromFile(Application.StartupPath + "\\报警.png");
                                 }
                                 if (SocketServer.main._XDRZInfoList[i]._nowDatas._alarm._word == 0)
                                 {
                                     dt.Rows[j]["alarm"] = null;
                                 }
                             }
                             catch
                             {
                                 dt.Rows[j]["alarm"] = null;
                             }
                        }                       
                    }
                }
            }
        }

        //刷新详细信息
        private void ShowDatas()
        {           
            if (dataGridView1.CurrentCell == null)
            {
                return;
            }
            int dgrow = dataGridView1.CurrentRow.Index;
            int row = 0;

            int stationid = Convert.ToInt32(dataGridView1["StationId", dgrow].Value);
            DataTable dt = dataGridView1.DataSource as DataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt32(dt.Rows[i]["StationId"]) == stationid)
                {
                    row = i;
                    break;
                }
            }
            groupBox8.Text = "数据明细 " + dt.Rows[row]["DT"].ToString();
            txtOneGT.Text = dt.Rows[row]["oneGiveTemp"].ToString();
            txtOneBT.Text = dt.Rows[row]["oneBackTemp"].ToString();
            txtTwoGT.Text = dt.Rows[row]["twoGiveTemp"].ToString();
            txtTwoBT.Text = dt.Rows[row]["twoBackTemp"].ToString();
            txtOneGP.Text = dt.Rows[row]["oneGivePress"].ToString();
            txtOneBP.Text = dt.Rows[row]["oneBackPress"].ToString();
            txtTwoGP.Text = dt.Rows[row]["twoGivePress"].ToString();
            txtTwoBP.Text = dt.Rows[row]["twoBackPress"].ToString();
            txtOpen.Text = dt.Rows[row]["openDegree"].ToString();
            txtWat.Text = dt.Rows[row]["WatBoxLevel"].ToString();
            txtOneFlux.Text = dt.Rows[row]["oneInstant"].ToString();
            txtOneAddFlux.Text = dt.Rows[row]["oneAccum"].ToString();
            txtHeat.Text = dt.Rows[row]["oneHeat"].ToString();
            txtAddHeat.Text = dt.Rows[row]["oneAddHeat"].ToString();
            txtSubFlux.Text = dt.Rows[row]["subInstant"].ToString();
            txtSubAddFlux.Text = dt.Rows[row]["subAccum"].ToString();
            txtTwoFlux.Text = dt.Rows[row]["twoInstant"].ToString();
            txtTwoAddFlux.Text = dt.Rows[row]["twoAccum"].ToString();
            txtHeat1.Text = dt.Rows[row]["twoHeat"].ToString();
            txtAddHeat1.Text = dt.Rows[row]["twoAddHeat"].ToString();
            txtPump1.Text = dt.Rows[row]["pumpState1"].ToString();
            txtPump2.Text = dt.Rows[row]["pumpState2"].ToString();
            txtPump3.Text = dt.Rows[row]["pumpState3"].ToString();
            txtSubPump1.Text = dt.Rows[row]["addPumpState1"].ToString();
            txtSubPump2.Text = dt.Rows[row]["addPumpState2"].ToString();
            txtTwoGTS.Text = dt.Rows[row]["twoGiveBaseTemp"].ToString();
            txtTwoBPS.Text = dt.Rows[row]["twoBackBasePress"].ToString();
            txtTwoPCha.Text = dt.Rows[row]["twoPressCha"].ToString();
        }
        //导出
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Tool.Export.saveAs(dataGridView1, this.tabPage1.Text);
        }
        //打印
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Tool.PrintDGV.Print_DataGridView(dataGridView1, "实时数据");
        }
        //明细
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == true)
            {
                panel2.Visible = false;
            }
            else
            {
                panel2.Visible = true;
            }
        }
        //明细显示
        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            ShowDatas();
        }
        //显示报警
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell == null)
                {
                    return;
                }
                int listid = -1;
                int stationid = Convert.ToInt32(dataGridView1["StationId", dataGridView1.CurrentCell.RowIndex].Value);
                for (int i = 0; i < SocketServer.main._XDRZInfoList.Length; i++)
                {
                    if (stationid == SocketServer.main._XDRZInfoList[i]._station._StationId)
                    {
                        listid = i;
                        break;
                    }
                }
                if (listid == -1)
                {
                    return;
                }
                if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._word > 0)
                {
                    groupBox10.Visible = true;

                    checkBox2.Checked = false;
                    checkBox4.Checked = false;
                    checkBox5.Checked = false;
                    checkBox7.Checked = false;
                    checkBox8.Checked = false;
                    checkBox9.Checked = false;
                    checkBox10.Checked = false;
                    checkBox11.Checked = false;
                    checkBox12.Checked = false;

                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._diaodian == Tool.GRAlarm.有)
                    {
                        checkBox12.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._yicigongdiya == Tool.GRAlarm.有)
                    {
                        checkBox2.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._ercigonggaoya == Tool.GRAlarm.有)
                    {
                        checkBox4.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._ercihuigaoya == Tool.GRAlarm.有)
                    {
                        checkBox5.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._ercihuidiya == Tool.GRAlarm.有)
                    {
                        checkBox7.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._yicigongdiwen == Tool.GRAlarm.有)
                    {
                        checkBox8.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._ercigonggaowen == Tool.GRAlarm.有)
                    {
                        checkBox9.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._kaiguandi == Tool.GRAlarm.有)
                    {
                        checkBox10.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._shuiweidi == Tool.GRAlarm.有)
                    {
                        checkBox10.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._kaiguangao == Tool.GRAlarm.有)
                    {
                        checkBox11.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._shuiweigao == Tool.GRAlarm.有)
                    {
                        checkBox11.Checked = true;
                    }

                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._xunhuanbeng1 == Tool.GRAlarm.有)
                    {
                        checkBox6.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._xunhuanbeng2 == Tool.GRAlarm.有)
                    {
                        checkBox13.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._xunhuanbeng3 == Tool.GRAlarm.有)
                    {
                        checkBox14.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._bushuibeng1 == Tool.GRAlarm.有)
                    {
                        checkBox15.Checked = true;
                    }
                    if (SocketServer.main._XDRZInfoList[listid]._nowDatas._alarm._bushuibeng2 == Tool.GRAlarm.有)
                    {
                        checkBox16.Checked = true;
                    }
                }
                
            }
            catch
            { }

        }

        //dgv隔行颜色
        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i % 2 == 1)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                }
            }
        }
        //采集
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            for (int i = 0; i < SocketServer.main._XDRZInfoList.Length; i++)
            {
                if (SocketServer.main._XDRZInfoList[i]._station._StationId == Convert.ToInt32(dataGridView1["StationId", dataGridView1.CurrentRow.Index].Value))
                {
                    SocketServer.main._XDRZInfoList[i]._command[0]._dt = DateTime.Now.AddMinutes(-SocketServer.main._XDRZInfoList[i]._station._cycle);
                    SocketServer.main._XDRZInfoList[i]._command[1]._dt = DateTime.Now.AddMinutes(-SocketServer.main._XDRZInfoList[i]._station._cycle);
                    MessageBox.Show("成功添加即时采集任务！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion
        
        #region his
        //加载字段
        private void load_field()
        {
            string[] dgv_showname = {"站点名称","时间","一次供温","一次回温","二次供温","二次回温","室外温度",
                                        "二次供温基准","一次供压","一次回压","水箱水位","二次供压","二次回压","二次供压基准",
                                        "一次瞬时流量","一次累积流量","一次瞬时热量","一次累积热量",
                                        "二次瞬时流量","二次累积流量","二次瞬时热量","二次累积热量",
                                        "补水瞬时流量","补水累积流量",
                                        "二次压差设定","调节阀反馈","循环泵1状态","循环泵2状态","循环泵3状态","补水泵1状态","补水泵2状态"};
            for (int i = 0; i < dgv_showname.Length; i++)
            {
                checkedListBox1.Items.Add(dgv_showname[i]);
            }
            string sql = "select StationId,StationName from tb_rzStations order by [StationId]";
            DataTable dt = Tool.DB.getDt(sql);
            comboBox3.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {              
                comboBox3.Items.Add(dt.Rows[i]["StationName"].ToString());
            }
        }
        //字段全选
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkedListBox1.SelectedIndex = -1;
            if (checkBox1.CheckState == CheckState.Checked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i,true);
                }
            }
            if (checkBox1.CheckState == CheckState.Unchecked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i,false);
                }
            }
        }
        //字段选择
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex == -1)
            {
                return;
            }
            checkBox1.CheckState = CheckState.Indeterminate;
        }
        //刷新显示
        private void his_date_ref()
        {
            if (comboBox3.Text == "")
            {
                MessageBox.Show("请选择某个站点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string his_dgv_sql = "select ";
            string[] dgv_columns = {"StationName", "DT", "oneGiveTemp", "oneBackTemp", "twoGiveTemp","twoBackTemp", "outsideTemp",
                                       "twoGiveBaseTemp", "oneGivePress", "oneBackPress", "WatBoxLevel", "twoGivePress", "twoBackPress", "twoBackBasePress", 
                                       "oneInstant", "oneAccum",  "oneHeat", "oneAddHeat", 
                                       "twoInstant", "twoAccum",  "twoHeat", "twoAddHeat",
                                       "subInstant", "subAccum",
                                       "twoPressCha", "openDegree", "pumpState1", "pumpState2", "pumpState3", "addPumpState1", "addPumpState2"};
            if (checkedListBox1.CheckedItems.Count==0)
            {
                MessageBox.Show("请选择某个字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            for (int i = 0; i < dgv_columns.Length; i++)
            {
                if (checkedListBox1.GetItemChecked(i) == true)
                {
                    his_dgv_sql = his_dgv_sql + dgv_columns[i] + ",";
                }
            }
            his_dgv_sql = his_dgv_sql.TrimEnd(',') + " from v_rzrealdata";
            DataTable dt = Tool.DB.getDt(his_dgv_sql + " where [DT]>='" + dateTimePicker1.Value.ToString() + "' and [DT]<='" + dateTimePicker2.Value.ToString() + "' and StationName= '"+comboBox3.Text+"'");
            dataGridView2.DataSource = "";
            dataGridView2.DataSource = dt;
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                dataGridView2.Columns[i].HeaderText = checkedListBox1.CheckedItems[i].ToString();
            }
        }
        //查询
        private void button1_Click(object sender, EventArgs e)
        {
            his_date_ref();
        }
        //打印表
        private void button2_Click(object sender, EventArgs e)
        {
            Tool.Export.saveAs(dataGridView2, dateTimePicker1.Value.ToString() + "至" + dateTimePicker2.Value.ToString() + "报表");

        }
        //导出表
        private void button3_Click(object sender, EventArgs e)
        {
            string inTitle = "    " + dateTimePicker1.Value.ToString() + "至" + dateTimePicker2.Value.ToString() + "报表";
            Tool.PrintDGV.Print_DataGridView(dataGridView2, inTitle);

        }
        //dgv隔行颜色
        private void dataGridView2_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (i % 2 == 1)
                {
                    dataGridView2.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                }
            }
        }
        #endregion

        #region arlm
        //加载字段
        private void load_field_alarm()
        {
            string[] dgv_showname = {"站点名称","时间","掉电","水位开关低","水位开关高","水位模拟高","水位模拟低","二次供温高","一次供温低","二次回压高","二次回压低","一次供压低","补水泵1故障","补水泵2故障","循环泵1故障","循环泵2故障","循环泵3故障"};
            for (int i = 0; i < dgv_showname.Length; i++)
            {
                checkedListBox3.Items.Add(dgv_showname[i]);
            }
            string sql = "select StationId,StationName from tb_rzStations order by [StationId]";
            DataTable dt = Tool.DB.getDt(sql);
            comboBox4.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox4.Items.Add(dt.Rows[i]["StationName"].ToString());
            }
        }
        //字段全选
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkedListBox3.SelectedIndex = -1;
            if (checkBox3.CheckState == CheckState.Checked)
            {
                for (int i = 0; i < checkedListBox3.Items.Count; i++)
                {
                    checkedListBox3.SetItemChecked(i, true);
                }
            }
            if (checkBox3.CheckState == CheckState.Unchecked)
            {
                for (int i = 0; i < checkedListBox3.Items.Count; i++)
                {
                    checkedListBox3.SetItemChecked(i, false);
                }
            }
        }
        //字段选择
        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox3.SelectedIndex == -1)
            {
                return;
            }
            checkBox3.CheckState = CheckState.Indeterminate;
        }
        //刷新显示
        private void alarm_date_ref()
        {
            if (comboBox4.Text == "")
            {
                MessageBox.Show("请选择某个站点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string alarm_dgv_sql = "select ";
            string[] dgv_columns = {"StationName", "DT", "powercut", "watboxdlow", "watboxdhight","watboxalow", "watboxahight", "twoGiveTempH", "oneGiveTempL", "twoBackPressH", "twoBackPressL", "oneGivePressL", "addPump1break", "addPump2break", "Pump1break", "Pump2break", "Pump3break"};
            if (checkedListBox3.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择某个字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            for (int i = 0; i < dgv_columns.Length; i++)
            {
                if (checkedListBox3.GetItemChecked(i) == true)
                {
                    alarm_dgv_sql = alarm_dgv_sql + dgv_columns[i] + ",";
                }
            }
            alarm_dgv_sql = alarm_dgv_sql.TrimEnd(',') + " from v_rzalarmdata";
            DataTable dt = Tool.DB.getDt(alarm_dgv_sql + " where [DT]>='" + dateTimePicker5.Value.ToString() + "' and [DT]<='" + dateTimePicker6.Value.ToString() + "' and StationName= '" + comboBox4.Text + "'");
            dataGridView7.DataSource = "";
            dataGridView7.DataSource = dt;
            for (int i = 0; i < checkedListBox3.CheckedItems.Count; i++)
            {
                dataGridView7.Columns[i].HeaderText = checkedListBox3.CheckedItems[i].ToString();
            }
        }
        //查询
        private void button6_Click(object sender, EventArgs e)
        {
            alarm_date_ref();
        }
        //打印表
        private void button7_Click(object sender, EventArgs e)
        {
            string inTitle = "    " + dateTimePicker5.Value.ToString() + "至" + dateTimePicker6.Value.ToString() + "报表";
            Tool.PrintDGV.Print_DataGridView(dataGridView7, inTitle);
        }
        //导出表
        private void button8_Click(object sender, EventArgs e)
        {
            Tool.Export.saveAs(dataGridView7, dateTimePicker5.Value.ToString() + "至" + dateTimePicker6.Value.ToString() + "报表");
        }
        //dgv隔行颜色
        private void dataGridView7_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            for (int i = 0; i < dataGridView7.Rows.Count; i++)
            {
                if (i % 2 == 1)
                {
                    dataGridView7.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                }
            }
        }
        #endregion
        //关闭报警
        private void label63_Click(object sender, EventArgs e)
        {
            groupBox10.Visible = false;
        }









    }
}
