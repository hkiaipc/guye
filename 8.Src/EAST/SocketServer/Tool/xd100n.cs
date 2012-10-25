using System;


namespace Tool
{
    class XD100n
    {
        private int _sID = 1;

        public XD100n(int stationID)
        {
            _sID = stationID;
        }

        //读取命令生成区域
        #region Get 
        //通用生成读取命令函数
        //byte function 功能码
        //int startaddress 起始地址
        //int endaddress 结束地址
        private byte[] getdata(byte function, int startaddress, int endaddress)
        {
            int length = endaddress - startaddress+1;
            byte[] outByte = new byte[8];
            byte[] temp = new byte[4];
            outByte[0] = byte.Parse(_sID.ToString());
            outByte[1] = function;
            temp = BitConverter.GetBytes(startaddress - 1);
            outByte[2] = temp[1];
            outByte[3] = temp[0];
            temp = BitConverter.GetBytes(length);
            outByte[4] = temp[1];
            outByte[5] = temp[0];
            return DataInfo.CRC16(outByte);
        }
        //获取当前数据模拟量
        public byte[] Get_nowdata()
        {
            return getdata(4,1,50);
        }
        //获取泵状态和报警
        public byte[] Get_di()
        {
            return getdata(2, 1, 32);
        }
        //获取调节阀控制模式
        public byte[] Get_controltype()
        {
            return getdata(3, 114, 114);
        }
        //获取调节阀设定值
        public byte[] Get_setvalue()
        {
            return getdata(3, 116, 116);
        }
        //获取调节阀温度曲线
        public byte[] Get_line()
        {
            return getdata(3, 117, 132);
        }
        //获取调节阀分时调整
        public byte[] Get_timepace()
        {
            return getdata(3, 133, 144);
        }
        //获取调节阀开度上下限
        public byte[] Get_valvemm()
        {
            return getdata(3,145,146);
        }
        //获取调节阀流量限定
        public byte[] Get_valvelimit()
        {
            return getdata(3, 153, 154);
        }
        //获取报警设置
        public byte[] Get_alarm()
        {
            return getdata(3, 198, 205);
        }
        //获取室外温度设置
        public byte[] Get_outtemp()
        {
            return getdata(3, 206, 207);
        }
        #endregion

