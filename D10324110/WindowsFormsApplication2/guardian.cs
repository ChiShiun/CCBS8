using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication2
{
    public partial class guardian : Form
    {
        public guardian()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox1.Text = "請選擇";
            comboBox2.Text = "請選擇";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("請輸入身份證字號!");
                return;
            }
            else
            {
                Apply.p_id = textBox1.Text;
            }

            if (textBox2.Text == "")
            {
                MessageBox.Show("請輸入姓名!");
                return;
            }
            else
            {
                Apply.p_name = textBox2.Text;
            }

            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇第二證件!");
                return;
            }
            else
            {
                Apply.p_documents = comboBox1.SelectedItem.ToString();
            }

            if (comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇關係");
                return;
            }
            else
            {
                Apply.p_relationship = comboBox2.SelectedItem.ToString();
            }
            this.Close();
        }

        private void guardian_Load(object sender, EventArgs e)
        {
            //if (Apply.number != "")
            //{
            //    SqlConnection cnn = new SqlConnection("Data Source=192.192.140.113;Initial Catalog=D10324110;User ID=sa;Password=takming");
            //    SqlCommand cmd = new SqlCommand("select * from 監護人資料 where 門號='" + Apply.number + "'");
            //    cmd.Connection = cnn;
            //    cnn.Open();
            //    SqlDataReader dr = cmd.ExecuteReader();
            //    if (dr.HasRows)
            //    {
            //        while (dr.Read())
            //        {
            //            textBox1.Text = dr.GetValue(1).ToString();
            //            textBox2.Text = dr.GetValue(2).ToString();
            //            switch (dr.GetValue(3).ToString())
            //            {
            //                case "健保卡":
            //                    comboBox1.SelectedIndex = 0;
            //                    break;
            //                case "駕照":
            //                    comboBox1.SelectedIndex = 1;
            //                    break;
            //            }
            //            switch (dr.GetValue(4).ToString())
            //            {
            //                case "父子":
            //                    comboBox2.SelectedIndex = 0;
            //                    break;
            //                case "父女":
            //                    comboBox2.SelectedIndex = 1;
            //                    break;
            //                case "母子":
            //                    comboBox2.SelectedIndex = 2;
            //                    break;
            //                case "母女":
            //                    comboBox2.SelectedIndex = 3;
            //                    break;
            //                case "姊弟":
            //                    comboBox2.SelectedIndex = 4;
            //                    break;
            //                case "姊妹":
            //                    comboBox2.SelectedIndex = 5;
            //                    break;
            //                case "兄弟":
            //                    comboBox2.SelectedIndex = 6;
            //                    break;
            //                case "兄妹":
            //                    comboBox2.SelectedIndex = 7;
            //                    break;
            //            }
            //        }
            //    }
            //    dr.Close();
            //    cnn.Close();
            //}
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 8)
            {
                e.Handled = false;
               
            }
            else if (textBox1.TextLength == 10)
            {
                e.Handled = true;
                
            }
            else
            {
                if (textBox1.TextLength == 0)
                {
                    if ((int)e.KeyChar >= 65 && (int)e.KeyChar <= 90)
                    {
                        e.KeyChar = (char)(int)e.KeyChar;
                        e.Handled = false;
                    }
                    else if (e.KeyChar >= 97 && e.KeyChar <= 122)
                    {
                        e.KeyChar = (char)((int)e.KeyChar - 32);
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    if ((int)e.KeyChar < 48 || e.KeyChar > 57)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                      
                    }
                }
            }
        }

        public void callData()
        {

            SqlConnection cnn = new SqlConnection("Data Source = 192.192.140.113; Initial Catalog = D10324110; User ID = sa; Password=takming");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            cmd.CommandText = "select * from 客戶基本資料 where 身分證字號='" + textBox1.Text + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    String documents = dr.GetValue(1).ToString();
                    String name = dr.GetValue(2).ToString();
                    String houseadd = dr.GetValue(3).ToString();
                    String commadd = dr.GetValue(4).ToString();
                    DateTime birthday = (DateTime)dr.GetValue(5);

                    if (documents == "健保卡")
                    {
                        comboBox1.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox1.SelectedIndex = 1;
                    }
                    textBox2.Text = name;
                    

                }
            }
            dr.Close();
            cnn.Close();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            callData();
        }
    }
    
}
