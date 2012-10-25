using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using Utilities;

namespace btGR
{
    /// <summary>
    /// frmDataNow 的摘要说明。
    /// </summary>
    public class frmDataNow : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components;
        private string s_name;
        private System.Windows.Forms.Label labOneGT;
        private System.Windows.Forms.Label labOneGP;
        private System.Windows.Forms.Label labOneBT;
        private System.Windows.Forms.Label labOneBP;
        private System.Windows.Forms.Label labOpenDegree;
        private System.Windows.Forms.Label labOneInstant;
        private System.Windows.Forms.Label labTwoGT;
        private System.Windows.Forms.Label labTwoGP;
        private System.Windows.Forms.Label labTwoBT;
        private System.Windows.Forms.Label labTwoBP;
        private System.Windows.Forms.Label labOutT;
        private System.Windows.Forms.Label labWatBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.StatusBarPanel xunhua;
        private System.Windows.Forms.StatusBarPanel bushui;
		private System.Windows.Forms.Label labOneHeat;
		private System.Windows.Forms.Label labSubIn;
		private System.Windows.Forms.StatusBarPanel xunhua1;
		private System.Windows.Forms.StatusBarPanel xunhua2;
		private System.Windows.Forms.PictureBox picBox1;
		private System.Windows.Forms.PictureBox picBox2;
		private System.Windows.Forms.StatusBarPanel bushui1;
		private System.Windows.Forms.PictureBox picBox3;
		private System.Windows.Forms.PictureBox picBox4;
		private System.Windows.Forms.PictureBox picBox5;
        private Label label1;
        private string S_TITLE="实时监控";
		

