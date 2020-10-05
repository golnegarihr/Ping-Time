using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Threading;
namespace ping
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool go = false;
        static Thread th = new Thread(new ThreadStart(pinging));
        static DataSet hosts_ds=new DataSet();
        static connection con = new connection();
        int index = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
           // if (hosts_ds.Tables["hosts_view"].Rows.Count ==null ) hosts_ds.Clear();
            RefreshList();
            DataGridViewImageColumn col = new DataGridViewImageColumn();
            col.HeaderText = "وضعیت";
            go = false; btnGo.BackColor = Color.LightSeaGreen;
            col.Name = "status";
            //dataGridView1.Columns.Add(col);
            //timer1.Enabled = true ;
        }
        private void RefreshList()
        {
            connection con = new connection();
            con.DatasetQuery("select * from hosts", "hosts_view", hosts_ds);
            DataColumn dcol = new DataColumn();
            DataGridViewImageColumn col = new DataGridViewImageColumn();
            dcol.DataType = typeof (Bitmap  );
            dcol.DefaultValue = imageList1.Images[1];
            dcol.ColumnName = ("status");
            if (hosts_ds.Tables["hosts_view"].Columns.Count<9)
            hosts_ds.Tables["hosts_view"].Columns.Add(dcol);
            dataGridView1.DataSource = hosts_ds.Tables["hosts_view"];
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.Columns[0].HeaderText = "عنوان";
            dataGridView1.Columns[0].Width = 200;
            dataGridView1.Columns[1].HeaderText = "نام هاست یا ip";
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[6].HeaderText = "ساعت";
            dataGridView1.Columns[6].Width = 120;
            dataGridView1.Columns[5].HeaderText = "زمان پاسخگویی";
            dataGridView1.Columns[5].Width = 80;
            dataGridView1.Columns[8].HeaderText = "وضعیت";
            dataGridView1.Columns[8].Width = 60;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["status"].Value = imageList1.Images[int.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString())];
                if (hosts_ds.Tables["hosts_view"].Rows[i][4].ToString() == hosts_ds.Tables["hosts_view"].Rows[i][7].ToString())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("\n خطا در ادامه روند برقراری ارتباط با");
                    sb.Append("\n " + hosts_ds.Tables["hosts_view"].Rows[i][1].ToString());
                    sb.Append("\n در زمان ");
                    sb.Append("\n " + hosts_ds.Tables["hosts_view"].Rows[i][6].ToString());
                    sb.Append("\n تعداد خطا ");
                    sb.Append("\n " + hosts_ds.Tables["hosts_view"].Rows[i][7].ToString());
                    notifyIcon1.ShowBalloonTip(3, hosts_ds.Tables["hosts_view"].Rows[i][0].ToString(),sb.ToString(), ToolTipIcon.Error);
                }
            }
            dataGridView1.Rows[index].Selected = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }


        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("این پنجره در حافظه در حال اجرا خواهد ماند");
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //pinging();
            hosts_ds.Tables["hosts_view"].Clear();
            RefreshList();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            frm_add add = new frm_add();
            add.ShowDialog();
            hosts_ds.Tables["hosts_view"].Clear();
            RefreshList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((dataGridView1.Rows.Count > 0) )
            {
                DialogResult r = new DialogResult();
                r = MessageBox.Show(this, "آیا مطمئن اید", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                if (r == DialogResult.Yes)
                {
                    string comm;
                    comm = "delete from hosts "
                         + "where IP_or_Host='" + dataGridView1.CurrentRow.Cells[1].Value + "'";
                    con.qurey(comm);
                }
                hosts_ds.Tables["hosts_view"].Clear();
                RefreshList();
            }
        }

        private void ccxzvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void goToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnGo_Click(this , null );
        }
        private static void pinging()
        {
            connection con = new connection();
            con.DatasetQuery("select * from hosts", "ping", hosts_ds);
            while (true)
            {
                System.Threading.Thread.Sleep(20);
                foreach (DataRow dr in hosts_ds.Tables["ping"].Rows)
                {
                    {
                        string adrres = dr[1].ToString(), replyTme = "0";
                        Byte  stat = 0;
                        int errcount;
                        try
                        {
                            errcount = int.Parse(dr[4].ToString()) + 1;
                            Ping ping = new Ping();
                            // Ping Remote Mashine
                            PingReply reply = ping.Send(adrres, int.Parse(dr[2].ToString()));
                            replyTme = reply.RoundtripTime.ToString();
                            if (reply.Status == IPStatus.Success)
                            
                                stat = 0;
                            
                            else stat = 1;
                        }
                        catch (Exception e)
                        {
                            replyTme = "0";
                            stat = 1;
                        }
                        finally
                        {
                            if (stat==0)
                                errcount = 0;
                            else 
                                errcount = int.Parse(dr[7].ToString()) + 1;
                            string com;
                            if (int.Parse(dr[4].ToString()) < errcount)
                            {
                                stat = 2;
                                String sb ;
                                sb = "lost connection " + dr[0].ToString() + " in date " + DateTime.Now.ToShortDateString() + " in Time " + DateTime.Now.ToLongTimeString()+"\n";
                                //System.IO.TextWriter tw = new System.IO.  StreamWriter(Properties.Settings.Default.Setting);
                                using (System.IO.StreamWriter tw=System.IO.File.AppendText(Properties.Settings.Default.Setting))
                                {
                                tw.WriteLine  ( sb);
                                tw.Close();
                                }
                            }
                            dr[7] = errcount;
                            com = "insert into ping_log (ip,reply_time,stat,ping_date,ping_time)"
                                + " values ('" + adrres + "'," + replyTme + "," + stat + ",'" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToLongTimeString() + "')";
                            con.qurey(com);
                            com = "update hosts"
                                + " set hosts.last_ping='" + replyTme + "',errs='"+errcount +"' ,stat=" + stat + " , hosts.time = '" + DateTime.Now.ToLongTimeString() + "'"
                                + " where IP_or_Host like '" + adrres + "'";
                            con.qurey(com);
                        }
                    }
                    //System.Threading.Thread.Sleep(100);
                }
            }
        }    
        

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (!go)
            {
                timer1.Enabled = true;
                th = new Thread(new ThreadStart(pinging));
                th.Start();

                btnGo.Text = "توقف ping";
                goToolStripMenuItem.Text = "توقف";
                go = true;
                btnGo.BackColor = Color.IndianRed;
            }
            else
            {
                th.Abort();
                th = null;
                btnGo.Text = "آغاز ping";
                timer1.Enabled = false;
                goToolStripMenuItem.Text = "شروع";
                go = false; btnGo.BackColor = Color.LightSeaGreen;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            detail detailFrm = new detail();
            detailFrm.ip = dataGridView1.Rows[index].Cells[1].Value.ToString() ;
            detailFrm.title = dataGridView1.Rows[index].Cells[0].Value.ToString();
            detailFrm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frm_edit editForm = new frm_edit();
            editForm.txtOnv.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            editForm.txtIp.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            editForm.txtTimeOut.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            editForm.txtErrCnt.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            editForm.ShowDialog();
            hosts_ds.Tables["hosts_view"].Clear();
            RefreshList();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(this, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button5_Click(this, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(this, null);
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(this, null);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void showconfig_Click(object sender, EventArgs e)
        {
            frm_setting settingFrm = new frm_setting();
            settingFrm.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.Setting );
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;
        }


    }
}
