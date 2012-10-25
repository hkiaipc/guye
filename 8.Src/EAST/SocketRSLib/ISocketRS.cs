using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace SocketRSLib
{  
    /// <summary>
    /// 
    /// </summary>
    public enum RSType
    {
        R,
        S,
    }


    /// <summary>
    /// 
    /// </summary>
    public struct RSRecord
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rst"></param>
        /// <param name="dt"></param>
        /// <param name="text"></param>
        public RSRecord( RSType rst, DateTime dt, string text )
        {
            this.rsType = rst;
            this.dt = dt;
            this.text = text;
        }

        public RSType rsType;
        public string text;
        public DateTime dt;
    }


    /// <summary>
    /// 
    /// </summary>
    public interface ISocketRS
    {
        /// <summary>
        /// 
        /// </summary>
        Socket Socket
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        List<RSRecord> RSRecords
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        byte[] ReceivedBytes
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        void Send(byte[] buffer);

        /// <summary>
        /// 
        /// </summary>
        void Close();

        /// <summary>
        /// 
        /// </summary>
        event EventHandler ReceivedEvent;

        event EventHandler ClosedEvent;
    }
}
