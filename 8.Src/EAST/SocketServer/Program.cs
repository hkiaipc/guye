using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xdgk.Common;

namespace SocketServer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Xdgk.Common.Diagnostics.HasPreInstance())
            {
                NUnit.UiKit.UserMessage.DisplayFailure("程序已经运行!");
                return;
            }

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form f = new login();
            if (f.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new main());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ProcessUnhandleException(e.Exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ProcessUnhandleException(e.ExceptionObject as Exception);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        static void ProcessUnhandleException(Exception ex)
        {
            Xdgk.Common.ExceptionHandler.Handle(ex);
        }
    }
}