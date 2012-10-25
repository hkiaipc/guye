using System;

namespace Tool
{
    //数据解析方式
    public class DataInfo
    {
        public DataInfo()
        {
        }
        
        #region IsDataTypes
        static public bool CheckIPAddress(string ip)
        {
            try { System.Net.IPAddress _ip = System.Net.IPAddress.Parse(ip); return true; }
            catch { return false; }
        }

        static public bool CheckInt(string i)
        {
            try { int _i = int.Parse(i); return true; }
            catch { return false; }
        }

        static public bool CheckFloat(string f)
        {
            try { float _f = float.Parse(f); return true; }
            catch { return false; }
        }
        #endregion

        #region GetGRDataTypes
        static public float GetFloatValue(byte[] datas, int begin)//, int len )
        {
            return BitConverter.ToSingle(datas, begin);
        }
        static public ulong GetULongValue(byte[] datas, int begin)
        {
            return BitConverter.ToUInt32(datas, begin);
        }
        static public int GetLongValue(byte[] datas, int begin)
        {
            return BitConverter.ToInt16(datas, begin);
        }

        static public float GetFloatValue2(byte[] datas, int begin)//, int len )
        {
            byte[] data_c = { datas[begin + 1], datas[begin + 0], datas[begin + 3], datas[begin+2] };
            return BitConverter.ToSingle(data_c, 0);
        }
        static public ulong GetULongValue2(byte[] datas, int begin)
        {
            byte[] data_c = { datas[begin + 3], datas[begin + 2], datas[begin + 1], datas[begin] };
            return BitConverter.ToUInt32(data_c, 0);
        }
        static public int GetLongValue2(byte[] datas, int begin)
        {
            byte[] data_c = { datas[begin + 1], datas[begin] };
            return BitConverter.ToInt16(data_c, 0);
        }

        static public float GetCharValue(byte datas)
        {
            byte[] data = new byte[] { datas, Convert.ToByte("00", 16) };
            float fdata = BitConverter.ToChar(data, 0);
            if (fdata > 150)
            {
                fdata = fdata - 256;
                return fdata;
            }
            else
                return fdata;
        }
        static public byte GetByteValue(byte[] datas, int begin)
        {
            return datas[begin];
        }
        static public byte[] GetBitValue(float datas)
        {
            byte[] getBitValue = new byte[4];
            getBitValue = BitConverter.GetBytes(datas);
            return getBitValue;
        }
        static public byte[] GetBitValueInt(int datas)
        {
            byte[] getBitValue = new byte[4];
            getBitValue = BitConverter.GetBytes(datas);
            return getBitValue;
        }
        static public byte bytetoBCD(byte data)
        {
            if (data >= 10)
            {
                byte hexlo = Convert.ToByte( data % 10);
                byte hexhi = Convert.ToByte((data - hexlo) / 10);
                hexhi *= 16;
                byte bcd = Convert.ToByte(hexhi + hexlo);
                return bcd;
            }
            else
            {
                return data;
            }
        }

        static public byte BCDtobyte(byte data)
        {
            byte hexlo = Convert.ToByte(data % 16);
            byte hexhi = Convert.ToByte((data - hexlo) / 16);
            hexhi *= 10;
            byte bcd = Convert.ToByte(hexhi + hexlo);
            return bcd;
        }

        #endregion

        #region GetGRDatastate

        //获取泵状态
        static public PumpState GetPumpState(byte state, int bitIndex)
        {
            byte mask = (byte)Math.Pow(2, bitIndex);
            int r = mask & state;
            if (r > 0)
                return PumpState.启动;
            else
                return PumpState.停止;
        }

        //获取报警状态
        static public GRAlarm GetAlarmData(byte state, int bitIndex)
        {
            byte mask = (byte)Math.Pow(2, bitIndex);
            int r = mask & state;
            if (r > 0)
                return GRAlarm.有;
            else
                return GRAlarm.无;
        }
        #endregion

        //计算CRC
        static public byte[] CRC16(Byte[] data)
        {
            if (data == null || data.Length <= 2)
            {
                if (data != null)
                {
                    Console.WriteLine(BitConverter.ToString(data));
                }
                return data;
            }

            byte CRC16Lo = 0xff, CRC16Hi = 0xff;
            byte CL = 0x1, CH = 0xA0;
            byte SaveHi, SaveLo;
            for (int i = 0; i < data.Length - 2; i++)
            {
                CRC16Lo ^= data[i];
                for (int Flag = 0; Flag < 8; Flag++)
                {
                    SaveHi = CRC16Hi;
                    SaveLo = CRC16Lo;
                    CRC16Hi >>= 1;
                    CRC16Lo >>= 1;
                    if ((SaveHi & 0x01) == 0x01)
                    {
                        CRC16Lo |= 0x80;
                    }
                    if ((SaveLo & 0x1) == 0x1)
                    {
                        CRC16Hi ^= CH;
                        CRC16Lo ^= CL;
                    }
                }
            }
            data[data.Length - 2] = CRC16Lo;
            data[data.Length - 1] = CRC16Hi;
            return data;
        }

        //校验crc
        static public bool check_crc(byte[] data)
        {
            if (data == null || data.Length <= 2)
            {
                return false;
            }

            byte[] temp = new byte[data.Length];
            for (int i = 0; i < data.Length - 2; i++)
            {
                temp[i] = data[i];
            }
            byte[] crc = CRC16(temp);
            if (crc[crc.Length - 2] == data[data.Length - 2] && crc[crc.Length - 1] == data[data.Length - 1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
