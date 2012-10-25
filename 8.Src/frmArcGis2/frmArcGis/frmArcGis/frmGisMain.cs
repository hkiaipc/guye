using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using Utilities;

namespace btGR
{
    /// <summary>
    /// Form1 的摘要说明。
    /// </summary>
    public class frmGisMain : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components;
        public Info mapLayer=null;
        private DataSet ds=null;
        private System.Windows.Forms.ToolBarButton tBarZoomIn;
        private System.Windows.Forms.ToolBarButton tBarZoomOut;
        private System.Windows.Forms.ToolBarButton tBarFull;
        private System.Windows.Forms.ToolBarButton tBarPan;
        private System.Windows.Forms.ToolBarButton toolBarButton1;
        private System.Windows.Forms.ToolBarButton tBarRe;
        private System.Windows.Forms.ToolBarButton tBarExit;
        private System.Windows.Forms.GroupBox groupBox1;
        
        // 2007.05.31
        //
        //private ContextMenu GisMapMenu;
        //		private MapObjects2.Point STAT=null;

        // 2007.05.31
        //
        //private MenuItem menuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        
        private System.Windows.Forms.ImageList imageList1;
        private MapObjects2.Rectangle zoomRect;
        private int tBarIndex;
        private double sr;
        private PointInfo[] s_Info=null;
        // 2007.05.31
        //
        //private int POSITIONY=65;

        // 2007.05.31
        //
        //private int POSITIONX=12;
      //  private string STATIONNAME="换热站";
        private MapObjects2.Symbol symE=null;
        //		private int GisMapToolsID;
        private string GisMapSName;
        private string GisMapSNo;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Timer timer1;
        private long iScale;
		private System.Windows.Forms.ToolBar tbGis1;
		private AxMapObjects2.AxMap axMap1;
		private AxMapObjects2.AxMap axMap2;
		
        private double SC=0.85;

