using System;

namespace Tool
{
    class xd300
    {
        private byte _sID = 0;

        public xd300(int stationID)
        {
            _sID = Convert.ToByte(stationID%256);
        }
        //获取记录总数 功能码0x06
        public byte[] Get_record_count()
        {
            byte[] outByte = new byte[9];
            outByte[0] = 0x21;
            outByte[1] = 0x58;
            outByte[2] = 0x44;
            outByte[3] = _sID;
            outByte[4] = 0xb0;
            outByte[5] = 0x06;
            outByte[6] = 0x00;
            return DataInfo.CRC16(outByte);
        }       

        //解析数据总数
        public int Read_record_count(byte[] inByte)
        {
            //成功后返回： ！＋X＋D＋地址＋设备类型＋功能码＋数据数（0x01）＋记录总条数＋CRC16
            int count=-1;
            count = inByte[7];
            return count;
        }

        //读取第N条记录 功能码0x07
        public byte[] Get_record_n(int n)
        {
            byte[] outByte = new byte[10];
            outByte[0] = 0x21;
            outByte[1] = 0x58;
            outByte[2] = 0x44;
            outByte[3] = _sID;
            outByte[4] = 0xb0;
            outByte[5] = 0x07;
            outByte[6] = 0x01;

            outByte[7] = Convert.ToByte(n%256);

            return DataInfo.CRC16(outByte);
        }

        //解析记录
        public XGDataNow Read_record_n(byte[] inByte)
        {
            //成功后返回： ！＋X＋D＋地址＋设备类型＋功能码＋数据数（0x10）＋第N条记录＋CRC16
            //如果N取值不正确，则返回：！＋X＋D＋地址＋设备类型＋功能码＋数据数（0x01）＋记录总条数＋CRC16
            //记录格式：记录号（两字节）＋秒＋分＋时＋日＋月＋年＋卡号（八字节）
            XGDataNow _xdata = new XGDataNow();
            _xdata._n = inByte[7]*256+inByte[8];
            _xdata._DT = "20" + inByte[14].ToString("x2") + "-" + inByte[13].ToString("x2") + "-" + inByte[12].ToString("x2") + " " + inByte[11].ToString("x2") + ":" + inByte[10].ToString("x2") + ":" + inByte[9].ToString("x2");
            string _card = inByte[15].ToString("x2") + inByte[16].ToString("x2") + inByte[17].ToString("x2") + inByte[18].ToString("x2") + inByte[19].ToString("x2") + inByte[20].ToString("x2") + inByte[21].ToString("x2") + inByte[22].ToString("x2");
            _xdata._card = _card;
            _xdata._person = _card;
            return _xdata;
        }

        //存储
        public void xgInsertSql(XDXGInfo bd)
        {
            string str = string.Format("INSERT INTO tb_xgrealdata (DT,XgID,card) VALUES ('{0}',{1},'{2}')",bd._XGDataNow._DT, bd._XGInfo._XgID, bd._XGDataNow._card);
            Tool.DB.runCmd(str);
        }

        //删除记录 功能码0x08
        public byte[] Get_record_clean()
        {
            byte[] outByte = new byte[9];
            outByte[0] = 0x21;
            outByte[1] = 0x58;
            outByte[2] = 0x44;
            outByte[3] = _sID;
            outByte[4] = 0xb0;
            outByte[5] = 0x08;
            outByte[6] = 0x00;
            return DataInfo.CRC16(outByte);
        }

        //删除当前记录 功能码0x0a
        public byte[] Get_record_clean_now()
        {
            byte[] outByte = new byte[9];
            outByte[0] = 0x21;
            outByte[1] = 0x58;
            outByte[2] = 0x44;
            outByte[3] = _sID;
            outByte[4] = 0xb0;
            outByte[5] = 0x0a;
            outByte[6] = 0x00;
            return DataInfo.CRC16(outByte);
        }

        //设置时间
        public byte[] Set_time()
        {
            byte[] outByte = new byte[12];
            outByte[0] = 0x21;
            outByte[1] = 0x58;
            outByte[2] = 0x44;
            outByte[3] = _sID;
            outByte[4] = 0xb0;
            outByte[5] = 0x01;
            outByte[6] = 0x03;
            outByte[7] = DataInfo.bytetoBCD(Convert.ToByte(DateTime.Now.Second));
            outByte[8] = DataInfo.bytetoBCD(Convert.ToByte(DateTime.Now.Minute));
            outByte[9] = DataInfo.bytetoBCD(Convert.ToByte(DateTime.Now.Hour));
            return DataInfo.CRC16(outByte);
        }

        //设置日期
        public byte[] Set_date()
        {
            byte[] outByte = new byte[12];
            outByte[0] = 0x21;
            outByte[1] = 0x58;
            outByte[2] = 0x44;
            outByte[3] = _sID;
            outByte[4] = 0xb0;
            outByte[5] = 0x01;
            outByte[6] = 0x03;
            outByte[7] = DataInfo.bytetoBCD(Convert.ToByte(DateTime.Now.Day));
            outByte[8] = DataInfo.bytetoBCD(Convert.ToByte(DateTime.Now.Month));
            outByte[9] = DataInfo.bytetoBCD(Convert.ToByte(DateTime.Now.Year % 100));

            return DataInfo.CRC16(outByte);
        }
    }

    public struct XDXGInfo
    {       
        public int _count;
        
        public XGInfo _XGInfo;
        public XGDataNow _XGDataNow;
        public bool _saveDatas;
        public bool _refDisplay;
        public bool state;
        public XGCommandcyc[] _command;
        public XGCommandonce[] _commandonce;
    }

    public struct XGInfo
    {
        public int _XgID;
        public string _XgName;
        public string _Remark;
        public string _DTUregister;
        public string _IPAddress;
        public int _deviceAddress;
        public int _cycle;
        public int _timeout;
        public int _retrytimes;
    }

    public struct XGDataNow
    {
        public string _DT;
        public int _n;
        public string _person;
        public string _card;
    }

    public struct XGCommandcyc
    {
        public byte[] _cmd;
        public DateTime _dt;
        public int _timeoutnow;
        public int _retrytimesnow;
        public bool _sendover;
        public bool _onoff;
    }

    public struct XGCommandonce
    {
        public byte[] _cmd;
        public bool _sendover;
        public int _timeoutnow;
        public bool _back;
        public bool _onoff;
    }

}
