using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace SocketServer
{
    public partial class curve : Form
    {
        public curve()
        {
            InitializeComponent();
        }

        public static string unit;

        private string[] dgv_showname = {"一次供温","一次回温","二次供温","二次回温","室外温度",
                                        "二次供温基准","一次供压","一次回压","水箱水位","二次供压","二次回压","二次供压基准",
                                        "一次瞬时流量","一次累积流量","一次瞬时热量","一次累积热量",
                                        "二次瞬时流量","二次累积流量","二次瞬时热量","二次累积热量",
                                        "补水瞬时热量","补水累积热量",
                                        "二次压差设定","调节阀反馈","循环泵1状态","循环泵2状态","循环泵3状态","补水泵1状态","补水泵2状态"};
        private string[] dgv_columns = {"oneGiveTemp", "oneBackTemp", "twoGiveTemp","twoBackTemp", "outsideTemp",
                                       "twoGiveBaseTemp", "oneGivePress", "oneBackPress", "WatBoxLevel", "twoGivePress", "twoBackPress", "twoBackBasePress", 
                                       "oneInstant", "oneAccum", "oneHeat", "oneAddHeat",
                                       "twoInstant", "twoAccum", "twoHeat", "twoAddHeat", 
                                       "subInstant", "subAccum",
                                       "twoPressCha", "openDegree", "pumpState1", "pumpState2", "pumpState3", "addPumpState1", "addPumpState2"};

        private void line_data_Load(object sender, EventArgs e)
        {
            from_ini();
        }

        private void from_ini()
        {
            dtpBegin.Value = dtpEnd.Value.AddDays(-1);
            panel4.BackColor = Color.Blue;
            //曲线类型站点名称
            load_field();
        }
        //曲线类型站点名称
        private void load_field()
        {
            for (int i = 0; i < dgv_showname.Length; i++)
            {
                this.comboBox2.Items.Add(dgv_showname[i]);
            }

            string sql = "select StationId,StationName from tb_rzStations order by [StationId]";
            DataTable dt = Tool.DB.getDt(sql);
            comboBox1.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt.Rows[i]["StationName"].ToString());
                comboBox3.Items.Add(dt.Rows[i]["StationName"].ToString());
                comboBox4.Items.Add(dt.Rows[i]["StationName"].ToString());
            }
            comboBox1.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;

            sql = "select linename from tb_rzline order by lineid";
            dt = Tool.DB.getDt(sql);
            this.checkedListBox1.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                checkedListBox1.Items.Add(dt.Rows[i]["linename"].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool clean = true;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i) == true)
                {
                    string sql = "select [linename], [stationname], [linetype], [linecolor] from [tb_rzline] where [linename]='" + checkedListBox1.Items[i].ToString() + "'";
                    DataTable dt = Tool.DB.getDt(sql);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("没有符合条件的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        drawline(dt.Rows[j]["linename"].ToString() , dt.Rows[j]["stationname"].ToString(), dt.Rows[j]["linetype"].ToString(), dt.Rows[j]["linecolor"].ToString(), clean);
                        clean = false;
                    }
                }
            }
        }

        private void drawline(string linename,string stationname,string linetype,string linecolor, bool clean)
        {
            string sql="";
            for(int i=0;i<dgv_showname.Length;i++)
            {
                if(dgv_showname[i]==linetype)
                {
                    sql = "select " + dgv_columns[i] + " ,dt from v_rzrealdata where StationName ='" + stationname + "' and dt>='" + dtpBegin.Value.ToString() + "' and dt<'" + dtpEnd.Value.ToString() + "' order by dt ";
                }
            }
            DataTable dt = Tool.DB.getDt(sql);

            PointPairList list = new PointPairList();
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DateTime time = (DateTime)dt.Rows[j][1];
                double x = new XDate(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
                double y = Convert.ToDouble(dt.Rows[j][0]);
                list.Add(x, y);
            }

            GraphPane myPane = zedGraphControl1.GraphPane;
            if (clean == true)
            {
                myPane.CurveList.Clear();
            }
            this.zedGraphControl1.ZoomOutAll(myPane);
            myPane.Title.Text = dtpBegin.Value.ToString() + " 至 " + dtpEnd.Value.ToString() + " 曲线";
            myPane.XAxis.Title.Text = "时间(h)";
            myPane.YAxis.Title.Text = "   ";
            myPane.XAxis.Type = AxisType.Date;
            myPane.XAxis.MajorGrid.IsVisible = true;  //珊格子
            myPane.YAxis.MajorGrid.IsVisible = true;

        //    myPane.XAxis.MinorGrid.IsVisible = true;  //珊格子
        //    myPane.YAxis.MinorGrid.IsVisible = true;

            myPane.XAxis.Scale.Format = "MM-dd H:mm";
            LineItem myCurve1 = myPane.AddCurve(linename, list, ColorTranslator.FromHtml(linecolor), SymbolType.None);
            myCurve1.Symbol.Fill = new Fill(ColorTranslator.FromHtml(linecolor));
            myCurve1.Symbol.Size=2.0f;
            myCurve1.Line.Width = 2.0F;
         //   myCurve1.Line.IsSmooth = false;//平滑曲线取消
            myCurve1.Line.IsAntiAlias = true; //抗锯齿
            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            if (checkBox2.Checked == true)
            {
                myPane.YAxis.Scale.Min = (double)numericUpDown2.Value;
                myPane.YAxis.Scale.Max = (double)numericUpDown1.Value;
            }
            else
            {
                myPane.YAxis.Scale.MinAuto = true;
                myPane.YAxis.Scale.MaxAuto = true;
            }

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            // 设置图例的大小和位置
            //       myPane.Legend.Position = LegendPos.Float;
            //       CoordType b = myPane.Legend.Location.CoordinateFrame;
            //       myPane.Legend.Location = new Location(0.15f, 0.15f, CoordType.PaneFraction, AlignH.Right, AlignV.Top);
            //       myPane.Legend.FontSpec.Size = 8f;
            //       myPane.Legend.IsHStack = false;
        }


        #region
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];
            return curve.Label.Text +": "+ pt.Y.ToString() +" "+ unit +" "+ XDate.XLDateToDateTime(pt.X).ToString();
        }
        #endregion
        //打印
        private void button2_Click_1(object sender, EventArgs e)
        {
            zedGraphControl1.DoPrint();
        }
        //添加曲线
        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请选择站点名称！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }
            if (comboBox2.Text == "")
            {
                MessageBox.Show("请选择曲线类型！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }
            string sql = "select [linename] from [tb_rzline] where [linename]='" + (comboBox1.Text+"_"+ comboBox2.Text)+ "'";
            if (Tool.DB.getStr(sql) != null)
            {
                MessageBox.Show("该曲线已存在！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }

            sql = "INSERT INTO [tb_rzline]([linename], [stationname], [linetype], [linecolor])" +
                       "VALUES('" + (comboBox1.Text + "_" + comboBox2.Text) + "','" + comboBox1.Text + "','" + comboBox2.Text + "','" + ColorTranslator.ToHtml(panel4.BackColor) + "')";

            Tool.DB.runCmd(sql);
            checkedListBox1.Items.Add(comboBox1.Text + "_" + comboBox2.Text);
        }
        //删除曲线
        private void button4_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex == -1)
            {
                return;
            }
            string sql = "DELETE FROM [tb_rzline] WHERE linename='" + checkedListBox1.SelectedItem.ToString() + "'";
            Tool.DB.runCmd(sql);
            checkedListBox1.Items.RemoveAt(checkedListBox1.SelectedIndex);
            if (checkedListBox1.Items.Count == 0)
            {
                checkedListBox1.SelectedIndex = -1;
            }
            else
            {
                checkedListBox1.SelectedIndex = 0;
            }
        }
        //颜色
        private void button7_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            panel4.BackColor = colorDialog1.Color;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                try
                {
                    string sql = "select  [linename], [stationname], [linetype], [linecolor] from [tb_rzline] where  [linename]='" + checkedListBox1.SelectedItem.ToString() + "'";
                    DataTable dt = Tool.DB.getDt(sql);
                    panel4.BackColor = ColorTranslator.FromHtml(dt.Rows[0]["linecolor"].ToString());
                    comboBox1.Text = dt.Rows[0]["stationname"].ToString();
                    comboBox2.Text = dt.Rows[0]["linetype"].ToString();
                }
                catch
                { }
            }
        }
        //选择取消某条曲线
        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (checkedListBox1.SelectedIndex == -1)
            {
                return;
            }
            checkBox1.CheckState = CheckState.Indeterminate;
        }
        //全选
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkedListBox1.SelectedIndex = -1;
            if (checkBox1.CheckState == CheckState.Checked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    this.checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                }
            }
            if (checkBox1.CheckState == CheckState.Unchecked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    this.checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                }
            }
        }

        //压力曲线


        private void button9_Click(object sender, EventArgs e)
        {
            drawpressline(comboBox3.Text);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            zedGraphControl2.DoPrint();
        }

        private void drawpressline(string stationname)
        {
            string sql = "select dt,oneGivePress,oneBackPress ,twoGivePress,twoBackPress from v_rzrealdata where StationName ='" + stationname + "' and dt>='" + this.dateTimePicker2.Value.ToString() + "' and dt<'" + dateTimePicker1.Value.ToString() + "' order by dt ";
            DataTable dt = Tool.DB.getDt(sql);

            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();
            PointPairList list4 = new PointPairList();

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DateTime time = (DateTime)dt.Rows[j][0];
                double x = new XDate(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
                double y = Convert.ToDouble(dt.Rows[j][1]);
                list1.Add(x, y);
                y = Convert.ToDouble(dt.Rows[j][2]);
                list2.Add(x, y);
                y = Convert.ToDouble(dt.Rows[j][3]);
                list3.Add(x, y);
                y = Convert.ToDouble(dt.Rows[j][4]);
                list4.Add(x, y);
            }

            GraphPane myPane = zedGraphControl2.GraphPane;
            myPane.CurveList.Clear();
            this.zedGraphControl2.ZoomOutAll(myPane);
            myPane.Title.Text = dtpBegin.Value.ToString() + " 至 " + dtpEnd.Value.ToString() +" "+stationname+ " 压力曲线";
            myPane.XAxis.Title.Text = "时间(h)";
            myPane.YAxis.Title.Text = "压力(MPa)";
            myPane.XAxis.Type = AxisType.Date;
            myPane.XAxis.MajorGrid.IsVisible = true;  //珊格子
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.Scale.Format = "MM-dd H:mm";

            LineItem myCurve1 = myPane.AddCurve("一次供水压力", list1, Color.Red, SymbolType.None);
            myCurve1.Symbol.Size = 2.0f;
            myCurve1.Line.Width = 2.0F;
            myCurve1.Line.IsAntiAlias = true; //抗锯齿
            myCurve1 = myPane.AddCurve("一次供回压力", list2, Color.DeepPink, SymbolType.None);
            myCurve1.Symbol.Size = 2.0f;
            myCurve1.Line.Width = 2.0F;
            myCurve1.Line.IsAntiAlias = true; //抗锯齿
            myCurve1 = myPane.AddCurve("二次供水压力", list3, Color.Blue, SymbolType.None);
            myCurve1.Symbol.Size = 2.0f;
            myCurve1.Line.Width = 2.0F;
            myCurve1.Line.IsAntiAlias = true; //抗锯齿
            myCurve1 = myPane.AddCurve("二次回水压力", list4, Color.BlueViolet, SymbolType.None);
            myCurve1.Symbol.Size = 2.0f;
            myCurve1.Line.Width = 2.0F;
            myCurve1.Line.IsAntiAlias = true; //抗锯齿

            zedGraphControl2.IsShowPointValues = true;
            zedGraphControl2.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

            myPane.YAxis.Scale.Min = 0.0;
            myPane.YAxis.Scale.MaxAuto = true;

            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();
        }

        //温度曲线
        private void button5_Click(object sender, EventArgs e)
        {
            drawtempline(comboBox4.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            zedGraphControl3.DoPrint();
        }

        private void drawtempline(string stationname)
        {
            string sql = "select dt,oneGiveTemp,oneBackTemp ,twoGiveTemp,twoBackTemp from v_rzrealdata where StationName ='" + stationname + "' and dt>='" + this.dateTimePicker6.Value.ToString() + "' and dt<'" + dateTimePicker5.Value.ToString() + "' order by dt ";
            DataTable dt = Tool.DB.getDt(sql);

            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();
            PointPairList list4 = new PointPairList();

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DateTime time = (DateTime)dt.Rows[j][0];
                double x = new XDate(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
                double y = Convert.ToDouble(dt.Rows[j][1]);
                list1.Add(x, y);
                y = Convert.ToDouble(dt.Rows[j][2]);
                list2.Add(x, y);
                y = Convert.ToDouble(dt.Rows[j][3]);
                list3.Add(x, y);
                y = Convert.ToDouble(dt.Rows[j][4]);
                list4.Add(x, y);
            }

            GraphPane myPane = zedGraphControl3.GraphPane;
            myPane.CurveList.Clear();
            this.zedGraphControl3.ZoomOutAll(myPane);
            myPane.Title.Text = dtpBegin.Value.ToString() + " 至 " + dtpEnd.Value.ToString() + " " + stationname + " 温度曲线";
            myPane.XAxis.Title.Text = "时间(h)";
            myPane.YAxis.Title.Text = "温度(℃)";
            myPane.XAxis.Type = AxisType.Date;
            myPane.XAxis.MajorGrid.IsVisible = true;  //珊格子
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.Scale.Format = "MM-dd H:mm";

            LineItem myCurve1 = myPane.AddCurve("一次供水温度", list1, Color.Red, SymbolType.None);
            myCurve1.Symbol.Size = 2.0f;
            myCurve1.Line.Width = 2.0F;
            myCurve1.Line.IsAntiAlias = true; //抗锯齿
            myCurve1 = myPane.AddCurve("一次供回温度", list2, Color.DeepPink, SymbolType.None);
            myCurve1.Symbol.Size = 2.0f;
            myCurve1.Line.Width = 2.0F;
            myCurve1.Line.IsAntiAlias = true; //抗锯齿
            myCurve1 = myPane.AddCurve("二次供水温度", list3, Color.Blue, SymbolType.None);
            myCurve1.Symbol.Size = 2.0f;
            myCurve1.Line.Width = 2.0F;
            myCurve1.Line.IsAntiAlias = true; //抗锯齿
            myCurve1 = myPane.AddCurve("二次回水温度", list4, Color.BlueViolet, SymbolType.None);
            myCurve1.Symbol.Size = 2.0f;
            myCurve1.Line.Width = 2.0F;
            myCurve1.Line.IsAntiAlias = true; //抗锯齿

            zedGraphControl3.IsShowPointValues = true;
            zedGraphControl3.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

            myPane.YAxis.Scale.Min = 0.0;
            myPane.YAxis.Scale.MaxAuto = true;

            zedGraphControl3.AxisChange();
            zedGraphControl3.Invalidate();
        }

    }
}
