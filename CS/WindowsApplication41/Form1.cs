using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication41
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
                dataTable1.Rows.Add(new object[] { i*20});
            myProgressBarControl1.BackColor = Color.Tomato;
            myRepositoryItemProgressBar1.BarColor = Color.Thistle;
            myRepositoryItemProgressBar1.Appearance.BackColor = Color.Teal;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            myProgressBarControl1.EditValue = (int)myProgressBarControl1.EditValue + 10;
        }
    }
}