        //设置命令生成区域
        #region Set 
        //通用设置命令
        //功能码固定为0x10
        //int startaddress 起始地址
        //int[] data 数据数组
        private byte[] setdata(int startaddress,int[] data)
        {
            byte[] outByte = new byte[9+data.Length*2];
            byte[] temp = new byte[4];
            outByte[0] = byte.Parse(_sID.ToString());
            outByte[1] = 0x10;
            temp = BitConverter.GetBytes(startaddress - 1);
            outByte[2] = temp[1];
            outByte[3] = temp[0];
            temp = BitConverter.GetBytes(data.Length);
            outByte[4] = temp[1];
            outByte[5] = temp[0];
            outByte[6] = Convert.ToByte(data.Length * 2);
            for (int i = 0; i < data.Length * 2; i++)
            {
                temp = BitConverter.GetBytes(data[i / 2]);
                if (i % 2 == 0)
                {
                    outByte[7 + i] = temp[1];
                }
                else
                {
                    int c = data[i / 2];
                    outByte[7 + i] = temp[0];
                }
            }
            return DataInfo.CRC16(outByte);
        }
        //设置类型 
        //0：温度曲线+二次供温
        //1：温度曲线+二次回温
        //2：温度曲线+二次供回水温差
        //3：温度设定+二次供温
        //4：温度设定+二次回温
        //5：温度设定+二次供回水温差
        //6：调节阀开度给定
        public byte[] Set_controltype(int Index)
        {
            int[] ind = { Index };
            return setdata(114,ind);
        }
        //设置二次供温
        public byte[] Set_setvalue(float value)
        {
            int[] va = { Convert.ToInt16(value * 10) };
            return setdata(116,va);
        }
        //设置调节阀温度曲线
        public byte[] Set_line(Set_line line)
        {
            int[] li = { line._ov1, line._gv1, line._ov2, line._gv2, line._ov3, line._gv3, line._ov4, line._gv4, line._ov5, line._gv5, line._ov6, line._gv6, line._ov7, line._gv7, line._ov8, line._gv8 };
            return setdata(117, li);
        }
        //设置调节阀分时调整
        public byte[] Set_timepace(Set_timepace tp)
        {
            int[] st = { Convert.ToInt16(tp._v1 * 10), Convert.ToInt16(tp._v2 * 10), Convert.ToInt16(tp._v3 * 10), Convert.ToInt16(tp._v4 * 10), Convert.ToInt16(tp._v5 * 10), Convert.ToInt16(tp._v6 * 10), Convert.ToInt16(tp._v7 * 10), Convert.ToInt16(tp._v8 * 10), Convert.ToInt16(tp._v9 * 10), Convert.ToInt16(tp._v10 * 10), Convert.ToInt16(tp._v11 * 10), Convert.ToInt16(tp._v12 * 10)};
            return setdata(133, st);
        }
        //设置调节阀开度上下限
        public byte[] Set_valvemm(Set_valvemm vm)
        {
            int[] mm = { vm._valvemin, vm._valvemax};
            return setdata(145, mm);
        }
        //设置调节阀流量限定
        public byte[] Set_valvelimit(Set_valvelimit sv)
        {
            int[] ef = { sv._enable, sv._valvelimit};
            return setdata(153, ef);
        }
        //设置报警设置
        public byte[] Set_alarm(Set_alarm sa)
        {
            int[] alarm = { Convert.ToInt16(sa._yicigdiya * 100), Convert.ToInt16(sa._erciggaoya * 100), Convert.ToInt16(sa._ercihgaoya * 100), Convert.ToInt16(sa._ercihdiya * 100), sa._yicigdiwen, sa._erciggaowen, Convert.ToInt16(sa._waterlow * 100), Convert.ToInt16(sa._waterhight * 100) };
            return setdata(198, alarm);
        }
        //设置室外温度设置
        public byte[] Set_outtemp(Set_outtemp sot)
        {
            int[] mt = {sot._outmode, Convert.ToInt16(sot._outtemp*10)};
            return setdata(206, mt);
        }
        //设置室外温度设置不包括模式
        public byte[] Set_outtemp_value(float _outtemp)
        {
            int[] mt = {Convert.ToInt16(_outtemp * 10) };
            return setdata(207, mt);
        }
        //循环泵急停
        public byte[] Set_xunhuan_stop()
        {
            // 01 0F 00 00 00 10 02 80 00 CRC
            byte[] outByte = new byte[10];
            outByte[0] = byte.Parse(_sID.ToString());
            outByte[1] = 0x0F;
            outByte[2] = 0x00;
            outByte[3] = 0x05;
            outByte[4] = 0x00;
            outByte[5] = 0x01;
            outByte[6] = 0x01;
            outByte[7] = 0x01;
            return DataInfo.CRC16(outByte);
        }
        //补水泵急停
        public byte[] Set_bushui_stop()
        {
            // 01 0F 00 00 00 10 02 00 40 CRC
            byte[] outByte = new byte[10];
            outByte[0] = byte.Parse(_sID.ToString());
            outByte[1] = 0x0F;
            outByte[2] = 0x00;
            outByte[3] = 0x07;
            outByte[4] = 0x00;
            outByte[5] = 0x01;
            outByte[6] = 0x01;
            outByte[7] = 0x01;
            return DataInfo.CRC16(outByte);
        }

        //循环泵启动
        public byte[] Set_xunhuan_start()
        {
            // 01 0F 00 00 00 10 02 80 00 CRC
            byte[] outByte = new byte[10];
            outByte[0] = byte.Parse(_sID.ToString());
            outByte[1] = 0x0F;
            outByte[2] = 0x00;
            outByte[3] = 0x04;
            outByte[4] = 0x00;
            outByte[5] = 0x01;
            outByte[6] = 0x01;
            outByte[7] = 0x01;
            return DataInfo.CRC16(outByte);
        }
        //补水泵启动
        public byte[] Set_bushui_start()
        {
            // 01 0F 00 00 00 10 02 00 40 CRC
            byte[] outByte = new byte[10];
            outByte[0] = byte.Parse(_sID.ToString());
            outByte[1] = 0x0F;
            outByte[2] = 0x00;
            outByte[3] = 0x06;
            outByte[4] = 0x00;
            outByte[5] = 0x01;
            outByte[6] = 0x01;
            outByte[7] = 0x01;
            return DataInfo.CRC16(outByte);
        }
        #endregion

