using System;
using System.Data;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using SocketRSLib;
using System.Drawing;

namespace SocketServer
{
    public partial class main : Form
    {        
        private System.Drawing.Point mousePosition;
        
        #region Members
        private SynchronizationContext _syn;
        private Tool.SocketServer _server;
        private SocketRSCollection _socketRSs = new SocketRSCollection();
        private ISocketRS _currentSocketRS;
        #endregion //

        public static string ip;
        public static string port;
        public static string sql_con;

        public struct outtemp
        {
            public static string station;
            public static float value;
            public static bool enble;
            public static int cycle;
            public static DateTime dtlast;
        }

        public struct ISocketRSlist
        {
            public string _ip;
            public ISocketRS _rs;
            public bool _Iscon;
            public bool _Isbusy;
        }

        public struct CardtoPerson
        {
            public string _card;
            public string _person;
        }

        public main()
        {
            InitializeComponent();
        }      

        private void LoadTreeNodes()
        {
            string sql = "select DISTINCT [group] from tb_rzStations";
            DataTable dt = Tool.DB.getDt(sql);
            if (dt == null)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            { 
                TreeNode tn = new TreeNode(dt.Rows[i]["group"].ToString());
                treeView1.Nodes.Add(tn);
                LoadTreeNodes2(tn);              
            }
            treeView1.ExpandAll();
        }
        private void LoadTreeNodes2(TreeNode tn)
        {
            string sql = "select StationName,StationId from tb_rzStations where [group]= '" + tn.Text + "'";
            DataTable dt = Tool.DB.getDt(sql);
            if (dt == null)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TreeNode tn1 = new TreeNode(dt.Rows[i]["StationName"].ToString());
                tn1.Tag = dt.Rows[i]["StationId"].ToString();
                tn.Nodes.Add(tn1);
            }
        }

        #region Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            this.toolStrip1.Location = CalcToolStripLocal(); 
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            //异步传递
            this._syn = SynchronizationContext.Current;

            //读取配置文件
            xml_load();

            //加载菜单
            LoadTreeNodes();

            //建立ISocketRS列表
            _ISocketRSlist = CreateISocketRSlist();

            //建立实时数据缓存
            _XDRZInfoList = Createxd100nDatas();

            //建立卡号人名关系列表
            _CardtoPersonlist = Createcardtopersonlist();

            //建立巡更缓存
            _XDXGInfoList = Createxd300Datas();

            //开始监听
            Listen_start(ip, port);

            //异步调用开启定时器        
            this.SyncExec(10);

            //界面的初始化
            from_ini();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Point CalcToolStripLocal()
        {
            int x = this.Size.Width - this.toolStrip1.Width - 1;
            int y = this.toolStrip1.Location.Y;
            Point pt = new Point(x, y);
            return pt;
        }

        //界面的初始化
        private void from_ini()
        {
            pictureBox6.Parent = pictureBox7;
            toolStrip1.Parent = pictureBox7;
            label6.Parent = pictureBox7;

            Form back = new background();
            back.Text = "背景";
            back.MdiParent = this;
            back.WindowState = FormWindowState.Maximized;
            back.Show();

            Form f = new btGR.frmGisMain();
            f.Text = "地理信息";
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;

            f = new curve();
            f.Text = "曲线";
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;

            f = new branch();
            f.Text = "管网图";
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;

            f = new rz.rz_flowchart();
            f.Text = "流程图";
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;

            f = new rz.rz_main();
            f.Text = "供热系统";
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;

        }

