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
        private string bin = "", m_Value = "";
        public FrmDBC()
        {
            InitializeComponent();
        }

        private void FrmDBC_Load(object sender, EventArgs e)
        {
            //create control array
            listNumByte.Clear();
            listNumByte.Add(numByte0);
            Random rd = new Random();
            for (int i = 1; i < 8; i++)
            {
                NumericUpDown ctrl = new NumericUpDown();
                ctrl.Name = "numByte" + i.ToString();
                ctrl.Maximum = byte.MaxValue;
                ctrl.Value = rd.Next(byte.MinValue, byte.MaxValue);
                ctrl.Location = new Point(numByte0.Location.X + i * 60, numByte0.Location.Y);
                ctrl.Size = numByte0.Size;
                ctrl.ValueChanged += numByte0_ValueChanged;
                grpMessage.Controls.Add(ctrl);
                listNumByte.Add(ctrl);
            }
            //add columns
            for (int i = 0; i < 8; i++)
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();//DataGridViewTextBoxColumn, not DataGridViewColumn
                col.Name = i.ToString();
                col.HeaderText = (7 - i).ToString();
                col.Width = 30;
                dgvLayout.Columns.Add(col);
            }
            //add rows
            for (int i = 0; i < 8; i++)
            {
                int index = dgvLayout.Rows.Add();
                dgvLayout.Rows[i].HeaderCell.Value = i.ToString();
            }
            //get data
            MessageDataChanged();
            cmbByteOrder.SelectedIndex = 0;
            cmbSigned.SelectedIndex = 0;
        }
        private void MessageDataChanged()
        {
            bin = "";
            for (int i = 0; i < 8; i++)
            {
                Data[i] = (byte)listNumByte[i].Value;
                string str = Convert.ToString(Data[i], 2).PadLeft(8, '0');
                bin += str;
                //draw layout
                for (int j = 0; j < 8; j++)
                {
                    dgvLayout.Rows[i].Cells[j].Value = str[j];
                }
            }
        }
        private void numByte0_ValueChanged(object sender, EventArgs e)
        {
            MessageDataChanged();
            SignalDataChange();
            //MessageBox.Show("new value:" + (sender as NumericUpDown).Value.ToString());
        }
        private void SignalDataChange()
        {
            int startbit = (int)(numStartBit.Value);
            int length = (int)(numLength.Value);
            int index = 0; int count = 0; bool flag = false;
            m_Value = "";
            if (cmbByteOrder.Text == "MSB")
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        index = i * 8 + 7 - j;
                        if (index == startbit)
                        {
                            flag = true;
                        }
                        if (flag && count < length)
                        {
                            dgvLayout.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                            count++;
                            m_Value += dgvLayout.Rows[i].Cells[j].Value.ToString();
                        }
                        else
                        {
                            dgvLayout.Rows[i].Cells[j].Style.BackColor = Color.White;
                        }
                    }
                }

            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 7; j >= 0; j--)
                    {
                        index = i * 8 + 7 - j;
                        if (index >= startbit && index <= startbit + length-1)
                        {
                            dgvLayout.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                            m_Value = dgvLayout.Rows[i].Cells[j].Value.ToString() + m_Value;
                        }
                        else
                        {
                            dgvLayout.Rows[i].Cells[j].Style.BackColor = Color.White;
                        }
                    }
                }
            }
            SignalValueChange();
        }
        private void SignalValueChange()
        {
            if (cmbSigned.Text == "NO")
            {
                txtValue.Text = string.Format("bin={0},dec={1}", m_Value, Convert.ToUInt32(m_Value, 2));
            }
            else
            {
                char sign = m_Value[0];
                string value = m_Value.PadLeft(32, sign);
                txtValue.Text = string.Format("bin={0},dec={1}", m_Value, Convert.ToInt32(value, 2));
            }
        }
        private void cmbByteOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            SignalDataChange();
        }

        private void cmbSigned_SelectedIndexChanged(object sender, EventArgs e)
        {
            SignalValueChange();
        }

        private void numStartBit_ValueChanged(object sender, EventArgs e)
        {
            SignalDataChange();
        }
    }
}
