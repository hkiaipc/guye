using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace SocketRSLib
{
    public class SocketRS : ISocketRS
    {
        /// <summary>
        /// 
        /// </summary>
        public Socket Socket
        {
            get { return _socket; }
        } private Socket _socket;

        private Thread _thread;

        /// <summary>
        /// 
        /// </summary>
        private bool _threadStarted;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public SocketRS(Socket socket)
        {
            _socket = socket;

            ThreadStart ts = new ThreadStart(ThreadCallback);
            _thread = new Thread(ts);

            // start thread
            //
            _threadStarted = true;
            _thread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ThreadCallback()
        {
            while (_threadStarted)
            {
                byte[] buffer = new byte[RECEIVE_BUFFER_SIZE];
                int receivedCount = 0;
                try
                {
                    receivedCount = _socket.Receive(buffer);
                }
                catch (SocketException sckEx)
                {
                    Console.WriteLine(sckEx.Message);
                    this.StopThread();
                }
                if (receivedCount > 0)
                {
                    if (this.ReceivedEvent != null)
                    {
                        this._receivedBytes = new byte[receivedCount];
                        Array.Copy(buffer, _receivedBytes, receivedCount);
                        this.ReceivedEvent(this, EventArgs.Empty);
                    }
                }
                else
                {
                    this.CloseSocket();
                    this.StopThread();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private const int RECEIVE_BUFFER_SIZE = 1024;
        public event EventHandler ReceivedEvent;
        public event EventHandler ClosedEvent;

        #region Properties 

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
        #endregion //

        /// <summary>
        /// 
        /// </summary>
        public void CloseSocket()
        {
            try
            {
                if (!_isClosed)
                {
                    _isClosed = true;
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();

                    if (this.ClosedEvent != null)
                    {
                        this.ClosedEvent(this, EventArgs.Empty);
                    }
                }
            }
            catch{}
        }

        bool _isClosed;

        /// <summary>
        /// 
        /// </summary>
        private void StopThread()
        {
            this._threadStarted = false;
            this._thread = null;

            // 
            //
            //if (this._thread != null &&
            //    this._thread.ThreadState != ThreadState.Aborted)
            //{
            //    this._thread.Abort();
            //}
        }

        /// <summary>
        /// user stop rs
        /// </summary>
        public void Close()
        {
            // shutdown close socket
            //CloseSocket();
            // thread abort
            ForceStop();

        }

        private void ForceStop()
        {
            this._thread.Abort();
            this._socket.Shutdown(SocketShutdown.Both);
            this._socket.Close();
        }

        public void Send(byte[] buffer)
        {
            this.Socket.Send(buffer);
        }
    }
}