        //加载配置
        private void xml_load()
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("config.xml");
                //监听ip地址
                ip = xDoc.DocumentElement.ChildNodes[0].Attributes.GetNamedItem("value").Value.Trim();
                //监听端口
                port = xDoc.DocumentElement.ChildNodes[1].Attributes.GetNamedItem("value").Value.Trim();
                //数据库联接字符串
                Tool.DB.sqlcon = xDoc.DocumentElement.ChildNodes[2].Attributes.GetNamedItem("value").Value.Trim();  
                //室外温度基准站点
                outtemp.station = xDoc.DocumentElement.ChildNodes[3].Attributes.GetNamedItem("value").Value.Trim();
                //室外温度开启循环
                if (xDoc.DocumentElement.ChildNodes[3].Attributes.GetNamedItem("enble").Value.Trim() == "true")
                {
                    outtemp.enble = true;
                }
                else
                {
                    outtemp.enble = false;
                }
                //室外温度循环周期
                outtemp.cycle = Convert.ToInt16(xDoc.DocumentElement.ChildNodes[3].Attributes.GetNamedItem("cycle").Value.Trim());

            }
            catch (Exception ex)
            {
                MessageBox.Show("读取配置文件出错！请确定程序完整性！" + ex, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //建立ISocketRS列表
        public ISocketRSlist[] CreateISocketRSlist()
        {
            try
            {
                string sql = "select DISTINCT [IPAddress] from tb_Gprs";
                DataTable dt = Tool.DB.getDt(sql);
                ISocketRSlist[] _ISocketRSlist = new ISocketRSlist[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _ISocketRSlist[i]._ip = dt.Rows[i]["IPAddress"].ToString();
                }
                return _ISocketRSlist;
            }
            catch
            {
                MessageBox.Show("建立ISocketRS列表失败！请检查数据库连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        //建立卡号人名关系列表
        public CardtoPerson[] Createcardtopersonlist()
        {
            try
            {
                string sql = "select [card],[person] from tb_xgcard";
                DataTable dt = Tool.DB.getDt(sql);
                CardtoPerson[] _CardtoPerson = new CardtoPerson[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _CardtoPerson[i]._card = dt.Rows[i]["card"].ToString();
                    _CardtoPerson[i]._person = dt.Rows[i]["person"].ToString();
                }
                return _CardtoPerson;
            }
            catch
            {
                MessageBox.Show("建立cardtoperson列表失败！请检查数据库连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        //建立xd100n数据缓存
        public Tool.XDRZInfo[] Createxd100nDatas()
        {
            try
            {
                string sql = "select [StationId],[StationName],[DTUregister],[IPAddress],[deviceAddress],[Remark],[heatArea],[heatbase],[group],[cycle],[timeout],[retrytimes] from v_rzstation order by [StationId]";
                DataTable dt = Tool.DB.getDt(sql);
                Tool.XDRZInfo[] baseList = new Tool.XDRZInfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    
                    baseList[i]._station._StationId = int.Parse(dt.Rows[i]["StationId"].ToString());
                    baseList[i]._station._StationName = dt.Rows[i]["StationName"].ToString();
                    baseList[i]._station._Remark = dt.Rows[i]["Remark"].ToString();
                    baseList[i]._station._heatbase = float.Parse(dt.Rows[i]["heatbase"].ToString());
                    baseList[i]._station._heatArea = float.Parse(dt.Rows[i]["heatArea"].ToString());
                    baseList[i]._station._group = dt.Rows[i]["group"].ToString();

                    baseList[i]._station._DTUregister = dt.Rows[i]["DTUregister"].ToString();
                    baseList[i]._station._IPAddress = dt.Rows[i]["IPAddress"].ToString();
                    baseList[i]._station._cycle = int.Parse(dt.Rows[i]["cycle"].ToString());
                    baseList[i]._station._deviceAddress = int.Parse(dt.Rows[i]["deviceAddress"].ToString());
                    baseList[i]._station._timeout = int.Parse(dt.Rows[i]["timeout"].ToString());
                    baseList[i]._station._retrytimes = int.Parse(dt.Rows[i]["retrytimes"].ToString());
                    baseList[i].state = true;

                    Tool.XD100n x = new Tool.XD100n(baseList[i]._station._deviceAddress);

                    string sql2 = "SELECT [StationName],[alarmword], [DT], [oneGiveTemp], [oneBackTemp], [twoGiveTemp], [twoBackTemp], [outsideTemp]," + 
                        "[twoGiveBaseTemp], [oneGivePress], [oneBackPress], [WatBoxLevel], [twoGivePress], [twoBackPress], "+
                        "[twoBackBasePress], [oneInstant], [oneAccum], [subInstant], [subAccum], [oneHeat], [oneAddHeat],"+
                        " [twoPressCha], [openDegree], [pumpState1], [pumpState2], [pumpState3], [addPumpState1], [addPumpState2],"+
                        " [StationId], [twoInstant], [twoAccum], [twoHeat], [twoAddHeat] FROM [v_rzreallast] where [StationId]=" + baseList[i]._station._StationId.ToString();
                    DataTable dt1 = Tool.DB.getDt(sql2);
                    if (dt1.Rows.Count>0)
                    {
                        baseList[i]._nowDatas._alarm._word = Convert.ToInt32(dt1.Rows[0]["alarmword"]);
                        baseList[i]._nowDatas._dt = Convert.ToDateTime(dt1.Rows[0]["DT"]);
                        baseList[i]._nowDatas._oneGiveTemp = Convert.ToSingle(dt1.Rows[0]["oneGiveTemp"]);
                        baseList[i]._nowDatas._oneBackTemp = Convert.ToSingle(dt1.Rows[0]["oneBackTemp"]);
                        baseList[i]._nowDatas._twoGiveTemp = Convert.ToSingle(dt1.Rows[0]["twoGiveTemp"]);
                        baseList[i]._nowDatas._twoBackTemp = Convert.ToSingle(dt1.Rows[0]["twoBackTemp"]);
                        baseList[i]._nowDatas._outsideTemp = Convert.ToSingle(dt1.Rows[0]["outsideTemp"]);
                        baseList[i]._nowDatas._twoGiveBaseTemp = Convert.ToSingle(dt1.Rows[0]["twoGiveBaseTemp"]);

                        baseList[i]._nowDatas._oneGivePress = Convert.ToSingle(dt1.Rows[0]["oneGivePress"]);
                        baseList[i]._nowDatas._oneBackPress = Convert.ToSingle(dt1.Rows[0]["oneBackPress"]);
                        baseList[i]._nowDatas._WatBoxLevel = Convert.ToSingle(dt1.Rows[0]["WatBoxLevel"]);
                        baseList[i]._nowDatas._twoGivePress = Convert.ToSingle(dt1.Rows[0]["twoGivePress"]);
                        baseList[i]._nowDatas._twoBackPress = Convert.ToSingle(dt1.Rows[0]["twoBackPress"]);

                        baseList[i]._nowDatas._oneInstant = Convert.ToSingle(dt1.Rows[0]["oneInstant"]);
                        baseList[i]._nowDatas._oneAccum = Convert.ToUInt32(dt1.Rows[0]["oneAccum"]);
                        baseList[i]._nowDatas._oneHeat = Convert.ToSingle(dt1.Rows[0]["oneHeat"]);
                        baseList[i]._nowDatas._oneAddHeat = Convert.ToDouble(dt1.Rows[0]["oneAddHeat"]);

                        baseList[i]._nowDatas._subInstant = Convert.ToSingle(dt1.Rows[0]["subInstant"]);
                        baseList[i]._nowDatas._subAccum = Convert.ToUInt32(dt1.Rows[0]["subAccum"]);

                        baseList[i]._nowDatas._twoInstant = Convert.ToSingle(dt1.Rows[0]["twoInstant"]);
                        baseList[i]._nowDatas._twoAccum = Convert.ToUInt32(dt1.Rows[0]["twoAccum"]);
                        baseList[i]._nowDatas._twoHeat = Convert.ToSingle(dt1.Rows[0]["twoHeat"]);
                        baseList[i]._nowDatas._twoAddHeat = Convert.ToDouble(dt1.Rows[0]["twoAddHeat"]);

                        baseList[i]._nowDatas._twoPressCha = Convert.ToSingle(dt1.Rows[0]["twoPressCha"]);
                        baseList[i]._nowDatas._openDegree = Convert.ToSingle(dt1.Rows[0]["openDegree"]);

                        baseList[i]._nowDatas._pump._cycPump1 = dttops(dt1.Rows[0]["pumpState1"].ToString());  
                        baseList[i]._nowDatas._pump._cycPump2 = dttops(dt1.Rows[0]["pumpState2"].ToString());
                        baseList[i]._nowDatas._pump._cycPump3 = dttops(dt1.Rows[0]["pumpState3"].ToString());
                        baseList[i]._nowDatas._pump._recruitPump1 = dttops(dt1.Rows[0]["addPumpState1"].ToString());
                        baseList[i]._nowDatas._pump._recruitPump1 = dttops(dt1.Rows[0]["addPumpState2"].ToString());

                        baseList[i]._nowDatas._alarm._word=Convert.ToInt32(dt1.Rows[0]["alarmword"]);

                        byte[] alarm = {Convert.ToByte(baseList[i]._nowDatas._alarm._word/256),Convert.ToByte(baseList[i]._nowDatas._alarm._word%256)};
                        baseList[i]._nowDatas._alarm = x.AlarmParse(alarm);
                        
                        baseList[i]._refDisplay = true;
                    }

                    
                    baseList[i]._command=new Tool.Commandcyc[2];
                    baseList[i]._command[0]._cmd = x.Get_nowdata();
                    baseList[i]._command[0]._onoff = true;
                    baseList[i]._command[1]._cmd = x.Get_di();
                    baseList[i]._command[1]._onoff = true;

                    baseList[i]._commandonce = new Tool.Commandonce[21];

                    baseList[i]._commandonce[0]._cmd = x.Get_controltype();
                    baseList[i]._commandonce[1]._cmd = x.Get_setvalue();
                    baseList[i]._commandonce[2]._cmd = x.Get_line();
                    baseList[i]._commandonce[3]._cmd = x.Get_timepace();
                    baseList[i]._commandonce[4]._cmd = x.Get_valvemm();
                    baseList[i]._commandonce[5]._cmd = x.Get_valvelimit();
                    baseList[i]._commandonce[6]._cmd = x.Get_alarm();
                    baseList[i]._commandonce[7]._cmd = x.Get_outtemp();

                    baseList[i]._commandonce[8]._cmd = x.Set_controltype(baseList[i]._Set._controltype);
                    baseList[i]._commandonce[9]._cmd = x.Set_setvalue(baseList[i]._Set._setvalue);
                    baseList[i]._commandonce[10]._cmd = x.Set_line(baseList[i]._Set._line);
                    baseList[i]._commandonce[11]._cmd = x.Set_timepace(baseList[i]._Set._timepace);
                    baseList[i]._commandonce[12]._cmd = x.Set_valvemm(baseList[i]._Set._valvemm);
                    baseList[i]._commandonce[13]._cmd = x.Set_valvelimit(baseList[i]._Set._valvelimit);
                    baseList[i]._commandonce[14]._cmd = x.Set_alarm(baseList[i]._Set._alarm);              
                    baseList[i]._commandonce[15]._cmd = x.Set_outtemp(baseList[i]._Set._outtemp);

                    baseList[i]._commandonce[16]._cmd = x.Set_outtemp_value(baseList[i]._Set._outtemp._outtemp);

                    baseList[i]._commandonce[17]._cmd = x.Set_xunhuan_stop();
                    baseList[i]._commandonce[18]._cmd = x.Set_bushui_stop();
                    baseList[i]._commandonce[19]._cmd = x.Set_xunhuan_start();
                    baseList[i]._commandonce[20]._cmd = x.Set_bushui_start();
                    
                    for (int k = 0; k < baseList[i]._commandonce.Length; k++)
                    {
                        baseList[i]._commandonce[k]._onoff = false;
                    }
                }
                return baseList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("建立数据缓存失败！请检查数据库连接！" + ex, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }            
        }

        //建立xd300数据缓存
        public Tool.XDXGInfo[] Createxd300Datas()
        {
            try
            {
                string sql = "SELECT [XgID],[XgName], [deviceAddress], [DTUregister], [IPAddress], [Remark], [cycle], [timeout], [retrytimes] FROM [v_xgstation]";
                DataTable dt = Tool.DB.getDt(sql);
                Tool.XDXGInfo[] baseList = new Tool.XDXGInfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    baseList[i]._XGInfo._XgID = int.Parse(dt.Rows[i]["XgID"].ToString());
                    baseList[i]._XGInfo._XgName = dt.Rows[i]["XgName"].ToString();
                    baseList[i]._XGInfo._Remark = dt.Rows[i]["Remark"].ToString();

                    baseList[i]._XGInfo._DTUregister = dt.Rows[i]["DTUregister"].ToString();
                    baseList[i]._XGInfo._IPAddress = dt.Rows[i]["IPAddress"].ToString();
                    baseList[i]._XGInfo._cycle = int.Parse(dt.Rows[i]["cycle"].ToString());
                    baseList[i]._XGInfo._deviceAddress = int.Parse(dt.Rows[i]["deviceAddress"].ToString());
                    baseList[i]._XGInfo._timeout = int.Parse(dt.Rows[i]["timeout"].ToString());
                    baseList[i]._XGInfo._retrytimes = int.Parse(dt.Rows[i]["retrytimes"].ToString());
                    baseList[i].state = true;

                    Tool.xd300 x = new Tool.xd300(baseList[i]._XGInfo._deviceAddress);

                    string sql2 = "SELECT [xglastdata_id], [DT], [XgName], [person], [XgID] FROM [v_xgreallast] where [XgID]=" + baseList[i]._XGInfo._XgID.ToString();
                    DataTable dt1 = Tool.DB.getDt(sql2);
                    if (dt1.Rows.Count > 0)
                    {
                        baseList[i]._XGDataNow._DT = dt1.Rows[0]["DT"].ToString();
                        baseList[i]._XGDataNow._person = dt1.Rows[0]["person"].ToString(); ;
                        baseList[i]._refDisplay = true;
                    }

                    baseList[i]._command = new Tool.XGCommandcyc[1];
                    baseList[i]._command[0]._cmd = x.Get_record_count();
                    baseList[i]._command[0]._onoff = true;

                    baseList[i]._commandonce = new Tool.XGCommandonce[5];
                    baseList[i]._commandonce[0]._cmd = x.Get_record_n(0);
                    baseList[i]._commandonce[1]._cmd = x.Get_record_clean_now();
                    baseList[i]._commandonce[2]._cmd = x.Get_record_clean();
                    baseList[i]._commandonce[3]._cmd = x.Set_date();
                    baseList[i]._commandonce[4]._cmd = x.Set_time();

                    for (int k = 0; k < baseList[i]._commandonce.Length; k++)
                    {
                        baseList[i]._commandonce[k]._onoff = false;
                    }

                }
                return baseList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("建立数据缓存失败！请检查数据库连接！" + ex, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        private Tool.PumpState dttops(string str)
        {
            if (str == Tool.PumpState.启动.ToString())
            {
                return Tool.PumpState.启动;
            }
            else
            {
                return Tool.PumpState.停止;
            }
        }

        #endregion //

        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要退出程序吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {                
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Visible = false;
            if (_server != null)            
            {
                this._server.Close();
                this._server = null;
            }
        }


        //异步传递
        #region Sync
        private void NoNameCallback(object state)
        {
            if (state is int)
            {
                int n = (int)state;
                if (n == 10)
                {
                    timer1.Interval = 1000;
                    timer1.Enabled = true;
                    timer2.Interval = 1000;
                    timer2.Enabled = true;
                    timer3.Interval = 60000;
                    timer3.Enabled = true;
                    return;
                }
            }

        }

        SendOrPostCallback _callback;

        private SendOrPostCallback GetSynCallback()
        {
            if (this._callback == null)
                _callback = new SendOrPostCallback(NoNameCallback);
            return _callback;
        }

        private void SyncExec(object state)
        {
            _syn.Post(GetSynCallback(), state);
        }
        #endregion

        //异步通讯
        #region
        //开始监听
        private void Listen_start(string ip,string port)
        {
            if (_server == null)
            {
                _server = new Tool.SocketServer();
                try
                {
                    _server.Listen(ip, int.Parse(port));
                    _server.NewConnectEvent += new EventHandler(_server_NewConnectEvent);

                }
                catch 
                {
               //     MessageBox.Show(ex.Message, "Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _server = null;
                    return;
                }
            }
            else
            {
                _server.Close();
                _server = null;
            }
        }
        //新建连接  --可能有多个设备共用一个rs
        private void _server_NewConnectEvent(object sender, EventArgs e)
        {
            ISocketRS rs = new SocketRSAPM(this._server.NewSocket);
            string ip = ((IPEndPoint)rs.Socket.RemoteEndPoint).Address.ToString();
            for (int i = 0; i < _ISocketRSlist.Length; i++)
            {
                if (_ISocketRSlist[i]._ip == ip)
                {
                    _ISocketRSlist[i]._rs = rs;
                    _ISocketRSlist[i]._Iscon = true;
                    _ISocketRSlist[i]._Isbusy = false;

                    this._socketRSs.Add(rs);
                    rs.ReceivedEvent += new EventHandler(rs_ReceivedEvent);
                    rs.ClosedEvent += new EventHandler(rs_ClosedEvent);
                    break;
                }
            }
        }

        //通过ip获取ISocketRSlist
        private ISocketRSlist GetISocketRSlist(string ip)
        {
            ISocketRSlist isl = new ISocketRSlist();
            for (int i = 0; i < _ISocketRSlist.Length; i++)
            {
                if (_ISocketRSlist[i]._ip == ip)
                {
                    isl = _ISocketRSlist[i];
                    break;
                }
            }
            return isl;
        }

        //ISocketRS占用/解除
        private void On_ISocketRS(string ip, bool onoff)
        {
            for (int k = 0; k < _ISocketRSlist.Length; k++)
            {
                if (_ISocketRSlist[k]._ip == ip)
                {
                    _ISocketRSlist[k]._Isbusy = onoff;
                    break;
                }
            }
        }

        //xd100n发送任务
        private void rz_send_rask()
        {
            for (int i = 0; i < _XDRZInfoList.Length; i++)
            {
                //一次执行命令
                for (int j = 0; j < _XDRZInfoList[i]._commandonce.Length; j++)
                {
                    //判断发送完成 开始计时
                    if (_XDRZInfoList[i]._commandonce[j]._sendover == true)
                    {
                        _XDRZInfoList[i]._commandonce[j]._timeoutnow++;
                    }
                    else
                    {
                        //_XDRZInfoList[i]._commandonce[j]._timeoutnow = 0;
                    }
                    //判断超时
                    if (_XDRZInfoList[i]._commandonce[j]._timeoutnow >= _XDRZInfoList[i]._station._timeout)
                    {
                        _XDRZInfoList[i]._commandonce[j]._timeoutnow = 0;
                        _XDRZInfoList[i]._commandonce[j]._sendover = false;
                        _XDRZInfoList[i]._commandonce[j]._onoff = false;
                        //解除ISocketRS占用
                        On_ISocketRS(_XDRZInfoList[i]._station._IPAddress, false);
                    }
                    //判断是不是需要发命令
                    if (_XDRZInfoList[i]._commandonce[j]._onoff == true)
                    {
                        //检测ISocketRS是否被占用和是否连接 
                        ISocketRSlist isl = GetISocketRSlist(_XDRZInfoList[i]._station._IPAddress);
                        if (isl._Iscon == false || isl._Isbusy == true)
                        {
                            continue;
                        }
                        bool send_flg = rs_send(isl._ip, _XDRZInfoList[i]._commandonce[j]._cmd);
                        if (send_flg == true)
                        {
                            _XDRZInfoList[i]._commandonce[j]._onoff = false;
                            _XDRZInfoList[i]._commandonce[j]._sendover = true;
                            _XDRZInfoList[i]._commandonce[j]._timeoutnow = 0;
                            _XDRZInfoList[i].lastaddr = _XDRZInfoList[i]._commandonce[j]._cmd[2] * 256 + _XDRZInfoList[i]._commandonce[j]._cmd[3];
                            //占用ISocketRS
                            On_ISocketRS(_XDRZInfoList[i]._station._IPAddress, true);
                        }
                    }
                }
                //周期执行命令
                for (int j = 0; j < _XDRZInfoList[i]._command.Length; j++)
                {
                    //判断命令是否开启
                    if (_XDRZInfoList[i]._command[j]._onoff == false)
                    {
                        break;
                    }
                    //判断发送完成 开始计时
                    if (_XDRZInfoList[i]._command[j]._sendover == true)
                    {
                        Console.WriteLine( "before: " + 
                        _XDRZInfoList[i]._command[j]._timeoutnow
                            );
                        _XDRZInfoList[i]._command[j]._timeoutnow++;
                        Console.WriteLine( "after: " + 
                        _XDRZInfoList[i]._command[j]._timeoutnow );
                    }
                    else
                    {
                        //_XDRZInfoList[i]._command[j]._timeoutnow=0;
                    }
                    //判断超时
                    if (_XDRZInfoList[i]._command[j]._timeoutnow >= _XDRZInfoList[i]._station._timeout)
                    {
                        //_XDRZInfoList[i]._command[j]._timeoutnow = 0;
                        _XDRZInfoList[i]._command[j]._sendover = false;
                        //解除ISocketRS占用
                        On_ISocketRS(_XDRZInfoList[i]._station._IPAddress, false);
                        _XDRZInfoList[i]._command[j]._retrytimesnow++;
                    }
                    //判断重试次数 超过次数 设备故障 //初始化连接
                    if (_XDRZInfoList[i]._command[j]._retrytimesnow >= _XDRZInfoList[i]._station._retrytimes)
                    {
                        _XDRZInfoList[i]._command[j]._retrytimesnow = 0;
                        _XDRZInfoList[i].state = false;
                        //   _currentSocketRS = GetISocketRSlist(_XDRZInfoList[i]._station._IPAddress)._rs;
                        //   _currentSocketRS.Close();
                    }
                    //判断是否到发送周期
                    TimeSpan ts = DateTime.Now - _XDRZInfoList[i]._command[j]._dt;
                    if (ts.TotalMinutes >= _XDRZInfoList[i]._station._cycle)
                    {
                        //检测ISocketRS是否被占用和是否连接 
                        ISocketRSlist isl = GetISocketRSlist(_XDRZInfoList[i]._station._IPAddress);
                        if (isl._Iscon == false || isl._Isbusy == true)
                        {
                            continue; ;
                        }
                        bool send_flg = rs_send(isl._ip, _XDRZInfoList[i]._command[j]._cmd);
                        if (send_flg == true)
                        {
                            _XDRZInfoList[i]._command[j]._sendover = true;
                            //_XDRZInfoList[i]._command[j]._timeoutnow = 0;
                            _XDRZInfoList[i]._command[j]._dt = DateTime.Now;
                            //占用ISocketRS
                            On_ISocketRS(_XDRZInfoList[i]._station._IPAddress, true);
                        }
                    }
                }

            }
        }
        //xd300发送任务
        private void xg_send_task()
        {
            for (int i = 0; i < _XDXGInfoList.Length; i++)
            {
                //一次执行命令
                for (int j = 0; j < _XDXGInfoList[i]._commandonce.Length; j++)
                {
                    //判断发送完成 开始计时
                    if (_XDXGInfoList[i]._commandonce[j]._sendover == true)
                    {
                        _XDXGInfoList[i]._commandonce[j]._timeoutnow++;
                    }
                    else
                    {
                        _XDRZInfoList[i]._commandonce[j]._timeoutnow = 0;
                    }
                    //判断超时
                    if (_XDXGInfoList[i]._commandonce[j]._timeoutnow >= _XDXGInfoList[i]._XGInfo._timeout)
                    {
                        _XDXGInfoList[i]._commandonce[j]._timeoutnow = 0;
                        _XDXGInfoList[i]._commandonce[j]._sendover = false;
                        _XDXGInfoList[i]._commandonce[j]._onoff = false;
                        //解除ISocketRS占用
                        On_ISocketRS(_XDXGInfoList[i]._XGInfo._IPAddress, false);
                    }
                    //判断是不是需要发命令
                    if (_XDXGInfoList[i]._commandonce[j]._onoff == true)
                    {
                        //检测ISocketRS是否被占用和是否连接 
                        ISocketRSlist isl = GetISocketRSlist(_XDXGInfoList[i]._XGInfo._IPAddress);
                        if (isl._Iscon == false || isl._Isbusy == true)
                        {
                            continue;
                        }
                        bool send_flg = rs_send(isl._ip, _XDXGInfoList[i]._commandonce[j]._cmd);
                        if (send_flg == true)
                        {
                            _XDXGInfoList[i]._commandonce[j]._onoff = false;
                            _XDXGInfoList[i]._commandonce[j]._sendover = true;
                            _XDXGInfoList[i]._commandonce[j]._timeoutnow = 0;
                            //占用ISocketRS
                            On_ISocketRS(_XDXGInfoList[i]._XGInfo._IPAddress, true);
                        }
                    }
                }
                //周期执行命令
                for (int j = 0; j < _XDXGInfoList[i]._command.Length; j++)
                {
                    //判断命令是否开启
                    if (_XDXGInfoList[i]._command[j]._onoff == false)
                    {
                        break;
                    }
                    //判断发送完成 开始计时
                    if (_XDXGInfoList[i]._command[j]._sendover == true)
                    {
                        _XDXGInfoList[i]._command[j]._timeoutnow++;
                    }
                    else
                    {
                        //_XDRZInfoList[i]._command[j]._timeoutnow = 0;
                    }
                    //判断超时
                    if (_XDXGInfoList[i]._command[j]._timeoutnow >= _XDXGInfoList[i]._XGInfo._timeout)
                    {
                        _XDXGInfoList[i]._command[j]._timeoutnow = 0;
                        _XDXGInfoList[i]._command[j]._sendover = false;
                        //解除ISocketRS占用
                        On_ISocketRS(_XDXGInfoList[i]._XGInfo._IPAddress, false);
                        _XDXGInfoList[i]._command[j]._retrytimesnow++;
                    }
                    //判断重试次数 超过次数 设备故障 //初始化连接
                    if (_XDXGInfoList[i]._command[j]._retrytimesnow >= _XDXGInfoList[i]._XGInfo._retrytimes)
                    {
                        _XDXGInfoList[i]._command[j]._retrytimesnow = 0;
                        _XDXGInfoList[i].state = false;
                        //   _currentSocketRS = GetISocketRSlist(_XDRZInfoList[i]._station._IPAddress)._rs;
                        //   _currentSocketRS.Close();
                    }
                    //判断是否到发送周期
                    TimeSpan ts = DateTime.Now - _XDXGInfoList[i]._command[j]._dt;
                    if (ts.TotalMinutes >= _XDXGInfoList[i]._XGInfo._cycle)
                    {
                        //检测ISocketRS是否被占用和是否连接 
                        ISocketRSlist isl = GetISocketRSlist(_XDXGInfoList[i]._XGInfo._IPAddress);
                        if (isl._Iscon == false || isl._Isbusy == true)
                        {
                            continue;
                        }
                        bool send_flg = rs_send(isl._ip, _XDXGInfoList[i]._command[j]._cmd);
                        if (send_flg == true)
                        {
                            _XDXGInfoList[i]._command[j]._sendover = true;
                            _XDXGInfoList[i]._command[j]._timeoutnow = 0;
                            _XDXGInfoList[i]._command[j]._dt = DateTime.Now;
                            //占用ISocketRS
                            On_ISocketRS(_XDXGInfoList[i]._XGInfo._IPAddress, true);
                        }
                    }
                }

            }
        }

        //定时发送命令任务
        private void timer1_Tick(object sender, EventArgs e)
        {
            rz_send_rask();
            xg_send_task(); 
        }
        
        //存储xd100n
        private void rz_save_task()
        {
            for (int i = 0; i < _XDRZInfoList.Length; i++)
            {
                if (_XDRZInfoList[i]._saveDatas == true)
                {
                    Tool.XD100n x = new Tool.XD100n(_XDRZInfoList[i]._station._deviceAddress);
                    x.InsertSql(_XDRZInfoList[i]);
                    _XDRZInfoList[i]._saveDatas = false;
                    return;
                }
            }
        }

        //存储xd300
        private void xg_save_task()
        {
            for (int i = 0; i < _XDXGInfoList.Length; i++)
            {
                if (_XDXGInfoList[i]._saveDatas == true)
                {
                    Tool.xd300 xg = new Tool.xd300(_XDXGInfoList[i]._XGInfo._deviceAddress);
                    xg.xgInsertSql(_XDXGInfoList[i]);
                    _XDXGInfoList[i]._saveDatas = false;
                    return;
                }
            }
        }


        //定时存储任务
        private void timer2_Tick(object sender, EventArgs e)
        {
            rz_save_task();
            xg_save_task();
        }

        //定时生成设置室外温度任务
        private void timer3_Tick(object sender, EventArgs e)
        {
            
            if (outtemp.enble == true)
            {
                TimeSpan delay = DateTime.Now - outtemp.dtlast;
                if (delay.Minutes >= outtemp.cycle)
                {
                    for (int i = 0; i < _XDRZInfoList.Length; i++)
                    {
                        if (_XDRZInfoList[i]._station._StationName == outtemp.station)
                        {
                            outtemp.value = _XDRZInfoList[i]._nowDatas._outsideTemp;
                            TimeSpan delay1=DateTime.Now - _XDRZInfoList[i]._nowDatas._dt;
                            //室外温度不是最新值
                            if (delay1.Minutes > 10)
                            {
                                return;
                            }
                            //室外温度超范围
                            if (outtemp.value > 50 && outtemp.value < -50)
                            {
                                return;
                            }

                            for (int j = 0; j < main._XDRZInfoList.Length; j++)
                            {
                                Tool.XD100n x = new Tool.XD100n(SocketServer.main._XDRZInfoList[j]._station._deviceAddress);
                                SocketServer.main._XDRZInfoList[j]._commandonce[16]._cmd = x.Set_outtemp_value(outtemp.value);
                                SocketServer.main._XDRZInfoList[j]._commandonce[16]._onoff = true;
                            }

                            break;
                        }
                    }
                    outtemp.dtlast = DateTime.Now;
                }
            }

            if (outtemp.enble == false)
            {
                string sql = "SELECT [id], [dt], [value] FROM [GYDB].[dbo].[tb_outtempdown_task] order by dt";
                DataTable dt = Tool.DB.getDt(sql);
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    DateTime tasktime = Convert.ToDateTime(dt.Rows[i]["dt"]);
                    if (tasktime.Hour == DateTime.Now.Hour && tasktime.Minute == DateTime.Now.Minute)
                    {
                        for (int j = 0; j < main._XDRZInfoList.Length; j++)
                        {
                            Tool.XD100n x = new Tool.XD100n(SocketServer.main._XDRZInfoList[j]._station._StationId);
                            SocketServer.main._XDRZInfoList[j]._commandonce[16]._cmd = x.Set_outtemp_value(Convert.ToSingle( dt.Rows[i]["value"]));
                            SocketServer.main._XDRZInfoList[j]._commandonce[16]._onoff = true;
                        }
                    }
                }
            }
        }

        //发送数据
        private bool rs_send(string ip, byte[] sendbuf)
        {
            ISocketRS rs = GetISocketRSlist(ip)._rs;
            _currentSocketRS = rs;
            try 
            {              
                _currentSocketRS.Send(sendbuf);
                return true;
            }
            catch
            {
                try
                {
                    _currentSocketRS.Close();
                }
                catch { }
                return false;
            }      
        }

        //xd100n数据处理
        private void rz_deal(byte[] rs ,int listid)
        {
            Tool.XD100n x = new Tool.XD100n(rs[0]);

            //实时数据模拟量 功能码0x04
            if (rs[1] == 0x04 && rs[2] == 0x64)
            {
                try
                {
                    Tool.XD100nData d = new Tool.XD100nData();
                    d = x.Read_nowdata(rs);
                    _XDRZInfoList[listid]._nowDatas = d;
                    _XDRZInfoList[listid]._command[0]._retrytimesnow = 0;
                    _XDRZInfoList[listid]._command[0]._sendover = false;
                }
                catch
                { }
            }
            //实时数据数字量 功能码0x02
            if (rs[1] == 0x02 && rs[2] == 0x04)
            {
                try
                {
                    Tool.GRPumpState ps = x.getpumpstate(rs);
                    Tool.GRAlarmData ad = x.getalarmstate(rs);
                    _XDRZInfoList[listid]._nowDatas._alarm = ad;
                    _XDRZInfoList[listid]._nowDatas._pump = ps;
                    _XDRZInfoList[listid]._saveDatas = true;
                    _XDRZInfoList[listid]._refDisplay = true;
                    _XDRZInfoList[listid]._command[1]._retrytimesnow = 0;
                    _XDRZInfoList[listid]._command[1]._sendover = false;
                }
                catch
                { }
            }

            //03可读返回
            if (rs[1] == 0x03)
            {
                //控制方法114
                if (_XDRZInfoList[listid].lastaddr == 114 - 1)
                {
                    _XDRZInfoList[listid]._Set._controltype = x.Read_controltype(rs);
                    _XDRZInfoList[listid]._commandonce[0]._back = true;
                    _XDRZInfoList[listid]._commandonce[0]._sendover = false;
                }
                //设定值116
                if (_XDRZInfoList[listid].lastaddr == 116 - 1)
                {
                    _XDRZInfoList[listid]._Set._setvalue = x.Read_setvalue(rs);
                    _XDRZInfoList[listid]._commandonce[1]._back = true;
                    _XDRZInfoList[listid]._commandonce[1]._sendover = false;
                }
                //曲线117
                if (_XDRZInfoList[listid].lastaddr == 117 - 1)
                {
                    _XDRZInfoList[listid]._Set._line = x.Read_line(rs);
                    _XDRZInfoList[listid]._commandonce[2]._back = true;
                    _XDRZInfoList[listid]._commandonce[2]._sendover = false;
                }
                //分时调整133
                if (_XDRZInfoList[listid].lastaddr == 133 - 1)
                {
                    _XDRZInfoList[listid]._Set._timepace = x.Read_timepace(rs);
                    _XDRZInfoList[listid]._commandonce[3]._back = true;
                    _XDRZInfoList[listid]._commandonce[3]._sendover = false;
                }
                //开度最大最小值145
                if (_XDRZInfoList[listid].lastaddr == 145 - 1)
                {
                    _XDRZInfoList[listid]._Set._valvemm = x.Read_valvemm(rs);
                    _XDRZInfoList[listid]._commandonce[4]._back = true;
                    _XDRZInfoList[listid]._commandonce[4]._sendover = false;
                }
                //流量限定153
                if (_XDRZInfoList[listid].lastaddr == 153 - 1)
                {
                    _XDRZInfoList[listid]._Set._valvelimit = x.Read_valvelimit(rs);
                    _XDRZInfoList[listid]._commandonce[5]._back = true;
                    _XDRZInfoList[listid]._commandonce[5]._sendover = false;
                }
                //报警198
                if (_XDRZInfoList[listid].lastaddr == 198 - 1)
                {
                    _XDRZInfoList[listid]._Set._alarm = x.Read_alarm(rs);
                    _XDRZInfoList[listid]._commandonce[6]._back = true;
                    _XDRZInfoList[listid]._commandonce[6]._sendover = false;
                }
                //室外温度206
                if (_XDRZInfoList[listid].lastaddr == 206 - 1)
                {
                    _XDRZInfoList[listid]._Set._outtemp = x.Read_outtemp(rs);
                    _XDRZInfoList[listid]._commandonce[7]._back = true;
                    _XDRZInfoList[listid]._commandonce[7]._sendover = false;
                }
                //刷新设置
                rz.rz_flowchart.refsetdisplay = 1;

            }
            //10写返回
            if (rs[1] == 0x10)
            {
                //控制方法114
                if (_XDRZInfoList[listid].lastaddr == 114 - 1)
                {
                    _XDRZInfoList[listid]._commandonce[8]._back = true;
                    _XDRZInfoList[listid]._commandonce[8]._sendover = false;

                }
                //设定值116
                if (_XDRZInfoList[listid].lastaddr == 116 - 1)
                {
                    _XDRZInfoList[listid]._commandonce[9]._back = true;
                    _XDRZInfoList[listid]._commandonce[9]._sendover = false;
                }
                //曲线117
                if (_XDRZInfoList[listid].lastaddr == 117 - 1)
                {
                    _XDRZInfoList[listid]._commandonce[10]._back = true;
                    _XDRZInfoList[listid]._commandonce[10]._sendover = false;
                }
                //分时调整133
                if (_XDRZInfoList[listid].lastaddr == 133 - 1)
                {
                    _XDRZInfoList[listid]._commandonce[11]._back = true;
                    _XDRZInfoList[listid]._commandonce[11]._sendover = false;
                }
                //开度最大最小值145
                if (_XDRZInfoList[listid].lastaddr == 145 - 1)
                {
                    _XDRZInfoList[listid]._commandonce[12]._back = true;
                    _XDRZInfoList[listid]._commandonce[12]._sendover = false;
                }
                //流量限定153
                if (_XDRZInfoList[listid].lastaddr == 153 - 1)
                {
                    _XDRZInfoList[listid]._commandonce[13]._back = true;
                    _XDRZInfoList[listid]._commandonce[13]._sendover = false;
                }
                //报警198
                if (_XDRZInfoList[listid].lastaddr == 198 - 1)
                {
                    _XDRZInfoList[listid]._commandonce[14]._back = true;
                    _XDRZInfoList[listid]._commandonce[14]._sendover = false;
                }
                //室外温度206
                if (_XDRZInfoList[listid].lastaddr == 206 - 1)
                {
                    _XDRZInfoList[listid]._commandonce[15]._back = true;
                    _XDRZInfoList[listid]._commandonce[15]._sendover = false;
                }
                //刷新设置
                rz.rz_flowchart.refsetdisplay = 1;
                
            }
            //急停返回
            if (rs[1] == 0x0F)
            {
                //循环泵启动返回
                if (rs[3] == 0x04)
                {
                    //古冶特定版本 2010-3-22验收
                    _XDRZInfoList[listid]._commandonce[17]._cmd = x.Set_xunhuan_stop();
                    _XDRZInfoList[listid]._commandonce[17]._onoff = true;
                    
                    _XDRZInfoList[listid]._commandonce[19]._back = true;
                    _XDRZInfoList[listid]._commandonce[19]._sendover = false;
                }
                //补水泵启动返回
                if (rs[3] == 0x06)
                {
                    _XDRZInfoList[listid]._commandonce[18]._cmd = x.Set_bushui_stop();
                    _XDRZInfoList[listid]._commandonce[18]._onoff = true;
                    _XDRZInfoList[listid]._commandonce[20]._back = true;
                    _XDRZInfoList[listid]._commandonce[20]._sendover = false;
                }

                //循环泵急停返回
                if (rs[3] == 0x05)
                {
                    _XDRZInfoList[listid]._commandonce[17]._back = true;
                    _XDRZInfoList[listid]._commandonce[17]._sendover = false;
                }
                //补水泵急停返回
                if (rs[3] == 0x07)
                {
                    _XDRZInfoList[listid]._commandonce[18]._back = true;
                    _XDRZInfoList[listid]._commandonce[18]._sendover = false;
                }

                //刷新设置
                rz.rz_flowchart.refsetdisplay = 1;
            }
            _XDRZInfoList[listid].state = true;
            //解除ISocketRS占用
            On_ISocketRS(_XDRZInfoList[listid]._station._IPAddress, false);
        }

        private string getperson(string card)
        {
            string person = "";
            for (int i = 0; i < _CardtoPersonlist.Length; i++)
            {
                if (card == _CardtoPersonlist[i]._card)
                {
                    person = _CardtoPersonlist[i]._person;
                    return person;
                }
            }
            return card;
        }

        //xd300数据处理
        private void xg_deal(byte[] rs, int xglistid)
        {
            Tool.xd300 xg = new Tool.xd300(rs[3]);
            //功能码 0x0a 主动上报
            if (rs[5] == 0x0a)
            {
                _XDXGInfoList[xglistid]._XGDataNow = xg.Read_record_n(rs);
                _XDXGInfoList[xglistid]._XGDataNow._person = getperson(_XDXGInfoList[xglistid]._XGDataNow._card);
                _XDXGInfoList[xglistid]._commandonce[1]._cmd = xg.Get_record_clean_now();
                _XDXGInfoList[xglistid]._commandonce[1]._onoff = true;
                _XDXGInfoList[xglistid]._commandonce[1]._timeoutnow = 0;
                _XDXGInfoList[xglistid]._commandonce[1]._sendover = false;
                _XDXGInfoList[xglistid]._saveDatas = true;
                _XDXGInfoList[xglistid]._refDisplay = true;
            }
            //功能码 0x06 读取记录总条数
            if (rs[5] == 0x06)
            {
                _XDXGInfoList[xglistid]._count = xg.Read_record_count(rs);
                if (_XDXGInfoList[xglistid]._count > 0)
                {
                    _XDXGInfoList[xglistid]._commandonce[0]._cmd = xg.Get_record_n(1);
                    _XDXGInfoList[xglistid]._commandonce[0]._onoff = true;
                    _XDXGInfoList[xglistid]._commandonce[0]._timeoutnow = 0;
                    _XDXGInfoList[xglistid]._commandonce[0]._sendover = false;
                }
            }
            //功能码 0x07 读取第N条记录
            if (rs[5] == 0x07)
            {
                _XDXGInfoList[xglistid]._XGDataNow = xg.Read_record_n(rs);
                _XDXGInfoList[xglistid]._XGDataNow._person = getperson(_XDXGInfoList[xglistid]._XGDataNow._card);
                if (_XDXGInfoList[xglistid]._XGDataNow._n < _XDXGInfoList[xglistid]._count)
                {
                    //读下条记录
                    _XDXGInfoList[xglistid]._commandonce[0]._cmd = xg.Get_record_n(_XDXGInfoList[xglistid]._XGDataNow._n + 1);
                    _XDXGInfoList[xglistid]._commandonce[0]._onoff = true;
                    _XDXGInfoList[xglistid]._commandonce[0]._timeoutnow = 0;
                    
                }
                if (_XDXGInfoList[xglistid]._XGDataNow._n == _XDXGInfoList[xglistid]._count && _XDXGInfoList[xglistid]._count>0)
                {
                    //清除总记录
                    _XDXGInfoList[xglistid]._commandonce[2]._cmd = xg.Get_record_clean();
                    _XDXGInfoList[xglistid]._commandonce[2]._onoff = true;
                    _XDXGInfoList[xglistid]._count = 0;

                }
                _XDXGInfoList[xglistid]._saveDatas = true;
                _XDXGInfoList[xglistid]._refDisplay = true;
                _XDXGInfoList[xglistid]._command[0]._retrytimesnow = 0;
                _XDXGInfoList[xglistid]._commandonce[0]._sendover = false;
            }
            //功能码 0x08 清除记录
            if (rs[5] == 0x08)
            {
                _XDXGInfoList[xglistid]._commandonce[2]._sendover = false;
            }
            //功能码 0x01 修改日期
            if (rs[5] == 0x01)
            {
                _XDXGInfoList[xglistid]._commandonce[3]._sendover = false;
            }
            //功能码 0x02 修改时间
            if (rs[5] == 0x01)
            {
                _XDXGInfoList[xglistid]._commandonce[4]._sendover = false;
            }

            _XDXGInfoList[xglistid].state = true;
            On_ISocketRS(_XDXGInfoList[xglistid]._XGInfo._IPAddress, false);
        }

        //接收事件
        private void rs_ReceivedEvent( object sender, EventArgs e )
        {            
            ISocketRS rs = sender as ISocketRS;

            //心跳包
            if (rs.ReceivedBytes[0] == 123)
            {
                return;
            }
            //不合校验
            if (Tool.DataInfo.check_crc(rs.ReceivedBytes) == false)
            {
                return;
            }

            string ip = ((IPEndPoint)rs.Socket.RemoteEndPoint).Address.ToString();

            //控制器
            Tool.XD100n x = new Tool.XD100n(rs.ReceivedBytes[0]);
            int listid = -1;
            for (int i = 0; i < _XDRZInfoList.Length; i++)
            {                
                if (_XDRZInfoList[i]._station._IPAddress == ip && _XDRZInfoList[i]._station._deviceAddress == rs.ReceivedBytes[0])
                {
                    listid = i;
                    break;
                }
            }
            if (listid != -1)
            {
                rz_deal(rs.ReceivedBytes, listid);                
            }

            //巡更
            Tool.xd300 xg = new Tool.xd300(rs.ReceivedBytes[3]);
            int xglistid = -1;
            for (int i = 0; i < _XDXGInfoList.Length; i++)
            {
                if (_XDXGInfoList[i]._XGInfo._IPAddress == ip && _XDXGInfoList[i]._XGInfo._deviceAddress == rs.ReceivedBytes[3] && rs.ReceivedBytes[0] == 0x21 && rs.ReceivedBytes[1] == 0x58 && rs.ReceivedBytes[2] == 0x44)
                {
                    xglistid = i;
                    break;
                }
            }
            if (xglistid != -1)
            {
                xg_deal(rs.ReceivedBytes, xglistid);
            }
        }




        //关闭连接事件
        private void rs_ClosedEvent( object sender, EventArgs e )
        {
            ISocketRS rs = sender as ISocketRS;
            this._socketRSs.Remove(rs);
            RemoveISocketRS(rs);
        }

       //移除连接
        private void RemoveISocketRS( ISocketRS rs )
        {
            rs.ReceivedEvent -= new EventHandler(rs_ReceivedEvent); ;
            rs.ClosedEvent -= new EventHandler(rs_ClosedEvent);
            this._socketRSs.Remove(rs);

            for (int i = 0; i < _ISocketRSlist.Length; i++)
            {
                if (_ISocketRSlist[i]._rs == rs)
                {
                    _ISocketRSlist[i]._rs = null;
                    _ISocketRSlist[i]._Iscon = false;
                    break;
                }
            }
        }
        #endregion

        //功能菜单
        #region
        //检测子窗体是否运行
        private bool checkChildFrmExist(string childFrmText)
        {
            try
            {
                foreach (Form childFrm in this.MdiChildren)
                {
                    if (childFrm.Text == childFrmText)
                    {
                        if (childFrm.WindowState == FormWindowState.Minimized)
                            childFrm.WindowState = FormWindowState.Maximized;
                        childFrm.Show();
                        childFrm.Activate();
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                MessageBox.Show("程序导入错误,请重新启动程序!");
                return false;
            }
        }
        //供热系统
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(null, null);
        }

        //背景
        private void pictureBox6_DoubleClick(object sender, EventArgs e)
        {
            if (this.checkChildFrmExist("背景") == true)
            {
                return;
            }
            Form f = new background();
            f.Text = "背景";
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        //地理信息
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(null, null);
        }
        //曲线
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            toolStripDropDownButton2_Click(null,null);
        }

        //管网示意
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            toolStripDropDownButton1_Click(null, null);
        }
        //巡更系统
        private void label9_Click(object sender, EventArgs e)
        {
            toolStripButton6_Click(null, null);
        }

        #endregion

        //窗口移动
        #region move
        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mousePosition.X = e.X;
                this.mousePosition.Y = e.Y;
            }
        }
        private void pictureBox7_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Top = Control.MousePosition.Y - mousePosition.Y;
                this.Left = Control.MousePosition.X - mousePosition.X;
            }
        }
        #endregion

        //模拟系统最大小化和关闭按钮
        #region systembutton
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //if (this.WindowState == FormWindowState.Maximized)
            //{
            //    this.WindowState = FormWindowState.Normal;
            //}
            //else
            //{
            //    this.WindowState = FormWindowState.Maximized;
            //}         
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        
        //树形结构选择显示的站点
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //数据按站stationid定时刷新
            if (treeView1.SelectedNode.Tag != null)
            {
                rz.rz_flowchart.stationid = Convert.ToInt32(treeView1.SelectedNode.Tag);
            }
            else
            {
                return;
            }

            if (this.checkChildFrmExist("流程图") == true)
            {
                return;
            }
            Form f = new rz.rz_flowchart();
            f.MdiParent = this;
            f.Text = "流程图";
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                treeView1.SelectedNode = treeView1.TopNode;
            }
        }

        //地理信息
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.checkChildFrmExist("地理信息") == true)
            {
                return;
            }
            Form f = new btGR.frmGisMain();
            f.Text = "地理信息";
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        //管网示意
        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            //if (this.checkChildFrmExist("管网图") == true)
            //{
            //    return;
            //}
            //Form f = new branch();
            //// Form f = new btGR.frmGisMain();
            //f.Text = "管网图";
            //f.MdiParent = this;
            //f.WindowState = FormWindowState.Maximized;
            //f.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.checkChildFrmExist("供热系统") == true)
            {
                return;
            }
            Form f = new rz.rz_main();
            f.Text = "供热系统";
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {
            if (this.checkChildFrmExist("曲线分析") == true)
            {
                return;
            }
            Form f = new curve();
            f.Text = "曲线分析";
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }


        //巡更系统
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (this.checkChildFrmExist("巡更系统") == true)
            {
                return;
            }
            Form f = new xg.xg_main();
            f.Text = "巡更系统";
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            toolStripButton6_Click(null, null);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            toolStripDropDownButton3_Click(null,null);
        }
        //系统管理
        private void toolStripDropDownButton3_Click(object sender, EventArgs e)
        {
            Form f1 = new message();
            if (f1.ShowDialog() == DialogResult.OK)
            {
                if (this.checkChildFrmExist("系统管理") == true)
                {
                    return;
                }
                Form f = new systemmanage();
                f.Text = "系统管理";
                f.MdiParent = this;
                f.WindowState = FormWindowState.Maximized;
                f.Show();
            }
        }
        //托盘图标单击
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            int alarm_count = -1;
            string alarm_string = "";
            for (int i = 0; i < _XDRZInfoList.Length; i++)
            {
                if (_XDRZInfoList[i]._nowDatas._alarm._all == Tool.GRAlarm.有)
                {
                    alarm_count++;
                    alarm_string += _XDRZInfoList[i]._station._StationName + Environment.NewLine;
                }
            }

            if (alarm_count == -1)
            {
                alarm_string = "无报警信息，所有机组运行正常！";
                this.notifyIcon1.ShowBalloonTip(1000, "提示", alarm_string, ToolTipIcon.Info);
            }
            else
            {
                alarm_count++;
                alarm_string += "共计有 " + alarm_count.ToString() + " 套机组报警！";
                this.notifyIcon1.ShowBalloonTip(1000, "警告！", alarm_string, ToolTipIcon.Warning);
            } 
        }
        //托盘图标双击
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
        //显示报警提示
        private void timer4_Tick(object sender, EventArgs e)
        {
            if (自动显示报警ToolStripMenuItem.Checked == false)
            {
                return;
            }
            int alarm_count = -1;
            string alarm_string = "";
            for (int i = 0; i < _XDRZInfoList.Length; i++)
            {
                if (_XDRZInfoList[i]._nowDatas._alarm._all == Tool.GRAlarm.有)
                {
                    alarm_count++;
                    alarm_string += _XDRZInfoList[i]._station._StationName + Environment.NewLine;
                }
            }

            if (alarm_count == -1)
            {
                return;
            }
            else
            {
                alarm_count++;
                alarm_string +=  "共计有 " + alarm_count.ToString() + " 套机组报警！";
                this.notifyIcon1.ShowBalloonTip(1000, "警告！", alarm_string, ToolTipIcon.Warning);
            } 
        }

        private void 自动显示报警ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (自动显示报警ToolStripMenuItem.Checked == false)
            {
                自动显示报警ToolStripMenuItem.Checked = true;
                return;
            }
            if (自动显示报警ToolStripMenuItem.Checked == true)
            {
                自动显示报警ToolStripMenuItem.Checked = false;
                return;
            }
        }


    }

}