        /// <summary>
        /// 
        /// </summary>
        public frmGisMain()
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();

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
                if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmGisMain));
			this.tbGis1 = new System.Windows.Forms.ToolBar();
			this.tBarZoomIn = new System.Windows.Forms.ToolBarButton();
			this.tBarZoomOut = new System.Windows.Forms.ToolBarButton();
			this.tBarFull = new System.Windows.Forms.ToolBarButton();
			this.tBarPan = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.tBarRe = new System.Windows.Forms.ToolBarButton();
			this.tBarExit = new System.Windows.Forms.ToolBarButton();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.axMap1 = new AxMapObjects2.AxMap();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.listBox = new System.Windows.Forms.ListBox();
			this.axMap2 = new AxMapObjects2.AxMap();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axMap1)).BeginInit();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axMap2)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbGis1
			// 
			this.tbGis1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.tbGis1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					  this.tBarZoomIn,
																					  this.tBarZoomOut,
																					  this.tBarFull,
																					  this.tBarPan,
																					  this.toolBarButton1,
																					  this.tBarRe,
																					  this.tBarExit});
			this.tbGis1.DropDownArrows = true;
			this.tbGis1.ImageList = this.imageList1;
			this.tbGis1.Location = new System.Drawing.Point(0, 0);
			this.tbGis1.Name = "tbGis1";
			this.tbGis1.ShowToolTips = true;
			this.tbGis1.Size = new System.Drawing.Size(720, 41);
			this.tbGis1.TabIndex = 1;
			this.tbGis1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbGis1_ButtonClick);
			// 
			// tBarZoomIn
			// 
			this.tBarZoomIn.ImageIndex = 0;
			this.tBarZoomIn.Text = "放大";
			// 
			// tBarZoomOut
			// 
			this.tBarZoomOut.ImageIndex = 1;
			this.tBarZoomOut.Text = "缩小";
			// 
			// tBarFull
			// 
			this.tBarFull.ImageIndex = 2;
			this.tBarFull.Text = "全图";
			// 
			// tBarPan
			// 
			this.tBarPan.ImageIndex = 3;
			this.tBarPan.Text = "漫游";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tBarRe
			// 
			this.tBarRe.ImageIndex = 4;
			this.tBarRe.Text = "查询";
			// 
			// tBarExit
			// 
			this.tBarExit.ImageIndex = 5;
			this.tBarExit.Text = "退出";
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.axMap1);
			this.groupBox1.Location = new System.Drawing.Point(8, 44);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(492, 452);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "地图信息";
			// 
			// axMap1
			// 
			this.axMap1.ContainingControl = this;
			this.axMap1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axMap1.Location = new System.Drawing.Point(3, 17);
			this.axMap1.Name = "axMap1";
			this.axMap1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap1.OcxState")));
			this.axMap1.Size = new System.Drawing.Size(486, 432);
			this.axMap1.TabIndex = 0;
			this.axMap1.MouseDownEvent += new AxMapObjects2._DMapEvents_MouseDownEventHandler(this.axMap1_MouseDownEvent);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.listBox);
			this.groupBox3.Location = new System.Drawing.Point(504, 172);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(208, 316);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "站点信息";
			// 
			// listBox
			// 
			this.listBox.BackColor = System.Drawing.Color.White;
			this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.listBox.ItemHeight = 16;
			this.listBox.Location = new System.Drawing.Point(3, 17);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(202, 292);
			this.listBox.Sorted = true;
			this.listBox.TabIndex = 0;
			this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
			// 
			// axMap2
			// 
			this.axMap2.ContainingControl = this;
			this.axMap2.Location = new System.Drawing.Point(8, 24);
			this.axMap2.Name = "axMap2";
			this.axMap2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap2.OcxState")));
			this.axMap2.Size = new System.Drawing.Size(192, 136);
			this.axMap2.TabIndex = 0;
			this.axMap2.AfterLayerDraw += new AxMapObjects2._DMapEvents_AfterLayerDrawEventHandler(this.axMap2_AfterLayerDraw);
			this.axMap2.MouseUpEvent += new AxMapObjects2._DMapEvents_MouseUpEventHandler(this.axMap2_MouseUpEvent);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.axMap2);
			this.groupBox2.Location = new System.Drawing.Point(504, 492);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(208, 4);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "鸟瞰地图";
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.radioButton3);
			this.groupBox4.Controls.Add(this.radioButton2);
			this.groupBox4.Controls.Add(this.radioButton1);
			this.groupBox4.Location = new System.Drawing.Point(504, 44);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(208, 124);
			this.groupBox4.TabIndex = 8;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "用户管理";
			// 
			// radioButton3
			// 
			this.radioButton3.Location = new System.Drawing.Point(24, 84);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(128, 32);
			this.radioButton3.TabIndex = 5;
			this.radioButton3.Text = "  热网监控层";
			this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(24, 56);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(152, 24);
			this.radioButton2.TabIndex = 4;
			this.radioButton2.Text = "  供热管网层";
			this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(24, 24);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(144, 24);
			this.radioButton1.TabIndex = 3;
			this.radioButton1.Text = "  地图层";
			this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
			// 
			// timer1
			// 
			this.timer1.Interval = 4500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// frmGisMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(720, 509);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.tbGis1);
			this.Name = "frmGisMain";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGisMain_MouseDown);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axMap1)).EndInit();
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axMap2)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);

		}
        #endregion

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.Run(new frmGisMain());
        }
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, System.EventArgs e)
        {	
            mapLayer=new Info();
            LoadMapInfo();
            InitializeMainMap();
            InitializeEyeMap();
            InitializeTools();
        }

        //		public delegate void ContextMenuEventHandler(object sender,string name,int type);
        //		public event ContextMenuEventHandler Menu_XDClick;		

        #region InitializeGis
        /// <summary>
        /// load layer info from .\mapLayers.mdb to Info::m_layers
        /// </summary>
        private void LoadMapInfo()
        {
            try
            {
                OleDbDataAdapter acs=new OleDbDataAdapter("SELECT * FROM mapInfo",mapLayer.GetAccessCon());
                ds=new DataSet();
                acs.Fill(ds,"layersInfo");
                int i=ds.Tables["layersInfo"].Rows.Count;
                mapLayer.m_layers=null;
                mapLayer.m_layers=new LayerInfo[i];

                for(int j=0;j<i;j++)
                {
                    mapLayer.m_layers[j].dtName=ds.Tables["layersInfo"].Rows[j]["Layer"].ToString();
                    mapLayer.m_layers[j].dtFile=ds.Tables["layersInfo"].Rows[j]["LayerName"].ToString();
                    mapLayer.m_layers[j].dtScale=System.Convert.ToDouble(ds.Tables["layersInfo"].Rows[j]["LayerScale"]);
                    mapLayer.m_layers[j].dtSize=System.Convert.ToInt64(ds.Tables["layersInfo"].Rows[j]["LayerSize"]);
                    mapLayer.m_layers[j].dtColor=System.Convert.ToInt64(ds.Tables["layersInfo"].Rows[j]["LayerColor"]);
                    mapLayer.m_layers[j].dtSymbol=System.Convert.ToInt64(ds.Tables["layersInfo"].Rows[j]["LayerSymbol"]);

                    mapLayer.m_layers[j].bzScale=System.Convert.ToInt64(ds.Tables["layersInfo"].Rows[j]["LabelScale"]);
                    mapLayer.m_layers[j].bzSize=System.Convert.ToInt64(ds.Tables["layersInfo"].Rows[j]["LabelSize"]);
                    mapLayer.m_layers[j].bzColor=System.Convert.ToInt64(ds.Tables["layersInfo"].Rows[j]["LabelColor"]);

                    mapLayer.m_layers[j].typeName=ds.Tables["layersInfo"].Rows[j]["TypeName"].ToString();
                }
                //				STAT.X=47699;
                //				STAT.Y=31331;
            }
            catch( Exception ex )
            {
                //MessageBox.Show("加载图层信息失败,请重新起动软件!","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                ExceptionHandler.Handle("加载图层信息失败,请重新起动软件!", ex );
            }
        }

        /// <summary>
        /// load layer from file, layer ->axMap1 ocx
        /// </summary>
        private void InitializeMainMap()
        {
            axMap1.Layers.Clear();
            axMap1.ScrollBars=false;
            MapObjects2.DataConnection mapCon=new MapObjects2.DataConnectionClass();
            try
            {
                mapCon.Database=Path.GetDirectoryName(Application.ExecutablePath)+"\\shaps\\";
                for(int m=0;m<3;m++)
                {
                    for(int i=0;i<mapLayer.m_layers.Length;i++)
                    {
                        MapObjects2.MapLayer layer;
                        layer = new MapObjects2.MapLayer();
                        layer.GeoDataset=mapCon.FindGeoDataset(mapLayer.m_layers[i].dtFile);				
                        switch (m)
                        {
                            case 0:
                                if (layer.shapeType != MapObjects2.ShapeTypeConstants.moShapeTypePolygon)
                                    continue;
                                break;
                            case 1:
                                if (layer.shapeType != MapObjects2.ShapeTypeConstants.moShapeTypeLine)
                                    continue;
                                break;
                            case 2:
                                if (layer.shapeType != MapObjects2.ShapeTypeConstants.moShapeTypePoint)
                                    continue;
                                break;
                            default:
                                continue;
                        }
                        mapLayer.m_layers[i].layer=layer;
                        axMap1.Layers.Add(mapLayer.m_layers[i].layer);
                        mapLayer.m_layers[i].layer.Symbol.Style=(short)mapLayer.m_layers[i].dtSymbol;
                        mapLayer.m_layers[i].layer.Symbol.Color=System.Convert.ToUInt32(mapLayer.m_layers[i].dtColor);
                        mapLayer.m_layers[i].layer.Symbol.Size=(short)mapLayer.m_layers[i].dtSize;
                        if(mapLayer.m_layers[i].layer.shapeType==MapObjects2.ShapeTypeConstants.moShapeTypePolygon)
                            mapLayer.m_layers[i].layer.Symbol.OutlineColor=System.Convert.ToUInt32(mapLayer.m_layers[i].dtColor);	
                    }
                }
		
                zoomRect=axMap1.Extent;
                zoomRect.ScaleRectangle(SC);
                				//axMap1.CenterAt(118.6679,39.5009);
				axMap1.Extent=zoomRect;
                axMap1.Refresh();				
            }
            catch (Exception ex)
            {
                //MessageBox.Show("加载图层失败,请重新起动软件!","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                ExceptionHandler.Handle("加载图层失败,请重新起动软件!", ex );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowLayers()
        {
            try
            {
                iScale=System.Convert.ToInt64(mapLayer.CalcScale(this.axMap1));
                for(int i=0;i<mapLayer.m_layers.Length;i++)
                {
                    if(mapLayer.m_layers[i].layer.Visible==true)
                    {
                        string name=mapLayer.m_layers[i].dtName;
                        if(mapLayer.m_layers[i].bzScale>iScale)
                        {
                            MapObjects2.LabelPlacer myRD=(MapObjects2.LabelPlacer)mapLayer.m_layers[i].layer.Renderer;
                            if (!(myRD != null && myRD.Field != ""))
                            {
                                myRD = new MapObjects2.LabelPlacer();
                                myRD.Field = "TEXTSTRING";
                                myRD.DrawBackground =true;
                                myRD.AllowDuplicates =false;
                                myRD.MaskLabels =false;
                                if (mapLayer.m_layers[i].layer.shapeType == MapObjects2.ShapeTypeConstants.moShapeTypeLine)   
								{myRD.PlaceAbove = true;
						         myRD.PlaceBelow=false;
								 myRD.PlaceOn=false;}
                                else
								{ myRD.PlaceAbove = true;
								  myRD.PlaceOn  = false;
								   myRD.PlaceBelow = false;}
                                myRD.DefaultSymbol.Font.Name="宋体";
                                myRD.DefaultSymbol.Font.Bold=false;
                                myRD.DefaultSymbol.Font.Size=System.Convert.ToDecimal(mapLayer.m_layers[i].bzSize);;
								myRD.DefaultSymbol.Color=System.Convert.ToUInt32(mapLayer.m_layers[i].bzColor);
                                mapLayer.m_layers[i].layer.Renderer = myRD;
                            }
                        }
                        else
                        {
                            MapObjects2.LabelPlacer myRD=(MapObjects2.LabelPlacer)mapLayer.m_layers[i].layer.Renderer;
                            if (myRD != null)
                                myRD.Field = "";
                        }
                    }
                }
                axMap1.Extent=axMap1.Extent;
                axMap1.Refresh();
            }
            catch(Exception ex)
            {
                //MessageBox.Show("图层信息加载失败,请重新起动软件!","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                ExceptionHandler.Handle("图层信息加载失败,请重新起动软件!", ex );
            }
        }

        /// <summary>
        /// di tu ceng = eyemap
        /// </summary>
        private void InitializeEyeMap()
        {
            MapObjects2.DataConnection mapCon=new MapObjects2.DataConnectionClass();
            MapObjects2.MapLayer EyeLayer=null; 
            try
            {
                mapCon.Database=Path.GetDirectoryName(Application.ExecutablePath)+"\\shaps\\";
                for(int i=0;i<mapLayer.m_layers.Length;i++)
                {
                    if(mapLayer.m_layers[i].typeName=="地图层" || mapLayer.m_layers[i].typeName=="全显层")
                    {
                        EyeLayer=new MapObjects2.MapLayerClass();
                        EyeLayer.GeoDataset=mapCon.FindGeoDataset(mapLayer.m_layers[i].dtFile);
                        EyeLayer.Symbol.Color=System.Convert.ToUInt32(mapLayer.m_layers[i].dtColor);
                        EyeLayer.Symbol.Size=1;
                        if(EyeLayer.shapeType==MapObjects2.ShapeTypeConstants.moShapeTypePolygon)
                            EyeLayer.Symbol.OutlineColor=System.Convert.ToUInt32(mapLayer.m_layers[i].dtColor);
                        axMap2.Layers.Add(EyeLayer);
                        axMap2.Refresh();
                    }
                }
            }
            catch( Exception ex )
            {
                //MessageBox.Show("加载图层失败,请重新起动软件!","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                ExceptionHandler.Handle("加载图层失败,请重新起动软件!", ex );
            }
        }
        #endregion

        #region GisControl
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMap2_AfterLayerDraw(object sender, AxMapObjects2._DMapEvents_AfterLayerDrawEvent e)
        {
            if(e.index!=0)
                return;
            symE=new MapObjects2.SymbolClass();
            symE.OutlineColor=(uint)MapObjects2.ColorConstants.moRed;
            symE.Outline=true;
            symE.SymbolType=MapObjects2.SymbolTypeConstants.moFillSymbol;
            symE.Style=(short)MapObjects2.FillStyleConstants.moTransparentFill;
            axMap2.DrawShape(axMap1.Extent,symE);
            MapObjects2.TextSymbol tsy=new MapObjects2.TextSymbolClass();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMap2_MouseUpEvent(object sender, AxMapObjects2._DMapEvents_MouseUpEvent e)
        {
            MapObjects2.Point pointE;
            pointE = axMap2.ToMapPoint(e.x, e.y);
            axMap1.CenterAt(pointE.X,pointE.Y);
            axMap2.Extent =axMap2.Extent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbGis1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (e.Button ==this.tBarZoomIn)
                zoomIn();
            if (e.Button==this.tBarZoomOut)
                zoomOut();
            if (e.Button==this.tBarPan)
                zoomPan();
            if(e.Button==this.tBarFull)
                zoomFull();
            if(e.Button==this.tBarRe)
                zoomRe();
            if(e.Button==this.tBarExit)
                this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void zoomIn()
        {
            tBarIndex=1;
            axMap1.MousePointer=MapObjects2.MousePointerConstants.moZoomIn;
        }

        /// <summary>
        /// 
        /// </summary>
        private void zoomOut()
        {
            tBarIndex=2;
            axMap1.MousePointer=MapObjects2.MousePointerConstants.moZoomOut;
        }

        /// <summary>
        /// 
        /// </summary>
        private void zoomPan()
        {
            tBarIndex=3;
            axMap1.MousePointer=MapObjects2.MousePointerConstants.moPan;
        }

        /// <summary>
        /// 
        /// </summary>
        private void zoomFull()
        {			
            axMap1.Extent=axMap1.FullExtent ;
            axMap1.Extent=axMap1.Extent;
            zoomRect=axMap1.Extent;
            zoomRect.ScaleRectangle(SC);
            //			axMap1.CenterAt(STAT.X,STAT.Y);
            axMap1.Extent=zoomRect;
            axMap1.Extent=axMap1.Extent;
            axMap1.Refresh();
            axMap2.Extent=axMap2.Extent;
            axMap1.MousePointer=MapObjects2.MousePointerConstants.moDefault;
            tBarIndex=0;
            ShowLayers();
        }

        /// <summary>
        /// 
        /// </summary>
        private void zoomRe()
        {
            tBarIndex=0;
            axMap1.MousePointer=MapObjects2.MousePointerConstants.moDefault;
        }

        /// <summary>
        /// axMap1 mouse down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMap1_MouseDownEvent(object sender, AxMapObjects2._DMapEvents_MouseDownEvent e)
        {			
            switch(tBarIndex)
            {
                case 1:
                {
					MapObjects2.Rectangle zoomInRect=new MapObjects2.RectangleClass();
                    MapObjects2.Point zoomInPt=new MapObjects2.PointClass();
                    zoomInRect=this.axMap1.TrackRectangle();
                    if (zoomInRect.Height>0 && zoomInRect.Width>0)
                    {
                        axMap1.Extent=zoomInRect;
                        axMap1.Extent=axMap1.Extent;		
                    }
                    else
                    {
                        zoomInPt=axMap1.ToMapPoint(e.x,e.y);
                        axMap1.CenterAt(zoomInPt.X,zoomInPt.Y);
                        zoomRect=axMap1.Extent;
                        sr=0.5;
                        zoomRect.ScaleRectangle(sr);
                        axMap1.Extent=zoomRect;
                        axMap1.Extent=axMap1.Extent;
                    }

                    ShowLayers();
                    axMap2.Extent=axMap2.Extent;
                    iScale=System.Convert.ToInt64(mapLayer.CalcScale(this.axMap1));
//					MessageBox.Show(iScale.ToString());
//                    if(iScale<=MAX_SCALE)
//                    {
//                        MessageBox.Show("选择图形比例过小，请重新选择!","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
//                        zoomFull();
//                        return;
//                    }
                    break;
                }

                case 2:
                {
                    MapObjects2.Rectangle zoomOutRect=new MapObjects2.RectangleClass();
                    MapObjects2.Point zoomOutPt=new MapObjects2.PointClass();
                    zoomOutRect=axMap1.TrackRectangle();
                    if(zoomOutRect.Height>0 && zoomOutRect.Width>0)
                    {
                        sr=axMap1.Extent.Width/zoomOutRect.Width;
                        axMap1.CenterAt(zoomOutRect.Center.X,zoomOutRect.Center.Y);
                    }
                    else
                    {
                        zoomOutPt=axMap1.ToMapPoint(e.x,e.y);
                        axMap1.CenterAt(zoomOutPt.X,zoomOutPt.Y);
                        sr=2;
                    }
                    zoomRect=axMap1.Extent;
                    zoomRect.ScaleRectangle(sr);
                    axMap1.Extent=zoomRect;
                    axMap1.Extent=axMap1.Extent;

                    ShowLayers();
                    axMap2.Extent=axMap2.Extent;
                    break;
                }

                case 3:
                {
                    axMap1.Pan();	
                   axMap2.Extent=axMap2.Extent;
                    break;
                }

                default:
                {   		
                    if(radioButton1.Checked==false)
                    {
                        MapObjects2.Point point;
                        point=axMap1.ToMapPoint(e.x,e.y);
						
					          // MessageBox.Show(point.X.ToString());
					          // MessageBox.Show(point.Y.ToString());

                        for(int i=0;i<s_Info.Length;i++)
                        {
							if(point.X>System.Convert.ToDouble(
								s_Info[i].s_East)-20 &&
								point.X<System.Convert.ToDouble(s_Info[i].s_East)+20 &&
								point.Y>System.Convert.ToDouble(s_Info[i].s_West)-20 && 
								point.Y<System.Convert.ToDouble(s_Info[i].s_West)+20
                                )
                            {
                        //        Point position=new Point();
                                GisMapSName=s_Info[i].s_Name;
                                GisMapSNo=s_Info[i].s_No;
                                //						position.X=e.x+POSITIONX;
                                //						position.Y=e.y+POSITIONY;
						        //                      GisMapMenu.Show(this,position);
                                if(GisMapSNo=="2"||GisMapSNo=="3")
                                {
                                    frmDataNowD f=new frmDataNowD(GisMapSName,GisMapSNo);
                                    f.ShowDialog();
									break;
                                }
                                else
                                {
                                    frmDataNow f=new frmDataNow(GisMapSName);
                                    f.ShowDialog();
									break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            if(axMap1.TrackingLayer.EventCount>0)
                axMap1.TrackingLayer.RemoveEvent(0);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DTLoadLayers()
        {
            for(int i=0;i<mapLayer.m_layers.Length;i++)
            {
                if(mapLayer.m_layers[i].typeName=="地图层"|mapLayer.m_layers[i].typeName=="全显层")
                    mapLayer.m_layers[i].layer.Visible=true;
                else
                    mapLayer.m_layers[i].layer.Visible=false;
            }
            if(axMap1.TrackingLayer.EventCount>0)
                axMap1.TrackingLayer.RemoveEvent(0);
            ShowLayers();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GWLoadLayers()
        {
            for(int i=0;i<mapLayer.m_layers.Length;i++)
            {
                //				if(mapLayer.m_layers[i].typeName=="背景层")
                //					mapLayer.m_layers[i].layer.Visible=false;
                //				else
                mapLayer.m_layers[i].layer.Visible=true;
            }
            ShowLayers();
        }

        /// <summary>
        /// 
        /// </summary>
        private void JKLoadLayers()
        {
            for(int i=0;i<mapLayer.m_layers.Length;i++)
            {
                if(mapLayer.m_layers[i].typeName=="地图层")
                    mapLayer.m_layers[i].layer.Visible=false;
                else
                    mapLayer.m_layers[i].layer.Visible=true;
            }
            ShowLayers();
        }
        #endregion

        #region ToolsControl
        /// <summary>
        /// 
        /// </summary>
        private void InitializeTools()
        {
            radioButton2.Checked=true;
            InitList();
            InitMenu();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
        {
            if(radioButton1.Checked==true)
            {
                DTLoadLayers();
                InitList();
            }
            if(radioButton2.Checked==true)
            {
                GWLoadLayers();
                InitList();
            }
            if(radioButton3.Checked==true)
            {
                JKLoadLayers();
                InitList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
        {
            if(radioButton1.Checked==true)
            {
                DTLoadLayers();
                InitList();
            }
            if(radioButton2.Checked==true)
            {
                GWLoadLayers();
                InitList();
            }
            if(radioButton3.Checked==true)
            {
                JKLoadLayers();
                InitList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitList()
        {
            listBox.Items.Clear();
            if(radioButton1.Checked==false)
            {
                listBox.Enabled=true;
                StationInfo sDataInfo=new StationInfo();
                s_Info=sDataInfo.GetPointInfo();
                string sn;
                for(int i=0;i<s_Info.Length;i++)
                {
                    sn=	s_Info[i].s_Name;	
                    listBox.Items.Add(sn);
                }
            }
            else
                listBox.Enabled=false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(axMap1.TrackingLayer.EventCount>0)
                axMap1.TrackingLayer.RemoveEvent(0);
            if(listBox.SelectedItems.Count>0)
            {
                string slName=listBox.SelectedItem.ToString();
                string s;
                MapObjects2.Point selectP=new MapObjects2.PointClass();
                for(int i=0;i<s_Info.Length;i++)
                {
                    s=s_Info[i].s_Name;
                    if(slName==s)
                    {
                        selectP.X=System.Convert.ToDouble(s_Info[i].s_East);
                        selectP.Y=System.Convert.ToDouble(s_Info[i].s_West);
                        axMap1.CenterAt(selectP.X,selectP.Y);
                        MapObjects2.Symbol selectS=axMap1.TrackingLayer.get_Symbol(0);
                        selectS.Color=System.Convert.ToUInt32(MapObjects2.ColorConstants.moBlue);
                        selectS.Size=10;
                        axMap1.TrackingLayer.AddEvent(selectP,0);
						break;
                    }
                }
            }
            if(this.timer1.Enabled==false)
                this.timer1.Enabled=true;
            else
            {
                this.timer1.Enabled=false;
                this.timer1.Enabled=true;
            }
        }
        #endregion

        #region MenuControl
        private void InitMenu()//(Point m)//
        {
            ////			Point position=new Point();
            //			GisMapMenu=new System.Windows.Forms.ContextMenu();
            //			this.GisMapMenu=this.GisMapMenu;
            //			MenuItem[] heatMenus=new MenuItem[4];
            //			MenuItem heatMenuItem1=new MenuItem("实时数据",new System.EventHandler(this.MenuClick_HeatNow));
            //			MenuItem heatMenuItem2=new MenuItem("数据查询",new System.EventHandler(this.MenuClick_HeatOld));
            //			MenuItem heatMenuItem3=new MenuItem("曲线查询",new System.EventHandler(this.MenuClick_HeatCurve));
            //			MenuItem heatMenuItem4=new MenuItem("报警查询",new System.EventHandler(this.MenuClick_HeatAlarm));
            //			heatMenus[0]=heatMenuItem1;
            //			heatMenus[1]=heatMenuItem2;
            //			heatMenus[2]=heatMenuItem3;
            //			heatMenus[3]=heatMenuItem4;
            ////			menuItem=new MenuItem("供热系统",heatMenus);
            ////			menuItem.Enabled=true;
            ////			GisMapMenu.MenuItems.Add( menuItem );
            ////			MenuItem[] subMenus=new MenuItem[2];
            ////			MenuItem subMenuItem1=new MenuItem("TM管理");
            ////			MenuItem subMenuItem2=new MenuItem("数据管理");
            ////			MenuItem subMenuItem3=new MenuItem("任务执行结果");
            ////			MenuItem subMenuItem4=new MenuItem("参数设置");
            //			subMenus[0]=subMenuItem1;
            //			subMenus[1]=subMenuItem2;
            //			subMenus[2]=subMenuItem3;
            ////			subMenus[3]=subMenuItem4;
            //			menuItem=new MenuItem("巡更系统",subMenus);
            //			GisMapMenu.MenuItems.Add( menuItem );	
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*private void MenuClick_HeatNow(System.Object sender, System.EventArgs e)
        {
            if(GisMapSNo=="2")
            {
                frmDataNowD f=new frmDataNowD(GisMapSName);
                f.ShowDialog();
            }
            else
            {
                frmDataNow f=new frmDataNow(GisMapSName);
                f.ShowDialog();
            }
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuClick_HeatOld(System.Object sender, System.EventArgs e)
        {
            //			Grid.frmDataPrint f=new DataCurve.Grid.frmDataPrint(GisMapSName);
            //			f.Text=GisMapSName+"热力站数据查询";
            //			f.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuClick_HeatCurve(System.Object sender, System.EventArgs e)
        {
            //			Curve.frmCurve f=new DataCurve.Curve.frmCurve(GisMapSName);
            //			f.Text=GisMapSName+"热力站曲线查询";
            //			f.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuClick_HeatAlarm(System.Object sender, System.EventArgs e)
        {
			
        }
        #endregion

        #region Other

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGisMain_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {		
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMap1_DrawingCanceled(object sender, System.EventArgs e)
        {
			
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMap1_DblClick(object sender, System.EventArgs e)
        {
            //			if(tBarIndex==0)
            //			{
            //				if(radioButton1.Checked==false)
            //				{
            //					MapObjects2.Point point;
            //					point=axMap1.ToMapPoint(e.x,e.y);
            //					for(int i=0;i<s_Info.Length;i++)
            //					{
            //						if(point.X>System.Convert.ToDouble(s_Info[i].s_East)-5 &&point.X<System.Convert.ToDouble(s_Info[i].s_East)+5&&point.Y>System.Convert.ToDouble(s_Info[i].s_West)-5 && point.Y<System.Convert.ToDouble(s_Info[i].s_West)+5)
            //						{
            //							GisMapSName=s_Info[i].s_Name;
            //							GisMapSNo=s_Info[i].s_No;
            //							if(GisMapSNo=="2")
            //							{
            //								frmDataNowD f=new frmDataNowD(GisMapSName);
            //								f.ShowDialog();
            //							}
            //							else
            //							{
            //								frmDataNow f=new frmDataNow(GisMapSName);
            //								f.ShowDialog();
            //							}
            //						}
            //					}
            //				}
            //			}
        }

		private void radioButton3_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

		
    }
}
