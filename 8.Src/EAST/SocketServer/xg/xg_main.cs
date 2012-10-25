using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xg
{
    public partial class xg_main : Form
    {
        public xg_main()
        {
            InitializeComponent();
        }

        private void xg_main_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            //实时数据相关
            loadlist();
            //历史数据相关
            dateTimePicker3.Value = dateTimePicker4.Value.AddDays(-1);
            splitContainer1.Panel1MinSize = tabControl1.Width / 2;
        }

        #region real

        //加载实时数据列表
        private void loadlist()
        {
            string sql = "select XgID,XgName from tb_xgstation order by [XgID]";
            DataTable dt = Tool.DB.getDt(sql);
            sql = "SELECT [XgID],[XgName],[DT], [person] FROM [v_xgreallast] ";
            DataTable dt2 = Tool.DB.getDt(sql);

            dt2.Rows.Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt2.Rows.Add();
                dt2.Rows[i]["XgID"] = dt.Rows[i]["XgID"];
                dt2.Rows[i]["XgName"] = dt.Rows[i]["XgName"];
            }

            dt2.Columns.Add("count", typeof(string));

            dataGridView1.DataSource = dt2;
            dataGridView1.Columns["XgID"].Visible = false;

            string[] dgv_columns = { "XgID", "XgName", "DT", "person", "count" };
            string[] dgv_showname = {"站点标识","站点名称","时间","巡更人员","当日巡更次数"};
            int[] dgv_columnswide = {10,100,150,170,100};
            for (int i = 0; i < dgv_columns.Length; i++)
            {
                dataGridView1.Columns[dgv_columns[i]].HeaderText = dgv_showname[i];
                dataGridView1.Columns[dgv_columns[i]].Width = dgv_columnswide[i];
            }

        }
        //定时更新数据
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < SocketServer.main._XDXGInfoList.Length; i++)
            {
                if (SocketServer.main._XDXGInfoList[i]._refDisplay == true)
                {
                    SocketServer.main._XDXGInfoList[i]._refDisplay = false;
                    DataTable dt = dataGridView1.DataSource as DataTable;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (SocketServer.main._XDXGInfoList[i]._XGInfo._XgID == Convert.ToInt32(dt.Rows[j]["XgID"]))
                        {
                            dt.Rows[j]["DT"] = SocketServer.main._XDXGInfoList[i]._XGDataNow._DT;
                            dt.Rows[j]["XgName"] = SocketServer.main._XDXGInfoList[i]._XGInfo._XgName;
                            dt.Rows[j]["person"] = SocketServer.main._XDXGInfoList[i]._XGDataNow._person;
                            string sql = "SELECT count(*) FROM v_xgrealdata  WHERE XgID=" + SocketServer.main._XDXGInfoList[i]._XGInfo._XgID.ToString() + " and [DT]>'" + DateTime.Now.Date + "'";
                            string count = Tool.DB.getStr(sql);
                            dt.Rows[j]["count"] = Convert.ToInt16(count);
                        }
                    }
                }
            }
        }

        //导出
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Tool.Export.saveAs(dataGridView1, "巡更实时数据");
        }

        //校时
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            for (int i = 0; i < SocketServer.main._XDXGInfoList.Length; i++)
            {
                if (SocketServer.main._XDXGInfoList[i]._XGInfo._XgID == Convert.ToInt32(dataGridView1["XgID", dataGridView1.CurrentRow.Index].Value))
                {
                    Tool.xd300 x = new Tool.xd300(SocketServer.main._XDXGInfoList[i]._XGInfo._deviceAddress);
                    SocketServer.main._XDXGInfoList[i]._commandonce[3]._cmd = x.Set_date();
                    SocketServer.main._XDXGInfoList[i]._commandonce[3]._onoff = true;
                    SocketServer.main._XDXGInfoList[i]._commandonce[3]._cmd = x.Set_time();
                    SocketServer.main._XDXGInfoList[i]._commandonce[4]._onoff = true;
                    MessageBox.Show("成功添加校时任务！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            for (int i = 0; i < SocketServer.main._XDXGInfoList.Length; i++)
            {
                if (SocketServer.main._XDXGInfoList[i]._XGInfo._XgID == Convert.ToInt32(dataGridView1["XgID", dataGridView1.CurrentRow.Index].Value))
                {
                    if (SocketServer.main._XDXGInfoList[i]._command[0]._timeoutnow > 0)
                    {
                        MessageBox.Show("正在执行采集命令！请稍候！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    SocketServer.main._XDXGInfoList[0]._command[0]._dt = DateTime.Now.AddMinutes(-SocketServer.main._XDXGInfoList[0]._XGInfo._cycle);
                    MessageBox.Show("成功添加即时采集任务！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
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

        #endregion

        

        #region his
        //导出
        private void button2_Click(object sender, EventArgs e)
        {
            Tool.Export.saveAs(dataGridView2, "巡更历史数据");
        }
        //查询
        private void button1_Click(object sender, EventArgs e)
        {
            his();
        }
        //查询
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            his();
        }
        //查询
        private void his()
        { 
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("请选择某个站点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string his_dgv_sql = "SELECT [XgName],[DT], [person] FROM [v_xgrealdata] where [DT]>='" + dateTimePicker3.Value.ToString() + "' and [DT]<='" + dateTimePicker4.Value.ToString() + "' and XgID= '" + dataGridView1["XgID", dataGridView1.CurrentRow.Index].Value.ToString() + "'";
            DataTable dt = Tool.DB.getDt(his_dgv_sql);
            dataGridView2.DataSource = "";
            dataGridView2.DataSource = dt;

            string[] dgv_columns = { "XgName", "DT", "person"};
            string[] dgv_showname = { "站点名称", "时间", "巡更人员"};
            int[] dgv_columnswide = { 100, 150, 170 };
            for (int i = 0; i < dgv_columns.Length; i++)
            {
                dataGridView2.Columns[dgv_columns[i]].HeaderText = dgv_showname[i];
                dataGridView2.Columns[dgv_columns[i]].Width = dgv_columnswide[i];
            } 
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






    }
}
