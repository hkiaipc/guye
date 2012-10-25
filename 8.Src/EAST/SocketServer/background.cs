using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SocketServer
{
    public partial class background : Form
    {
        public background()
        {
            InitializeComponent();
        }
        ShockwaveFlashObjects.ShockwaveFlash axShockwaveFlash1 = new ShockwaveFlashObjects.ShockwaveFlash();

        private void background_Load(object sender, EventArgs e)
        {
             axShockwaveFlash1.Movie  = Application.StartupPath + "\\kailuananniu2.swf"; 
             axShockwaveFlash1.Play(); 
        }
    }
}
