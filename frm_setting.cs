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
    public partial class frm_setting : Form
    {
        public frm_setting()
        {
            InitializeComponent();
        }

        private void frm_setting_Load(object sender, EventArgs e)
        {
            txtDir.Text = Properties.Settings.Default.Setting;
        }

        private void txtShow_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(txtDir.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Setting = txtDir.Text;
            System.IO.StreamWriter swriter;
            swriter = System.IO.File.CreateText(txtDir.Text);
            swriter.Close();
            this.Close();
        }

        private void txtPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folBrw=new FolderBrowserDialog();

            if (folBrw.ShowDialog() == DialogResult.OK)
            {
                txtDir.Text = folBrw.SelectedPath;

            }
        }
    }
}
