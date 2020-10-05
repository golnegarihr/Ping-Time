using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
namespace ping
{
    public partial class frm_add : Form
    {
        public frm_add()
        {
            InitializeComponent();
        }
        connection con = new connection();
        bool exp1 = false;
        bool exp2 = false;
        private int ping_sta(string remoteMachineNameOrIP)
        {
            int i = 0;
            try
            {
                Ping ping = new Ping();
                // Ping Remote Mashine
                PingReply reply = ping.Send(remoteMachineNameOrIP);
                // Show Result
                StringBuilder sb = new StringBuilder();
                if (reply.Status == IPStatus.TimedOut)
                {
                    sb.Append("time out");
                    i = 0;
                }
                else
                {
                    sb.Append("آدرس : " + reply.Address.ToString());
                    sb.Append("\n وضعیت : " + reply.Status.ToString());
                    sb.Append("\n زمان پاشخگویی : " + reply.RoundtripTime.ToString() + " Ms              ");
                }
                MessageBox.Show(this, sb.ToString(), "Ping Result: " + remoteMachineNameOrIP, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
            }
            catch (Exception e)
            {
                MessageBox.Show(this, "پاسخی دریافت نشد!", "Ping Result: " + remoteMachineNameOrIP, MessageBoxButtons.OK, MessageBoxIcon.Error , MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                i = 2;
            }
            return i;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_add_Load(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
                ping_sta(txtIp.Text );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((txtOnv.Text != "") && (txtIp.Text != "") && (txtTimeOut.Text != "") &&((exp1!=true)||(exp2!=true)))
            {
                string comm;
                comm = "insert into hosts (title,IP_or_Host,tme_out,stat,cnt_err)"
                     + " values ('" + txtOnv.Text + "','" + txtIp.Text + "','" + txtTimeOut.Text + "',True,'" + txtErrCnt.Text + "')";
                con.qurey(comm);
                this.Close();
            }
            else MessageBox.Show("خطایی در داده های وارد شده وجود دارد");
        }

        private void txtTimeOut_Leave(object sender, EventArgs e)
        {
            if (int.Parse(txtTimeOut.Text) > 1000)
            {
                MessageBox.Show("مقدار وارد شده بزرگتر از 1000 است");
                exp1 = true;
            }
            else
                exp1 = false;
        }

        private void txtErrCnt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtErrCnt_Leave(object sender, EventArgs e)
        {
            if (int.Parse(txtErrCnt.Text)>255)
            {
                MessageBox.Show("مقدار وارد شده بزرگتر از 1000 است");
                exp2 = true;
            }
            else
                exp2 = false;
        }



        private void chkMob_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
