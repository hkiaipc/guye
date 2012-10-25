using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SocketServer
{
    public partial class systemmanage : Form
    {
        public systemmanage()
        {
            InitializeComponent();
        }

        private void systemmanage_Load(object sender, EventArgs e)
        {
            //站点管理相关
            sta_date_ref();
        }

        #region comm
        private void comm_date_ref()
        {
            string comm_dgv_sql = "SELECT [GprsID] as [标识], [DTUregister] as [注册信息], [IPAddress] as [IP地址], [Remark] as [备注] FROM [tb_Gprs]";
            dataGridView2.DataSource = Tool.DB.getDt(comm_dgv_sql);
        }
        //添加
        private void toolStripButton4_Click(object sender, EventArgs e)
        {

        }
        //修改
        private void toolStripButton5_Click(object sender, EventArgs e)
        {

        }
        //删除
        private void toolStripButton6_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region sta
        private void sta_date_ref()
        {
            string sta_dgv_sql = "SELECT [StationId] as [站点标识], [StationName] as [站点名称], [group] as [所属组], [DTUregister] as [DTU标识], [IPAddress] as [IP地址], [deviceAddress] as [设备地址], [heatArea] as [供热面积], [heatbase] as [供热基准], [cycle] as [采集周期], [timeout] as[超时时间], [retrytimes] as [重试次数], [Remark] as [备注] FROM [v_rzstation]";
            dataGridView3.DataSource = Tool.DB.getDt(sta_dgv_sql);
        }
        //添加站点
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            groupBox4.Text = "添加";
            groupBox4.Visible = true;
            this.textstationname.Text = "";
            this.textgorup.Text = "";
            this.comboBox1.Text = "";
            this.textremark.Text = "";
            this.numcycle.Value = 10;
            this.numretry.Value = 3;
            this.numtimeout.Value = 5;
        }
        //修改站点
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            groupBox4.Text = "修改站点";
            groupBox4.Visible = true;
            if (dataGridView3.CurrentRow == null)
            {
                return;
            }
            int row = dataGridView3.CurrentRow.Index;
            this.textstationname.Text = dataGridView3["站点名称", row].Value.ToString();
            this.textgorup.Text = dataGridView3["所属组", row].Value.ToString();
            this.comboBox1.Text = dataGridView3["IP地址", row].Value.ToString();
            this.textremark.Text = dataGridView3["备注", row].Value.ToString();
            this.numcycle.Value = Convert.ToInt32(dataGridView3["采集周期", row].Value);
            this.numretry.Value = Convert.ToInt32(dataGridView3["重试次数", row].Value);
            this.numtimeout.Value = Convert.ToInt32(dataGridView3["超时时间", row].Value);
        }
        //删除站点
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow == null)
            {
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("确定删除站点：" + dataGridView3["站点名称", dataGridView3.CurrentCell.RowIndex].Value.ToString() + "？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                string sql = "DELETE FROM [tb_rzStations] WHERE " + Convert.ToInt32(dataGridView3["站点标识", dataGridView3.CurrentCell.RowIndex].Value) + "";
                Tool.DB.runCmd(sql);
                sta_date_ref();
            }
        }
        //添加修改的取消
        private void button4_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = false;
        }
        //添加修改的确定
        private void button5_Click(object sender, EventArgs e)
        {
            bool checkpass = checkdata();
            if (checkpass == true)
            {
                groupBox4.Visible = false;
                string sql = "";
                if (groupBox4.Text == "添加")
                {
                    sql = "INSERT INTO [tb_rzStations]([StationName], [GprsID], [deviceAddress], [Remark], [group], [cycle], [timeout], [retrytimes])" +
                           "VALUES('" + textstationname.Text + "','"+comboBox1.Text+"',  '" + textremark.Text + "',  '" + textgorup.Text + "', " + numcycle.Value.ToString() + ", " + numtimeout.Value.ToString() + ", " + numretry.Value.ToString() + ")";
                }
                if (groupBox4.Text == "修改")
                {
                    sql = "UPDATE [tb_rzStations] SET [StationName]='" + textstationname.Text + "', [GprsID]= '" + comboBox1.Text + "', [deviceAddress]='" + numericUpDown4.ToString() + "', [Remark]='" + textremark.Text + "', " +
                        " [group]='" + textgorup.Text + "', [cycle]=" + numcycle.Value.ToString() + ", [timeout]=" + numtimeout.Value.ToString() + ", [retrytimes]=" + numretry.Value.ToString() + " WHERE [StationId]=" + Convert.ToInt32(dataGridView3["站点标识", dataGridView3.CurrentCell.RowIndex].Value) + "";
                }
                Tool.DB.runCmd(sql);
                sta_date_ref();
            }
        }
        //数据合法性判断
        private bool checkdata()
        {
            return true;
        }
        #endregion



    }
}