        public frmDataNow(string StationName)
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();
            s_name=StationName;
            //
            // TODO: 在 InitializeComponent 调用后添加任何构造函数代码
            //
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataNow));
            this.labOneGT = new System.Windows.Forms.Label();
            this.labOneGP = new System.Windows.Forms.Label();
            this.labOneBT = new System.Windows.Forms.Label();
            this.labOneBP = new System.Windows.Forms.Label();
            this.labOpenDegree = new System.Windows.Forms.Label();
            this.labOneInstant = new System.Windows.Forms.Label();
            this.labTwoGT = new System.Windows.Forms.Label();
            this.labTwoGP = new System.Windows.Forms.Label();
            this.labTwoBT = new System.Windows.Forms.Label();
            this.labTwoBP = new System.Windows.Forms.Label();
            this.labOutT = new System.Windows.Forms.Label();
            this.labWatBox = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.xunhua = new System.Windows.Forms.StatusBarPanel();
            this.xunhua1 = new System.Windows.Forms.StatusBarPanel();
            this.xunhua2 = new System.Windows.Forms.StatusBarPanel();
            this.bushui = new System.Windows.Forms.StatusBarPanel();
            this.bushui1 = new System.Windows.Forms.StatusBarPanel();
            this.labOneHeat = new System.Windows.Forms.Label();
            this.labSubIn = new System.Windows.Forms.Label();
            this.picBox1 = new System.Windows.Forms.PictureBox();
            this.picBox2 = new System.Windows.Forms.PictureBox();
            this.picBox4 = new System.Windows.Forms.PictureBox();
            this.picBox5 = new System.Windows.Forms.PictureBox();
            this.picBox3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.xunhua)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xunhua1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xunhua2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bushui)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bushui1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // labOneGT
            // 
            this.labOneGT.Location = new System.Drawing.Point(64, 130);
            this.labOneGT.Name = "labOneGT";
            this.labOneGT.Size = new System.Drawing.Size(82, 18);
            this.labOneGT.TabIndex = 0;
            this.labOneGT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOneGP
            // 
            this.labOneGP.Location = new System.Drawing.Point(212, 130);
            this.labOneGP.Name = "labOneGP";
            this.labOneGP.Size = new System.Drawing.Size(82, 18);
            this.labOneGP.TabIndex = 1;
            this.labOneGP.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOneBT
            // 
            this.labOneBT.Location = new System.Drawing.Point(66, 248);
            this.labOneBT.Name = "labOneBT";
            this.labOneBT.Size = new System.Drawing.Size(80, 18);
            this.labOneBT.TabIndex = 2;
            this.labOneBT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOneBP
            // 
            this.labOneBP.Location = new System.Drawing.Point(216, 248);
            this.labOneBP.Name = "labOneBP";
            this.labOneBP.Size = new System.Drawing.Size(78, 18);
            this.labOneBP.TabIndex = 3;
            this.labOneBP.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOpenDegree
            // 
            this.labOpenDegree.Location = new System.Drawing.Point(212, 342);
            this.labOpenDegree.Name = "labOpenDegree";
            this.labOpenDegree.Size = new System.Drawing.Size(82, 18);
            this.labOpenDegree.TabIndex = 4;
            this.labOpenDegree.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOneInstant
            // 
            this.labOneInstant.Location = new System.Drawing.Point(64, 342);
            this.labOneInstant.Name = "labOneInstant";
            this.labOneInstant.Size = new System.Drawing.Size(82, 18);
            this.labOneInstant.TabIndex = 5;
            this.labOneInstant.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labTwoGT
            // 
            this.labTwoGT.Location = new System.Drawing.Point(532, 132);
            this.labTwoGT.Name = "labTwoGT";
            this.labTwoGT.Size = new System.Drawing.Size(80, 18);
            this.labTwoGT.TabIndex = 6;
            this.labTwoGT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labTwoGP
            // 
            this.labTwoGP.Location = new System.Drawing.Point(690, 130);
            this.labTwoGP.Name = "labTwoGP";
            this.labTwoGP.Size = new System.Drawing.Size(82, 18);
            this.labTwoGP.TabIndex = 7;
            this.labTwoGP.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labTwoBT
            // 
            this.labTwoBT.Location = new System.Drawing.Point(536, 258);
            this.labTwoBT.Name = "labTwoBT";
            this.labTwoBT.Size = new System.Drawing.Size(82, 18);
            this.labTwoBT.TabIndex = 8;
            this.labTwoBT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labTwoBP
            // 
            this.labTwoBP.Location = new System.Drawing.Point(692, 258);
            this.labTwoBP.Name = "labTwoBP";
            this.labTwoBP.Size = new System.Drawing.Size(82, 18);
            this.labTwoBP.TabIndex = 9;
            this.labTwoBP.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOutT
            // 
            this.labOutT.Location = new System.Drawing.Point(212, 404);
            this.labOutT.Name = "labOutT";
            this.labOutT.Size = new System.Drawing.Size(82, 18);
            this.labOutT.TabIndex = 10;
            this.labOutT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labWatBox
            // 
            this.labWatBox.Location = new System.Drawing.Point(328, 430);
            this.labWatBox.Name = "labWatBox";
            this.labWatBox.Size = new System.Drawing.Size(86, 18);
            this.labWatBox.TabIndex = 11;
            this.labWatBox.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 468);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.xunhua,
            this.xunhua1,
            this.xunhua2,
            this.bushui,
            this.bushui1});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(796, 22);
            this.statusBar1.TabIndex = 12;
            this.statusBar1.Text = "statusBar1";
            // 
            // xunhua
            // 
            this.xunhua.Name = "xunhua";
            this.xunhua.Text = "1号循环泵状态：";
            this.xunhua.Width = 150;
            // 
            // xunhua1
            // 
            this.xunhua1.Name = "xunhua1";
            this.xunhua1.Text = "2号循环泵状态：";
            this.xunhua1.Width = 150;
            // 
            // xunhua2
            // 
            this.xunhua2.Name = "xunhua2";
            this.xunhua2.Text = "3号循环泵状态：";
            this.xunhua2.Width = 150;
            // 
            // bushui
            // 
            this.bushui.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.bushui.Name = "bushui";
            this.bushui.Text = "1号补水泵状态：";
            this.bushui.Width = 169;
            // 
            // bushui1
            // 
            this.bushui1.Name = "bushui1";
            this.bushui1.Text = "2号补水泵状态：";
            this.bushui1.Width = 160;
            // 
            // labOneHeat
            // 
            this.labOneHeat.Location = new System.Drawing.Point(64, 402);
            this.labOneHeat.Name = "labOneHeat";
            this.labOneHeat.Size = new System.Drawing.Size(82, 20);
            this.labOneHeat.TabIndex = 13;
            this.labOneHeat.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labSubIn
            // 
            this.labSubIn.Location = new System.Drawing.Point(326, 374);
            this.labSubIn.Name = "labSubIn";
            this.labSubIn.Size = new System.Drawing.Size(88, 16);
            this.labSubIn.TabIndex = 14;
            this.labSubIn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // picBox1
            // 
            this.picBox1.Image = ((System.Drawing.Image)(resources.GetObject("picBox1.Image")));
            this.picBox1.Location = new System.Drawing.Point(560, 278);
            this.picBox1.Name = "picBox1";
            this.picBox1.Size = new System.Drawing.Size(34, 36);
            this.picBox1.TabIndex = 15;
            this.picBox1.TabStop = false;
            // 
            // picBox2
            // 
            this.picBox2.Image = ((System.Drawing.Image)(resources.GetObject("picBox2.Image")));
            this.picBox2.Location = new System.Drawing.Point(560, 324);
            this.picBox2.Name = "picBox2";
            this.picBox2.Size = new System.Drawing.Size(34, 38);
            this.picBox2.TabIndex = 16;
            this.picBox2.TabStop = false;
            // 
            // picBox4
            // 
            this.picBox4.Image = ((System.Drawing.Image)(resources.GetObject("picBox4.Image")));
            this.picBox4.Location = new System.Drawing.Point(712, 342);
            this.picBox4.Name = "picBox4";
            this.picBox4.Size = new System.Drawing.Size(32, 30);
            this.picBox4.TabIndex = 18;
            this.picBox4.TabStop = false;
            // 
            // picBox5
            // 
            this.picBox5.Image = ((System.Drawing.Image)(resources.GetObject("picBox5.Image")));
            this.picBox5.Location = new System.Drawing.Point(762, 342);
            this.picBox5.Name = "picBox5";
            this.picBox5.Size = new System.Drawing.Size(34, 30);
            this.picBox5.TabIndex = 19;
            this.picBox5.TabStop = false;
            // 
            // picBox3
            // 
            this.picBox3.Image = ((System.Drawing.Image)(resources.GetObject("picBox3.Image")));
            this.picBox3.Location = new System.Drawing.Point(560, 368);
            this.picBox3.Name = "picBox3";
            this.picBox3.Size = new System.Drawing.Size(32, 36);
            this.picBox3.TabIndex = 17;
            this.picBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(328, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "2010-01-01 00:00:00";
            // 
            // frmDataNow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(796, 490);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picBox5);
            this.Controls.Add(this.picBox4);
            this.Controls.Add(this.picBox3);
            this.Controls.Add(this.picBox2);
            this.Controls.Add(this.picBox1);
            this.Controls.Add(this.labSubIn);
            this.Controls.Add(this.labOneHeat);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.labWatBox);
            this.Controls.Add(this.labOutT);
            this.Controls.Add(this.labTwoBP);
            this.Controls.Add(this.labTwoBT);
            this.Controls.Add(this.labTwoGP);
            this.Controls.Add(this.labTwoGT);
            this.Controls.Add(this.labOneInstant);
            this.Controls.Add(this.labOpenDegree);
            this.Controls.Add(this.labOneBP);
            this.Controls.Add(this.labOneBT);
            this.Controls.Add(this.labOneGP);
            this.Controls.Add(this.labOneGT);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDataNow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDataNow";
            this.Load += new System.EventHandler(this.frmDataNow_Load);
            this.Closed += new System.EventHandler(this.frmDataNow_Closed);
            ((System.ComponentModel.ISupportInitialize)(this.xunhua)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xunhua1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xunhua2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bushui)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bushui1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion

        private void frmDataNow_Load(object sender, System.EventArgs e)
        {
            this.Text=s_name+S_TITLE;
            LoadDatas();
            if(timer1.Enabled==false)
                this.timer1.Enabled=true;
        }
        private void LoadDatas()
        {
            try
            {
                DBcon con=new DBcon();
                SqlCommand cmd=new SqlCommand("SELECT top 1 * FROM v_rzreallast where StationName='"+s_name+"' order by DT desc",con.GetConnection());
                SqlDataReader dr=cmd.ExecuteReader();
                if(!dr.Read())
                {
                    MessageBox.Show("该站点暂无数据！");
                    dr.Close();
                    cmd.Dispose();
                    return;
                }
                dr.Close();
                cmd.Dispose();
                SqlDataAdapter da=new SqlDataAdapter("SELECT top 1 * FROM v_rzreallast where StationName='"+s_name+"' order by DT desc",con.GetConnection());
                DataSet ds=new DataSet();
                da.Fill(ds,"GRValue");
				int aaa=ds.Tables["GRValue"].Rows.Count;
                this.labOneGT.Text = ds.Tables["GRValue"].Rows[0]["oneGiveTemp"].ToString() + " ℃";
                this.labOneGP.Text=ds.Tables["GRValue"].Rows[0]["oneGivePress"].ToString()+" Mpa";
                this.labOneBT.Text = ds.Tables["GRValue"].Rows[0]["oneBackTemp"].ToString() + " ℃";
                this.labOneBP.Text=ds.Tables["GRValue"].Rows[0]["oneBackPress"].ToString()+" Mpa";
                this.labOneInstant.Text=ds.Tables["GRValue"].Rows[0]["oneAccum"].ToString()+" m3";
                this.labOpenDegree.Text=ds.Tables["GRValue"].Rows[0]["openDegree"].ToString()+" %";
                this.labOneHeat.Text = ds.Tables["GRValue"].Rows[0]["OneHeat"].ToString() + " ℃";
                this.labOutT.Text = ds.Tables["GRValue"].Rows[0]["outsideTemp"].ToString() + " ℃";
                this.labTwoBP.Text=ds.Tables["GRValue"].Rows[0]["twoBackPress"].ToString()+" Mpa";
                this.labTwoBT.Text = ds.Tables["GRValue"].Rows[0]["twoBackTemp"].ToString() + " ℃";
                this.labTwoGP.Text=ds.Tables["GRValue"].Rows[0]["twoGivePress"].ToString()+" Mpa";
                this.labTwoGT.Text=ds.Tables["GRValue"].Rows[0]["twoGiveTemp"].ToString()+" ℃";
                this.labWatBox.Text=ds.Tables["GRValue"].Rows[0]["WatBoxLevel"].ToString()+" m";
				this.labSubIn.Text=ds.Tables["GRValue"].Rows[0]["SubInstant"].ToString()+" m3/min";
                this.label1.Text=ds.Tables["GRValue"].Rows[0]["DT"].ToString();
                string dsd=ds.Tables["GRValue"].Rows[0]["addPumpState1"].ToString();
				if(ds.Tables["GRValue"].Rows[0]["pumpState1"].ToString()=="启动")
				  {
					this.xunhua.Text="1号循环泵状态：启动";
					this.picBox1.Visible=false;
				  }	
				else 
				  {
				       this.xunhua.Text="1号循环泵状态：停止";
				      this.picBox1.Visible=true;
				  }				
				if(ds.Tables["GRValue"].Rows[0]["pumpState2"].ToString()=="启动")
				{					
					this.xunhua1.Text="2号循环泵状态：启动";
					this.picBox2.Visible=false;
				}
				else
				{
					this.xunhua1.Text="2号循环泵状态：停止";
				    this.picBox2.Visible=true;
				}

				if(ds.Tables["GRValue"].Rows[0]["pumpState3"].ToString()=="启动")
				{
					this.xunhua2.Text="3号循环泵状态：启动";
					this.picBox3.Visible=false;
				}
				else 
				{
					this.xunhua2.Text="3号循环泵状态：停止";
				    this.picBox3.Visible=true;
				}
				if(ds.Tables["GRValue"].Rows[0]["addPumpState1"].ToString()=="启动")
				{
					this.bushui.Text="1号补水泵状态：启动";
					this.picBox4.Visible=false;
				}
				else
				{
					this.bushui.Text="1号补水泵状态：停止";
				   this.picBox4.Visible=true;
				}
				if(ds.Tables["GRValue"].Rows[0]["addPumpState2"].ToString()=="启动")
				{
					this.bushui1.Text="2号补水泵状态：启动";
					this.picBox5.Visible=false;
				}
				else
				{
					this.bushui1.Text="2号补水泵状态：停止";
				    this.picBox5.Visible=true;
				}
				//				if(ds.Tables["GRValue"].Rows[0]["addPumpState1"].ToString()=="Ture"||ds.Tables["GRValue"].Rows[0]["addPumpState1"].ToString()=="Ture")
                //					this.bushui.Text="补水泵状态：启动";
                //				else
                //					this.bushui.Text="补水泵状态：停止";
                da.Dispose();
            }
            catch(Exception ex)
            {
                //MessageBox.Show("实时数据读取失败!","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                ExceptionHandler.Handle("实时数据读取失败!", ex );
            }
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            LoadDatas();
        }

        private void frmDataNow_Closed(object sender, System.EventArgs e)
        {
            if(timer1.Enabled==true)
                this.timer1.Enabled=false;
        }

    }
}
