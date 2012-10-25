using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketRSLib
{
    public class SocketRSAPM : ISocketRS
    {
        public Socket _socket;
        byte[] _receBuffer = new byte[1024];

        public SocketRSAPM(Socket s)
        {
            _socket = s;
            receive();
        }

        #region Socket
        /// <summary>
        /// 
        /// </summary>
        public Socket Socket
        {
            get { return _socket; }
        }
        #endregion

        #region Send

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public void Send(byte[] buffer)
        {
            _socket.Send(buffer);

            AddRSData(RSType.S, buffer);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void receive()
        {
            AsyncCallback cb = new AsyncCallback(callback);
            try
            {

                IAsyncResult ia = _socket.BeginReceive(_receBuffer, 0, 1024, SocketFlags.None, cb, _socket);
            }
            catch { }

        }

        #region RSRecords
        public List<RSRecord> RSRecords
        {
            get { return _rsRecords; }
        } private List<RSRecord> _rsRecords = new List<RSRecord>();
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public byte[] ReceivedBytes
        {
            get { return _receivedBytes; }
        } private byte[] _receivedBytes;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ia"></param>
        void callback(IAsyncResult ia)
        {
            Socket sck = ia.AsyncState as Socket;
            int n = 0;
            try
            {
                n = sck.EndReceive(ia);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (SocketException sckEx)
            {
                Console.WriteLine(sckEx.ErrorCode.ToString() + " " + sckEx.Message);
                this.CloseHelper();
                return;
            }

            if (n > 0)
            {
                _receivedBytes = new byte[n];
                Array.Copy(_receBuffer, _receivedBytes, n);
                AddRSData(RSType.R, _receivedBytes);
                if (this.ReceivedEvent != null)
                    ReceivedEvent(this, EventArgs.Empty);

                receive();
            }
            else
            {
                CloseHelper();
            }
        }


        #region AddRSData
        private void AddRSData(RSType rstype, byte[] buffer)
        {
            string text = ASCIIEncoding.ASCII.GetString(buffer);
            RSRecord record = new RSRecord(rstype, DateTime.Now, text);
            this._rsRecords.Add(record);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void CloseHelper()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            if (this.ClosedEvent != null)
                this.ClosedEvent(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ReceivedEvent;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ClosedEvent;

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            CloseHelper();
        }
    }
}
