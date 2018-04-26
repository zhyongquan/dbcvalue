using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dbcvalue
{
    public partial class FrmDBC : Form
    {
        private List<NumericUpDown> listNumByte = new List<NumericUpDown>();
        private byte[] Data = new byte[8];
        public FrmDBC()
        {
            InitializeComponent();
        }

        private void FrmDBC_Load(object sender, EventArgs e)
        {
            cmbByteOrder.SelectedIndex = 0;
            listNumByte.Clear();
            listNumByte.Add(numByte0);
            for (int i = 1; i < 8; i++)
            {
                NumericUpDown ctrl = new NumericUpDown();
                ctrl.Name = "numByte" + i.ToString();
                ctrl.Maximum = byte.MaxValue;
                ctrl.Location = new Point(numByte0.Location.X + i * 60, numByte0.Location.Y);
                ctrl.Size = numByte0.Size;
                ctrl.ValueChanged += numByte0_ValueChanged;
                grpMessage.Controls.Add(ctrl);
            }
            for (int i = 0; i < 8; i++)
            {
                DataGridViewColumn col = new DataGridViewColumn();
                col.Name = i.ToString();
                col.HeaderText = (7 - i).ToString();
                col.Width = 30;
                dgvLayout.Columns.Add(col);
            }
            dgvLayout.Rows.Add();
        }
        private void numByte0_ValueChanged(object sender, EventArgs e)
        {
            //get data
            for (int i = 0; i < 8; i++)
            {
                Data[i] = (byte)listNumByte[i].Value;
            }
            //draw layout

            //MessageBox.Show("new value:" + (sender as NumericUpDown).Value.ToString());
        }

        private void cmbByteOrder_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numStartBit_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
