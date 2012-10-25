using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Tool
{
    public class SocketServer
    {


        #region Members 
        /// <summary>
        /// 
        /// </summary>
        private Socket _server = new Socket( AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp );
        private Thread _acceptThread;
        public event EventHandler NewConnectEvent;     
        private bool _isListening;

        #endregion //

        /// <summary>
        /// 
        /// </summary>
        public SocketServer()
        {
        }

        #region
        /// <summary>
        /// 
        /// </summary>
        public Socket NewSocket
        {
            get { return _newSocket; }
        } private Socket _newSocket;
        #endregion //


        //public ArrayList SocketRSList
        //{
        //    get { return _socketRsList; }
        //}
        //private ArrayList _socketRsList = new ArrayList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        public void Listen(string ip, int port)
        {
            IPAddress localIP = IPAddress.Parse(ip);
            EndPoint localEP = new IPEndPoint(localIP, port);
            _server.Bind(localEP);
            _server.Listen(100);

            _isListening = true;
            ThreadStart ts = new ThreadStart(Doit);
            _acceptThread = new Thread(ts);
            _acceptThread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Doit()
        {
            try
            {
                while (_isListening)
                {
                    _newSocket = _server.Accept();
                    if (this.NewConnectEvent != null)
                    {
                        NewConnectEvent(this, EventArgs.Empty);
                    }

                    //SocketRS rs = new SocketRS(sck);
                    //this._socketRsList.Add(rs);
                }
            }
            catch { }
        }

        public void Close()
        {
            //// close socket client 
            ////
            //foreach (object obj in this.SocketRSList)
            //{
            //    SocketRS rs = obj as SocketRS;
            //    //rs.Close();
            //}

            this._isListening = false;
            if (this._acceptThread != null &&
                this._acceptThread.ThreadState != ThreadState.Aborted)
            {
                this._acceptThread.Abort();
                this._acceptThread = null;
            }
            this._server.Close();
        }

    }
}
