using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ping
{
    public partial class detail : Form
    {
        public detail()
        {
            InitializeComponent();
        }
        DataSet hosts_ds = new DataSet();
        public string ip,title;
        private void timer1_Tick(object sender, EventArgs e)
        {
            list_refresh();
        }
        private void list_refresh()
        {
            hosts_ds.Clear();
            connection con = new connection();
            con.DatasetQuery("select  top "+numericUpDown1.Value +" id,reply_time,ping_date,ping_time,stat from ping_log where ip like '" + ip + "' order by id desc", "log", hosts_ds);
            DataColumn dcol = new DataColumn();
            DataGridViewImageColumn col = new DataGridViewImageColumn();
            dcol.DataType = typeof(Bitmap);
            dcol.DefaultValue = imageList1.Images[1];
            dcol.ColumnName = ("status");
            if (hosts_ds.Tables["log"].Columns.Count < 6)
                hosts_ds.Tables["log"].Columns.Add(dcol);
            dataGridView1.DataSource = hosts_ds.Tables["log"];
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "زمان پاسخ";
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].HeaderText = "تاریخ";
            dataGridView1.Columns[3].HeaderText = "ساعت";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].HeaderText = "وضعیت";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                    dataGridView1.Rows[i].Cells["status"].Value = imageList1.Images[int.Parse (dataGridView1.Rows[i].Cells[4].Value.ToString())];
            }
        }
        private void detail_Load(object sender, EventArgs e)
        {
            label2.Text = ip;
            label4.Text = title;
            list_refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            this.Close();
        }
    }
}