        //数据解析区域
        #region Read
        //解析设置类型
        public int Read_controltype(byte[] inByte)
        { 
            return DataInfo.GetLongValue2(inByte, 3);
        }
        //解析设定值
        public float Read_setvalue(byte[] inByte)
        {
            return (float)Math.Round(DataInfo.GetLongValue2(inByte, 3)/10.0,1);
        }
        //解析曲线
        public Set_line Read_line(byte[] inByte)
        {
            Set_line sl = new Set_line();
            sl._ov1 = DataInfo.GetLongValue2(inByte, 3);
            sl._gv1 = DataInfo.GetLongValue2(inByte, 5);
            sl._ov2 = DataInfo.GetLongValue2(inByte, 7);
            sl._gv2 = DataInfo.GetLongValue2(inByte, 9);
            sl._ov3 = DataInfo.GetLongValue2(inByte, 11);
            sl._gv3 = DataInfo.GetLongValue2(inByte, 13);
            sl._ov4 = DataInfo.GetLongValue2(inByte, 15);
            sl._gv4 = DataInfo.GetLongValue2(inByte, 17);
            sl._ov5 = DataInfo.GetLongValue2(inByte, 19);
            sl._gv5 = DataInfo.GetLongValue2(inByte, 21);
            sl._ov6 = DataInfo.GetLongValue2(inByte, 23);
            sl._gv6 = DataInfo.GetLongValue2(inByte, 25);
            sl._ov7 = DataInfo.GetLongValue2(inByte, 27);
            sl._gv7 = DataInfo.GetLongValue2(inByte, 29);
            sl._ov8 = DataInfo.GetLongValue2(inByte, 31);
            sl._gv8 = DataInfo.GetLongValue2(inByte, 33);
            return sl;
        }
        //解析分时调整
        public Set_timepace Read_timepace(byte[] inByte)
        {
            Set_timepace st = new Set_timepace();
            st._v1 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 3)/ 10.0, 1);
            st._v2 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 5)/ 10.0, 1);
            st._v3 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 7)/ 10.0, 1);
            st._v4 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 9)/ 10.0, 1);
            st._v5 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 11)/ 10.0, 1);
            st._v6 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 13)/ 10.0, 1);
            st._v7 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 15)/ 10.0, 1);
            st._v8 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 17)/ 10.0, 1);
            st._v9 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 19)/ 10.0, 1);
            st._v10 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 21)/ 10.0, 1);
            st._v11 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 23)/ 10.0, 1);
            st._v12 = (float)Math.Round(DataInfo.GetLongValue2(inByte, 25) / 10.0, 1);
            return st;
        }
        //解析开度上下限
        public Set_valvemm Read_valvemm(byte[] inByte)
        {
            Set_valvemm sv = new Set_valvemm();
            sv._valvemin = DataInfo.GetLongValue2(inByte, 3);
            sv._valvemax = DataInfo.GetLongValue2(inByte, 5);
            return sv;
        }
        //解析流量限定
        public Set_valvelimit Read_valvelimit(byte[] inByte)
        {
            Set_valvelimit svl = new Set_valvelimit();
            svl._enable = DataInfo.GetLongValue2(inByte, 3);
            svl._valvelimit = DataInfo.GetLongValue2(inByte, 5);
            return svl;
        }
        //解析报警设置
        public Set_alarm Read_alarm(byte[] inByte)
        {
            Set_alarm sa = new Set_alarm();
            sa._yicigdiya = (float)Math.Round(DataInfo.GetLongValue2(inByte, 3) / 100.0, 1);
            sa._erciggaoya = (float)Math.Round(DataInfo.GetLongValue2(inByte, 5) / 100.0, 1);
            sa._ercihgaoya=(float)Math.Round(DataInfo.GetLongValue2(inByte, 7) / 100.0, 1);
            sa._ercihdiya=(float)Math.Round(DataInfo.GetLongValue2(inByte, 9) / 100.0, 1);
            sa._yicigdiwen=DataInfo.GetLongValue2(inByte, 11);
            sa._erciggaowen = DataInfo.GetLongValue2(inByte, 13);
            sa._waterlow=(float)Math.Round(DataInfo.GetLongValue2(inByte, 15) / 100.0, 1);
            sa._waterhight=(float)Math.Round(DataInfo.GetLongValue2(inByte, 17) / 100.0, 1);
            return sa;
        }
        //解析室外温度设置
        public Set_outtemp Read_outtemp(byte[] inByte)
        {
            Set_outtemp so = new Set_outtemp();
            so._outmode = DataInfo.GetLongValue2(inByte, 3);
            so._outtemp = (float)Math.Round(DataInfo.GetLongValue2(inByte, 5)/10.0,1);
            return so;
        }

        //解析实时数据
        public XD100nData Read_nowdata(byte[] inByte)
        {
            XD100nData _rdata = new XD100nData();
            _rdata._dt = DateTime.Now;
            _rdata._outsideTemp = (float)Math.Round(DataInfo.GetLongValue2(inByte, 15) / 10.0, 1);
            _rdata._oneGiveTemp = (float)Math.Round(DataInfo.GetLongValue2(inByte, 17) / 10.0, 1);
            _rdata._oneBackTemp = (float)Math.Round(DataInfo.GetLongValue2(inByte, 19) / 10.0, 1);
            _rdata._twoGiveTemp = (float)Math.Round(DataInfo.GetLongValue2(inByte, 21) / 10.0, 1);
            _rdata._twoBackTemp = (float)Math.Round(DataInfo.GetLongValue2(inByte, 23) / 10.0, 1);

            _rdata._oneGivePress = (float)Math.Round(DataInfo.GetLongValue2(inByte, 25) / 1000.0, 2);
            _rdata._oneBackPress = (float)Math.Round(DataInfo.GetLongValue2(inByte, 27) / 1000.0, 2);
            _rdata._twoGivePress = (float)Math.Round(DataInfo.GetLongValue2(inByte, 29) / 1000.0, 2);
            _rdata._twoBackPress = (float)Math.Round(DataInfo.GetLongValue2(inByte, 31) / 1000.0, 2);

            _rdata._openDegree = (float)Math.Round(DataInfo.GetLongValue2(inByte, 33) / 10.0, 1);
            _rdata._WatBoxLevel = (float)Math.Round(DataInfo.GetLongValue2(inByte, 35) / 100.0, 1);

            _rdata._oneInstant = (float)Math.Round(DataInfo.GetFloatValue2(inByte, 37), 1);
            _rdata._oneAccum = DataInfo.GetULongValue2(inByte, 41);

            _rdata._oneHeat = (float)Math.Round(DataInfo.GetFloatValue2(inByte, 45), 1);
            _rdata._oneAddHeat = DataInfo.GetULongValue2(inByte, 49) / 10d;

            _rdata._twoInstant = (float)Math.Round(DataInfo.GetFloatValue2(inByte, 65), 1);
            _rdata._twoAccum = DataInfo.GetULongValue2(inByte, 69);

            _rdata._twoHeat = (float)Math.Round(DataInfo.GetFloatValue2(inByte, 73), 1);
            _rdata._twoAddHeat = DataInfo.GetULongValue2(inByte, 77) / 10d;

            _rdata._subInstant = (float)Math.Round(DataInfo.GetFloatValue2(inByte, 53), 1);
            _rdata._subAccum = DataInfo.GetULongValue2(inByte, 57);

            byte statebyte = DataInfo.GetByteValue(inByte, 86);
            GRPumpState grPumpState = PumpParse(statebyte);
            _rdata._pump = grPumpState;
            byte[] warnbyte = new byte[2];
            warnbyte[1] = DataInfo.GetByteValue(inByte, 88);
            warnbyte[0] = DataInfo.GetByteValue(inByte, 87);
            GRAlarmData grAlarmData = AlarmParse(warnbyte);
            _rdata._alarm = grAlarmData;

            _rdata._twoGiveBaseTemp = (float)Math.Round(DataInfo.GetLongValue2(inByte, 97) / 10.0, 1);
            _rdata._twoPressCha = (float)Math.Round(DataInfo.GetLongValue2(inByte, 99) / 100.0, 2);
            _rdata._twoBackBasePress = (float)Math.Round(DataInfo.GetLongValue2(inByte, 101) / 100.0, 2);
            XDRZInfo xdrz = new XDRZInfo();
            xdrz._station._StationId = _sID;
            xdrz._nowDatas = _rdata;
            return _rdata;
        }

        public void InsertSql(XDRZInfo bd)
        {
            string str = string.Format("INSERT INTO tb_rzrealdata (StationId,DT,oneGiveTemp,oneBackTemp,twoGiveTemp,twoBackTemp,outsideTemp,twoGiveBaseTemp,oneGivePress,oneBackPress,WatBoxLevel,twoGivePress,twoBackPress,twoBackBasePress,oneInstant,oneAccum,subInstant,subAccum,oneHeat,oneAddHeat,twoInstant,twoAccum,twoHeat,twoAddHeat,twoPressCha,openDegree,pumpState1,pumpState2,pumpState3,addPumpState1,addPumpState2,alarmword) VALUES ({0},'{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},'{26}','{27}','{28}','{29}','{30}',{31})", 
                bd._station._StationId, bd._nowDatas._dt, bd._nowDatas._oneGiveTemp, bd._nowDatas._oneBackTemp, bd._nowDatas._twoGiveTemp, bd._nowDatas._twoBackTemp, bd._nowDatas._outsideTemp, bd._nowDatas._twoGiveBaseTemp, bd._nowDatas._oneGivePress, bd._nowDatas._oneBackPress, bd._nowDatas._WatBoxLevel, bd._nowDatas._twoGivePress, bd._nowDatas._twoBackPress, bd._nowDatas._twoBackBasePress, bd._nowDatas._oneInstant, bd._nowDatas._oneAccum, bd._nowDatas._subInstant, bd._nowDatas._subAccum, bd._nowDatas._oneHeat, bd._nowDatas._oneAddHeat, bd._nowDatas._twoInstant, bd._nowDatas._twoAccum, bd._nowDatas._twoHeat, bd._nowDatas._twoAddHeat, bd._nowDatas._twoPressCha, bd._nowDatas._openDegree, bd._nowDatas._pump._cycPump1, bd._nowDatas._pump._cycPump2, bd._nowDatas._pump._cycPump3, bd._nowDatas._pump._recruitPump1, bd._nowDatas._pump._recruitPump2,bd._nowDatas._alarm._word);
            Tool.DB.runCmd(str);
            if (bd._nowDatas._alarm._all == GRAlarm.有)
            {
                str = string.Format("INSERT INTO tb_rzalarmdata (StationId,DT,powercut,watboxdlow,watboxdhight,addPump2break,addPump1break,Pump3break,Pump2break,Pump1break,watboxalow,watboxahight,twoGiveTempH,oneGiveTempL,twoBackPressL,twoBackPressH,oneGivePressL) VALUES ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')", bd._station._StationId, bd._nowDatas._dt, bd._nowDatas._alarm._diaodian, bd._nowDatas._alarm._kaiguandi, bd._nowDatas._alarm._kaiguangao, bd._nowDatas._alarm._bushuibeng2, bd._nowDatas._alarm._bushuibeng1, bd._nowDatas._alarm._xunhuanbeng3, bd._nowDatas._alarm._xunhuanbeng2, bd._nowDatas._alarm._xunhuanbeng1, bd._nowDatas._alarm._shuiweidi, bd._nowDatas._alarm._shuiweigao, bd._nowDatas._alarm._ercigonggaowen, bd._nowDatas._alarm._yicigongdiwen, bd._nowDatas._alarm._ercihuidiya, bd._nowDatas._alarm._ercihuigaoya, bd._nowDatas._alarm._yicigongdiya);
                Tool.DB.runCmd(str);
            }
        }

        //位寄存器解析泵状态
        public GRPumpState getpumpstate(byte[] inByte)
        {
            byte pumpbyte = inByte[3];
            return PumpParse(pumpbyte);
        }
        //位寄存器解析报警
        public GRAlarmData getalarmstate(byte[] inByte)
        { 
            byte[] alarmbyte = {inByte[5],inByte[6]};
            return AlarmParse(alarmbyte);
        }
        //报警解析
        public GRAlarmData AlarmParse(byte[] alarm)
        {
            GRAlarmData gral = new GRAlarmData();
            if (alarm[0] != 0x00 || alarm[1] != 0x00)
            {
                gral._all = GRAlarm.有;
            }
            else
            {
                gral._all = GRAlarm.无;
            }
            gral._yicigongdiya = DataInfo.GetAlarmData(alarm[0], 0);
            gral._ercigonggaoya = DataInfo.GetAlarmData(alarm[0], 1);
            gral._ercihuigaoya = DataInfo.GetAlarmData(alarm[0], 2);
            gral._ercihuidiya = DataInfo.GetAlarmData(alarm[0], 3);
            gral._yicigongdiwen = DataInfo.GetAlarmData(alarm[0], 4);
            gral._ercigonggaowen = DataInfo.GetAlarmData(alarm[0], 5);
            gral._shuiweigao = DataInfo.GetAlarmData(alarm[0], 6);
            gral._shuiweidi = DataInfo.GetAlarmData(alarm[0], 7);

            gral._xunhuanbeng1 = DataInfo.GetAlarmData(alarm[1], 0);
            gral._xunhuanbeng2 = DataInfo.GetAlarmData(alarm[1], 1);
            gral._xunhuanbeng3 = DataInfo.GetAlarmData(alarm[1], 2);
            gral._bushuibeng1 = DataInfo.GetAlarmData(alarm[1], 3);
            gral._bushuibeng2 = DataInfo.GetAlarmData(alarm[1], 4);
            gral._kaiguangao = DataInfo.GetAlarmData(alarm[1], 5);
            gral._kaiguandi = DataInfo.GetAlarmData(alarm[1], 6);
            gral._diaodian = DataInfo.GetAlarmData(alarm[1], 7);
            gral._word = alarm[0] * 256 + alarm[1];
            return gral;
        }
        //泵状态解析
        private GRPumpState PumpParse(byte state)
        {
            GRPumpState grPs = new GRPumpState();
            grPs._cycPump1 = DataInfo.GetPumpState(state, 0);
            grPs._cycPump2 = DataInfo.GetPumpState(state, 1);
            grPs._cycPump3 = DataInfo.GetPumpState(state, 2);
            grPs._recruitPump1 = DataInfo.GetPumpState(state, 3);
            grPs._recruitPump2 = DataInfo.GetPumpState(state, 4);
            return grPs;
        }
        #endregion
    }

    public struct XD100nData
    {
        public float _oneGiveTemp;
        public float _oneBackTemp;
        public float _twoGiveTemp;
        public float _twoBackTemp;
        public float _openDegree;
        public float _outsideTemp;
        public float _twoGiveBaseTemp;
        public float _oneGivePress;
        public float _oneBackPress;
        public float _WatBoxLevel;
        public float _twoGivePress;
        public float _twoBackPress;
        public float _twoBackBasePress;
        public float _oneInstant;
        public ulong _oneAccum;
        public float _twoInstant;
        public ulong _twoAccum;
        public float _subInstant;
        public ulong _subAccum;
        public float _oneHeat;
        public double _oneAddHeat;
        public float _twoHeat;
        public double _twoAddHeat;
        public float _twoPressCha;
        public GRPumpState _pump;
        public GRAlarmData _alarm;
        public DateTime _dt;
    }
    public enum PumpState
    {
        启动 = 0,
        停止 = 1,
    }
    public enum GRAlarm
    {
        有 = 1,
        无 = 0,
    }
    public struct GRPumpState
    {
        public PumpState _cycPump1;
        public PumpState _cycPump2;
        public PumpState _cycPump3;
        public PumpState _recruitPump1;
        public PumpState _recruitPump2;
    }
    public struct GRAlarmData
    {
        public GRAlarm _all;
        public int _word;
        public GRAlarm _yicigongdiya;
        public GRAlarm _ercigonggaoya;
        public GRAlarm _ercihuigaoya;
        public GRAlarm _ercihuidiya;
        public GRAlarm _yicigongdiwen;
        public GRAlarm _ercigonggaowen;
        public GRAlarm _shuiweigao;
        public GRAlarm _shuiweidi;
        public GRAlarm _xunhuanbeng1;
        public GRAlarm _xunhuanbeng2;
        public GRAlarm _xunhuanbeng3;
        public GRAlarm _bushuibeng1;
        public GRAlarm _bushuibeng2;
        public GRAlarm _kaiguangao;
        public GRAlarm _kaiguandi;
        public GRAlarm _diaodian;
    }
    public struct XDRZInfo
    {
        public StationInfo _station;
        public bool _saveDatas;
        public bool _refDisplay;
        public bool state;
        public XD100nData _nowDatas;
        public Commandcyc[] _command;
        public Commandonce[] _commandonce;
        public int lastaddr;
        public Set _Set;
    }
    public struct StationInfo
    {
        public int _StationId;
        public string _StationName;       
        public string _group;
        public float _heatbase;
        public float _heatArea;
        public string _Remark;

        public string _DTUregister;
        public string _IPAddress;
        public int _deviceAddress;
        public int _cycle;
        public int _timeout;
        public int _retrytimes;
    }
    public struct Commandcyc
    //public class Commandcyc
    {
        public byte[] _cmd;
        public DateTime _dt;
        public int _timeoutnow;
        public int _retrytimesnow;
        public bool _sendover;
        public bool _onoff;
    }

    public struct Commandonce
    {
        public byte[] _cmd;
        public bool _sendover;
        public int _timeoutnow;
        public bool _back;
        public bool _onoff;
    }

    public struct Set
    {
        public int _controltype; //40114 控制模式
        //0：温度曲线+二次供温
        //1：温度曲线+二次回温
        //2：温度曲线+二次供回水温差
        //3：温度设定+二次供温
        //4：温度设定+二次回温
        //5：温度设定+二次供回水温差
        //6：调节阀开度给定
        public float _setvalue; //116 设定值
        public Set_line _line;       
        public Set_timepace _timepace;
        public Set_valvemm _valvemm;
        public Set_valvelimit _valvelimit;
        public Set_alarm _alarm;
        public Set_outtemp _outtemp; 
    }

        public struct Set_line
        {
            public int _ov1;
            public int _gv1;
            public int _ov2;
            public int _gv2;
            public int _ov3;
            public int _gv3;
            public int _ov4;
            public int _gv4;
            public int _ov5;
            public int _gv5;
            public int _ov6;
            public int _gv6;
            public int _ov7;
            public int _gv7;
            public int _ov8;
            public int _gv8;
            //117	温度曲线1段室外温度
            //118	温度曲线1段设定值
            //～		
            //131	温度曲线8段室外温度
            //132	温度曲线8段设定值
        }
        public struct Set_timepace
        {
            public float _v1;
            public float _v2;
            public float _v3;
            public float _v4;
            public float _v5;
            public float _v6;
            public float _v7;
            public float _v8;
            public float _v9;
            public float _v10;
            public float _v11;
            public float _v12;
            //133	字	分时调整1段
            //～		
            //144	字	分时调整12段
        }
        public struct Set_valvemm
        {
            public int _valvemax;
            public int _valvemin;
            //40145 最大开度
            //40146 最小开度
        }
        public struct Set_valvelimit
        {
            public int _enable;
            public int _valvelimit;
            //40153 流量限制使能
            //40154 限制流量
        }
        public struct Set_alarm
        {
            public float _yicigdiya;
            public float _erciggaoya;
            public float _ercihgaoya;
            public float _ercihdiya;
            public int _yicigdiwen;
            public int _erciggaowen;
            public float _waterlow;
            public float _waterhight;
            //40198	字	一次供低压报警
            //40199	字	二次供高压报警
            //40200	字	二次回高压报警
            //40201	字	二次回低压报警
            //40202	字	一次供低温报警
            //40203	字	二次供高温报警
            //40204	字	水箱水位低报警
            //40205	字	水箱水位高报警  
        }
        public struct Set_outtemp
        {
            public int _outmode;
            public float _outtemp; 
            //40206	字	室外温度，0：本地1：远程
            //40207	字	室外温度远程设定值
        }
}