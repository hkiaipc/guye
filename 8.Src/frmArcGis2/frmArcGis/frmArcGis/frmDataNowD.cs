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
    /// frmDataNowD 的摘要说明。
    /// </summary>
    public class frmDataNowD : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labWatBox;
        private System.Windows.Forms.Label labOutT;
        private System.Windows.Forms.Label labTwoBP;
        private System.Windows.Forms.Label labTwoBT;
        private System.Windows.Forms.Label labTwoGP;
        private System.Windows.Forms.Label labTwoGT;
        private System.Windows.Forms.Label labOneInstant;
        private System.Windows.Forms.Label labOpenDegree;
        private System.Windows.Forms.Label labOneBP;
        private System.Windows.Forms.Label labOneBT;
        private System.Windows.Forms.Label labOneGP;
        private System.Windows.Forms.Label labOneGT;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.StatusBarPanel xunhua;
        private System.Windows.Forms.StatusBarPanel bushui;
        private System.Windows.Forms.StatusBarPanel xunhua2;

        private string s_name;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.Label labOneHeat;
		private System.Windows.Forms.Label labSubIn;
		private System.Windows.Forms.StatusBarPanel xunhua1;
		private System.Windows.Forms.StatusBarPanel bushui1;
		private System.Windows.Forms.PictureBox picBox1;
		private System.Windows.Forms.PictureBox picBox2;
		private System.Windows.Forms.PictureBox picBox3;
		private System.Windows.Forms.PictureBox picBox4;
		private System.Windows.Forms.PictureBox picBox5;
        private Label label1;
        private string S_TITLE="实时监控";
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StationName"></param>
		public frmDataNowD(string StationName,string StationNo)
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();
            s_name=StationName;
			
			if (StationNo=="2")
			{
			   
			this.radioButton1.Visible=true;
			this.radioButton2.Visible=true;
            this.radioButton3.Visible=false;
			
			}
			else if(StationNo=="3")
				{
				    this.radioButton1.Visible=true;
					this.radioButton2.Visible=true;
					this.radioButton3.Visible=true;
				
				
				}
			this.radioButton1.Text=s_name+"1";
			this.radioButton2.Text=s_name+"2";
			this.radioButton3.Text=s_name+"3";
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataNowD));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labWatBox = new System.Windows.Forms.Label();
            this.labOutT = new System.Windows.Forms.Label();
            this.labTwoBP = new System.Windows.Forms.Label();
            this.labTwoBT = new System.Windows.Forms.Label();
            this.labTwoGP = new System.Windows.Forms.Label();
            this.labTwoGT = new System.Windows.Forms.Label();
            this.labOneInstant = new System.Windows.Forms.Label();
            this.labOpenDegree = new System.Windows.Forms.Label();
            this.labOneBP = new System.Windows.Forms.Label();
            this.labOneBT = new System.Windows.Forms.Label();
            this.labOneGP = new System.Windows.Forms.Label();
            this.labOneGT = new System.Windows.Forms.Label();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.xunhua = new System.Windows.Forms.StatusBarPanel();
            this.xunhua1 = new System.Windows.Forms.StatusBarPanel();
            this.xunhua2 = new System.Windows.Forms.StatusBarPanel();
            this.bushui = new System.Windows.Forms.StatusBarPanel();
            this.bushui1 = new System.Windows.Forms.StatusBarPanel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.labOneHeat = new System.Windows.Forms.Label();
            this.labSubIn = new System.Windows.Forms.Label();
            this.picBox1 = new System.Windows.Forms.PictureBox();
            this.picBox2 = new System.Windows.Forms.PictureBox();
            this.picBox3 = new System.Windows.Forms.PictureBox();
            this.picBox4 = new System.Windows.Forms.PictureBox();
            this.picBox5 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.xunhua)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xunhua1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xunhua2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bushui)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bushui1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labWatBox
            // 
            this.labWatBox.Location = new System.Drawing.Point(330, 430);
            this.labWatBox.Name = "labWatBox";
            this.labWatBox.Size = new System.Drawing.Size(84, 18);
            this.labWatBox.TabIndex = 23;
            this.labWatBox.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOutT
            // 
            this.labOutT.Location = new System.Drawing.Point(214, 404);
            this.labOutT.Name = "labOutT";
            this.labOutT.Size = new System.Drawing.Size(80, 18);
            this.labOutT.TabIndex = 22;
            this.labOutT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labTwoBP
            // 
            this.labTwoBP.Location = new System.Drawing.Point(690, 256);
            this.labTwoBP.Name = "labTwoBP";
            this.labTwoBP.Size = new System.Drawing.Size(84, 18);
            this.labTwoBP.TabIndex = 21;
            this.labTwoBP.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labTwoBT
            // 
            this.labTwoBT.Location = new System.Drawing.Point(538, 258);
            this.labTwoBT.Name = "labTwoBT";
            this.labTwoBT.Size = new System.Drawing.Size(82, 18);
            this.labTwoBT.TabIndex = 20;
            this.labTwoBT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labTwoGP
            // 
            this.labTwoGP.Location = new System.Drawing.Point(692, 130);
            this.labTwoGP.Name = "labTwoGP";
            this.labTwoGP.Size = new System.Drawing.Size(82, 18);
            this.labTwoGP.TabIndex = 19;
            this.labTwoGP.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labTwoGT
            // 
            this.labTwoGT.Location = new System.Drawing.Point(532, 132);
            this.labTwoGT.Name = "labTwoGT";
            this.labTwoGT.Size = new System.Drawing.Size(82, 18);
            this.labTwoGT.TabIndex = 18;
            this.labTwoGT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOneInstant
            // 
            this.labOneInstant.Location = new System.Drawing.Point(66, 342);
            this.labOneInstant.Name = "labOneInstant";
            this.labOneInstant.Size = new System.Drawing.Size(82, 18);
            this.labOneInstant.TabIndex = 17;
            this.labOneInstant.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOpenDegree
            // 
            this.labOpenDegree.Location = new System.Drawing.Point(214, 344);
            this.labOpenDegree.Name = "labOpenDegree";
            this.labOpenDegree.Size = new System.Drawing.Size(82, 18);
            this.labOpenDegree.TabIndex = 16;
            this.labOpenDegree.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOneBP
            // 
            this.labOneBP.Location = new System.Drawing.Point(212, 248);
            this.labOneBP.Name = "labOneBP";
            this.labOneBP.Size = new System.Drawing.Size(84, 18);
            this.labOneBP.TabIndex = 15;
            this.labOneBP.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOneBT
            // 
            this.labOneBT.Location = new System.Drawing.Point(66, 246);
            this.labOneBT.Name = "labOneBT";
            this.labOneBT.Size = new System.Drawing.Size(78, 18);
            this.labOneBT.TabIndex = 14;
            this.labOneBT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOneGP
            // 
            this.labOneGP.Location = new System.Drawing.Point(216, 132);
            this.labOneGP.Name = "labOneGP";
            this.labOneGP.Size = new System.Drawing.Size(78, 18);
            this.labOneGP.TabIndex = 13;
            this.labOneGP.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labOneGT
            // 
            this.labOneGT.Location = new System.Drawing.Point(66, 132);
            this.labOneGT.Name = "labOneGT";
            this.labOneGT.Size = new System.Drawing.Size(80, 18);
            this.labOneGT.TabIndex = 12;
            this.labOneGT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 458);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.xunhua,
            this.xunhua1,
            this.xunhua2,
            this.bushui,
            this.bushui1});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(800, 22);
            this.statusBar1.TabIndex = 24;
            this.statusBar1.Text = "statusBar1";
            // 
            // xunhua
            // 
            this.xunhua.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.xunhua.Name = "xunhua";
            this.xunhua.Text = "1号循环泵状态：";
            this.xunhua.Width = 161;
            // 
            // xunhua1
            // 
            this.xunhua1.Name = "xunhua1";
            this.xunhua1.Text = "2号循环泵状态：";
            this.xunhua1.Width = 150;
            // 
            // xunhua2
            // 
            this.xunhua2.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.xunhua2.Name = "xunhua2";
            this.xunhua2.Text = "3号循环泵状态：";
            this.xunhua2.Width = 161;
            // 
            // bushui
            // 
            this.bushui.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.bushui.Name = "bushui";
            this.bushui.Text = "1号补水泵状态：";
            this.bushui.Width = 161;
            // 
            // bushui1
            // 
            this.bushui1.Name = "bushui1";
            this.bushui1.Text = "2号补水泵状态：";
            this.bushui1.Width = 150;
            // 
            // radioButton1
            // 
            this.radioButton1.BackColor = System.Drawing.Color.Transparent;
            this.radioButton1.Location = new System.Drawing.Point(139, 21);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(114, 18);
            this.radioButton1.TabIndex = 26;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = false;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.BackColor = System.Drawing.Color.Transparent;
            this.radioButton2.Location = new System.Drawing.Point(259, 21);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(116, 18);
            this.radioButton2.TabIndex = 27;
            this.radioButton2.Text = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = false;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.BackColor = System.Drawing.Color.Transparent;
            this.radioButton3.Location = new System.Drawing.Point(370, 21);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(118, 18);
            this.radioButton3.TabIndex = 28;
            this.radioButton3.Text = "radioButton3";
            this.radioButton3.UseVisualStyleBackColor = false;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // labOneHeat
            // 
            this.labOneHeat.Location = new System.Drawing.Point(64, 402);
            this.labOneHeat.Name = "labOneHeat";
            this.labOneHeat.Size = new System.Drawing.Size(84, 20);
            this.labOneHeat.TabIndex = 29;
            this.labOneHeat.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labSubIn
            // 
            this.labSubIn.Location = new System.Drawing.Point(330, 372);
            this.labSubIn.Name = "labSubIn";
            this.labSubIn.Size = new System.Drawing.Size(84, 18);
            this.labSubIn.TabIndex = 30;
            this.labSubIn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // picBox1
            // 
            this.picBox1.Image = ((System.Drawing.Image)(resources.GetObject("picBox1.Image")));
            this.picBox1.Location = new System.Drawing.Point(560, 278);
            this.picBox1.Name = "picBox1";
            this.picBox1.Size = new System.Drawing.Size(34, 36);
            this.picBox1.TabIndex = 31;
            this.picBox1.TabStop = false;
            // 
            // picBox2
            // 
            this.picBox2.Image = ((System.Drawing.Image)(resources.GetObject("picBox2.Image")));
            this.picBox2.Location = new System.Drawing.Point(559, 324);
            this.picBox2.Name = "picBox2";
            this.picBox2.Size = new System.Drawing.Size(34, 36);
            this.picBox2.TabIndex = 32;
            this.picBox2.TabStop = false;
            // 
            // picBox3
            // 
            this.picBox3.Image = ((System.Drawing.Image)(resources.GetObject("picBox3.Image")));
            this.picBox3.Location = new System.Drawing.Point(560, 368);
            this.picBox3.Name = "picBox3";
            this.picBox3.Size = new System.Drawing.Size(32, 36);
            this.picBox3.TabIndex = 33;
            this.picBox3.TabStop = false;
            // 
            // picBox4
            // 
            this.picBox4.Image = ((System.Drawing.Image)(resources.GetObject("picBox4.Image")));
            this.picBox4.Location = new System.Drawing.Point(712, 342);
            this.picBox4.Name = "picBox4";
            this.picBox4.Size = new System.Drawing.Size(30, 30);
            this.picBox4.TabIndex = 34;
            this.picBox4.TabStop = false;
            // 
            // picBox5
            // 
            this.picBox5.Image = ((System.Drawing.Image)(resources.GetObject("picBox5.Image")));
            this.picBox5.Location = new System.Drawing.Point(766, 346);
            this.picBox5.Name = "picBox5";
            this.picBox5.Size = new System.Drawing.Size(30, 30);
            this.picBox5.TabIndex = 35;
            this.picBox5.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(330, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 12);
            this.label1.TabIndex = 36;
            this.label1.Text = "2010-01-01 00:00:00";
            // 
            // frmDataNowD
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picBox5);
            this.Controls.Add(this.picBox4);
            this.Controls.Add(this.picBox3);
            this.Controls.Add(this.picBox2);
            this.Controls.Add(this.picBox1);
            this.Controls.Add(this.labSubIn);
            this.Controls.Add(this.labOneHeat);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
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
            this.Name = "frmDataNowD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDataNowD";
            this.Load += new System.EventHandler(this.frmDataNowD_Load);
            this.Closed += new System.EventHandler(this.frmDataNowD_Closed);
            ((System.ComponentModel.ISupportInitialize)(this.xunhua)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xunhua1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xunhua2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bushui)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bushui1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDataNowD_Load(object sender, System.EventArgs e)
        {
            this.Text=s_name+S_TITLE;
		    // LoadDatas();
			this.radioButton1.Checked=true;
			if(timer1.Enabled==false)
              this.timer1.Enabled=true;
        }


        /// <summary>
        /// 
        /// </summary>
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
                    this.labTwoGT.Text = ds.Tables["GRValue"].Rows[0]["twoGiveTemp"].ToString() + " ℃";
					this.labWatBox.Text=ds.Tables["GRValue"].Rows[0]["WatBoxLevel"].ToString()+" m";
				    this.labSubIn.Text=ds.Tables["GRValue"].Rows[0]["SubInstant"].ToString()+" m3/min";
					this.label1.Text=ds.Tables["GRValue"].Rows[0]["DT"].ToString();
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
				  {  this.bushui1.Text="2号补水泵状态：启动";
				     this.picBox5.Visible=false;
				  }
				else
				  {  
					this.bushui1.Text="2号补水泵状态：停止";
					this.picBox5.Visible=true;
				  }
					da.Dispose();
				}
				catch(Exception ex)
				{
					//MessageBox.Show("实时数据读取失败!","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
					ExceptionHandler.Handle("实时数据读取失败!", ex );
				}
			}
		

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            LoadDatas();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDataNowD_Closed(object sender, System.EventArgs e)
        {
            if(timer1.Enabled==true)
                this.timer1.Enabled=false;
        }


		private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButton1.Checked ==true)
			{
			   s_name=this.radioButton1.Text;
				//MessageBox.Show(s_name);
				LoadDatas();
			 
			}
			
		}
		private void radioButton2_CheckedChanged(object sender,System.EventArgs e)
		{
		 
			if(radioButton2.Checked==true)
			{
				s_name=this.radioButton2.Text;
				//MessageBox.Show(s_name);
				LoadDatas();
			}
		
		}
        
		private void radioButton3_CheckedChanged(object sender, System.EventArgs e)
		{
			if(radioButton3.Checked==true)
			{
				s_name=this.radioButton3.Text ;
				//MessageBox.Show(s_name);
				LoadDatas();
			}	
		}

    }
}
