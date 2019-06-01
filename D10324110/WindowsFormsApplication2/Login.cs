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
    public partial class Login : Form
    {
        int num = 0;
        public Login()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection cnn = new SqlConnection("data source=192.192.140.113;initial catalog=D10324110;user id=sa;password=takming");
            SqlCommand cmd = new SqlCommand("select * from 員工資料表 where 員工代號='" + textBox1.Text + "' and 密碼='" + textBox2.Text + "'");
            cmd.Connection = cnn;
            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                while(dr.Read())
                {
                    if (dr.GetValue(5).ToString()=="5")
                    {
                        Main main = new Main();
                        main.Show();
                        this.Hide();
                    }
                    else
                    {
                        Main2 main2 = new Main2();
                        main2.Show();
                        this.Hide();
                    }
                }
            }
            else
            {
                num++;
                if(num<3)
                {
                    MessageBox.Show("登入失敗");
                }
                else
                {
                    MessageBox.Show("登入失敗3次\n請確認後再登入");
                    Environment.Exit(Environment.ExitCode);
                }
                dr.Close();
                cnn.Close();
            }

        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((int)e.KeyChar==13)
            {
                button1_Click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
