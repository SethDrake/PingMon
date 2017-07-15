using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingMon
{
    public partial class MainFrm : Form
    {
        private Ping pingSender;
        private Icon[] icons;

        public MainFrm()
        {
            InitializeComponent();
            this.Hide();
            pingSender = new Ping();
            icons = new Icon[] {
                Properties.Resources.green,
                Properties.Resources.yellow,
                Properties.Resources.red
            };
            runPing();
        }

        private void runPing()
        {
            try
            {
                PingReply reply = pingSender.Send("8.8.8.8");
                if (reply.Status == IPStatus.Success)
                {
                    long pingValue = reply.RoundtripTime;
                    lblStatus.Text = String.Format("Ping: {0} ms", pingValue);
                    notifyIcon.Text = lblStatus.Text;
                    if (pingValue <= 70)
                    {
                        lblStatus.ForeColor = Color.Green;
                        notifyIcon.Icon = icons[0];
                    }
                    else if (pingValue <= 160)
                    {
                        lblStatus.ForeColor = Color.DarkOrange;
                        notifyIcon.Icon = icons[1];
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        notifyIcon.Icon = icons[2];
                    }
                }
                else
                {
                    setUnavailableMode();
                }
            }
            catch (Exception)
            {
                setUnavailableMode();
            }
        }

        private void setUnavailableMode()
        {
            lblStatus.Text = "Unavailable";
            notifyIcon.Text = lblStatus.Text;
            lblStatus.ForeColor = Color.Red;
            notifyIcon.Icon = icons[2];
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            runPing();
        }

        private void MainFrm_MinimumSizeChanged(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.Visible)
            {
                this.Show();
            } else
            {
                this.Hide();
            }
        }
    }
}
