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
    public partial class Adjust : Form
    {
        SqlConnection cnn = new SqlConnection("Data Source=192.192.140.113;Initial Catalog=D10324110;User ID=sa;Password=takming");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        string name_code;
        public Adjust()
        {
            InitializeComponent();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Main m = new Main();

            this.Dispose();
            m.Show();
        }

        private void Adjust_Load(object sender, EventArgs e)
        {
            //info_off();
            button1.Enabled = true;
            textBox11.Text = "0";


        }
        public void info_off()
        {
            button1.Enabled = false;

            r_info.Hide();
            c_info.Hide();
            p_info.Hide();
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            info_off();
            c_info.Show();
            close();

            name_code = button2.Text;

            cmd.CommandText = "select * from 客戶基本資料 where 身分證字號='" + textBox2.Text + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                c_id.Text = dr.GetValue(0).ToString();
                comboBox2.Text = dr.GetValue(1).ToString();
                c_name.Text = dr.GetValue(2).ToString();
                if (dr.GetValue(6).ToString() == "男")
                {
                    c_sex_0.Checked = true;
                }
                else
                {
                    c_sex_1.Checked = true;
                }
                dateTimePicker1.Text = dr.GetValue(5).ToString();
                textBox14.Text = dr.GetValue(3).ToString();
                textBox15.Text = dr.GetValue(4).ToString();
                if (textBox14.Text == textBox15.Text)
                {
                    checkBox1.Checked = true;
                    textBox15.Enabled = false;
                }
            }
            dr.Close();
            cnn.Close();


        }

        public void close()
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmd.CommandText = "select * from 客戶申裝 where 門號='" + textBox1.Text + "'";
            //cmd.CommandText = "select * from 客戶申裝 where 門號='0925-625875'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox2.Text = dr.GetValue(1).ToString();
                textBox8.Text = dr.GetValue(5).ToString();
            }
            else
            {
                textBox2.Text = "";
                textBox8.Text = "";
            }
            dr.Close();
            cnn.Close();

            if (textBox2.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("請輸入正確的電話號碼~!!");
            }
            else
            {
                cmd.CommandText = "select * from 客戶基本資料 where 身分證字號='" + textBox2.Text + "'";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox3.Text = dr.GetValue(2).ToString();
                    textBox4.Text = dr.GetValue(6).ToString();
                    DateTime dt = Convert.ToDateTime(dr.GetValue(5).ToString());
                    textBox5.Text = dt.ToString("yyyy-MM-dd");
                    textBox6.Text = dr.GetValue(3).ToString();
                    textBox7.Text = dr.GetValue(4).ToString();
                }
                dr.Close();
                cnn.Close();
                open();
            }
        }
        public void open()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {

            //客戶基本資料儲存
            cmd.CommandText = "update 客戶基本資料 set 第二證件='" + comboBox2.Text + "',姓名='" + c_name.Text + "',生日='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',戶籍地址='" + textBox14.Text + "',帳單地址='" + textBox15.Text + "' where 身分證字號='" + c_id.Text + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            dr.Close();
            cnn.Close();

            button14_Click_1(sender, e);
            button1_Click(sender, e);
            addDB客戶異動();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            info_off();
            open();
        }
        private void button14_Click_1(object sender, EventArgs e)
        {
            info_off();
            open();
        }
        public void addDB客戶異動()
        {
            DateTime dt = DateTime.Now;
            DateTime dt2 = DateTime.Now;
            string str = "";
            string next = "";
            string money = "";

            cmd.CommandText = "select * from 異動項目 where 異動項目名稱='" + name_code + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                str = dr.GetValue(0).ToString();
                next = dr.GetValue(2).ToString();
                money = dr.GetValue(3).ToString();
            }
            dr.Close();
            cnn.Close();

            if (next == "次日")
            {
                dt2 = DateTime.Now.AddDays(1);
            }
            else
            {
                dt2 = DateTime.Now.AddDays(Convert.ToDouble(textBox8.Text));
            }

            cmd.CommandText = "insert into 客戶異動 values('" +  textBox1.Text + "','" + str + "','" + dt.ToString("yyyy/MM/dd HH:mm:ss") + "','" + dt2.ToString("yyyy/MM/dd HH:mm:ss") + "')";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            dr.Close();
            cnn.Close();

            additem(name_code, money);
        }
        public void additem(string name, string money)
        {
            listBox1.Items.Add(name);
            listBox2.Items.Add("$" + money + "元");
            double sum = Convert.ToDouble(textBox11.Text) + Convert.ToDouble(money);
            textBox11.Text = sum.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //info_off();
            //s_info.Show();
            //close();
            //name_code = button4.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            info_off();
            r_info.Show();
            close();
            name_code = button3.Text;

            string code = "";

            cmd.CommandText = "select * from 客戶申裝 where 門號='" + textBox1.Text + "'";
            //cmd.CommandText = "select * from 客戶申裝 where 門號='0925-625875'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                code = dr.GetValue(3).ToString();
            }
            dr.Close();
            cnn.Close();

            if (code == "P001")
            {
                rot1.Text = "學生方案";
            }
            else
            {
                rot1.Text = "一般方案";
            }

            cmd.CommandText = "select * from 資費方案 where 資費方案 = '" + code + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                rot2.Text = dr.GetValue(1).ToString();
                rot3.Text = dr.GetValue(2).ToString();
                rot4.Text = dr.GetValue(3).ToString();
                rot5.Text = dr.GetValue(4).ToString();
                rot6.Text = dr.GetValue(5).ToString();
                rot7.Text = dr.GetValue(6).ToString();
                rot8.Text = dr.GetValue(7).ToString();
            }
            dr.Close();
            cnn.Close();

            cmd.CommandText = "select * from 資費方案";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                rnc1.Items.Clear();
                while (dr.Read())
                {
                    if (dr.GetValue(0).ToString() == "P001")
                    {
                        rnc1.Items.Add("學生方案");
                    }
                    else
                    {
                        rnc1.Items.Add("一般方案");
                    }
                }
            }
            dr.Close();
            cnn.Close();
        }

        private void rnc1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = "";

            if (rnc1.Text == "學生方案")
            {
                code = "P001";
            }
            else
            {
                code = "P002";
            }

            cmd.CommandText = "select * from 資費方案 where 資費方案 = '" + code + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                rnt1.Text = dr.GetValue(1).ToString();
                rnt2.Text = dr.GetValue(2).ToString();
                rnt3.Text = dr.GetValue(3).ToString();
                rnt4.Text = dr.GetValue(4).ToString();
                rnt5.Text = dr.GetValue(5).ToString();
                rnt6.Text = dr.GetValue(6).ToString();
                rnt7.Text = dr.GetValue(7).ToString();
            }
            dr.Close();
            cnn.Close();
        }

        private void r_info_s_Click(object sender, EventArgs e)
        {
            //費率方案儲存
            if (rot1.Text == rnc1.Text)
            {
                MessageBox.Show("原費率方案與新費率方案相同~!!");
            }
            else
            {
                string oldstr = "";
                string newstr = "";

                if (rot1.Text == "學生方案")
                {
                    oldstr = "P001";
                }
                else
                {
                    oldstr = "P002";
                }
                if (rnc1.Text == "學生方案")
                {
                    newstr = "P001";
                }
                else
                {
                    newstr = "P002";
                }

                cmd.CommandText = "insert into 客戶費率方案異動 values('" + textBox1.Text + "','" + oldstr + "','" + newstr + "')";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                dr.Close();
                cnn.Close();

                updateDB客戶申裝();
                addDB客戶異動();
                r_info_c_Click(sender, e);
            }

        }
        public void updateDB客戶申裝()
        {
            if (name_code == button3.Text)
            {
                string code = "";
                if (rnc1.Text == "學生方案")
                {
                    code = "P001";
                }
                else
                {
                    code = "P002";
                }
                cmd.CommandText = "update 客戶申裝 set 資費方案代號='" + code + "' where 門號='" +  textBox1.Text + "'";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                dr.Close();
                cnn.Close();
            }
        }
        private void r_info_c_Click(object sender, EventArgs e)
        {
            info_off();
            open();
            rnc1.Items.Clear();
            rnc1.Text = "";
            rnt1.Text = "";
            rnt2.Text = "";
            rnt3.Text = "";
            rnt4.Text = "";
            rnt5.Text = "";
            rnt6.Text = "";
            rnt7.Text = "";
        }
        String oldpow = "";
        private void button6_Click(object sender, EventArgs e)
        {

            pwn1.Select();
            String name = "";
            String account = "";
            info_off();
            p_info.Show();
            close();
            name_code = button6.Text;


            cmd.CommandText = "select * from 客戶申裝 where 門號='" + textBox1.Text + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            dr.Read();
            oldpow=dr.GetValue(4).ToString();
            dr.Close();
            cnn.Close();
            if (oldpow == "自繳")
            {
                pwo1.Select();
                pbo.Text = "";
                pbno.Text = "";
            }
            else
            {
                pwo2.Select();
                cmd.CommandText = "select 門號,合約.合約代號,機構名稱,帳號 from 客戶轉帳 left OUTER join 合約 ON 客戶轉帳.合約代號 = 合約.合約代號 where 門號='" + textBox1.Text + "'";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                dr.Read();
                name= dr.GetValue(2).ToString();
                account=dr.GetValue(3).ToString();
                pbo.Text = name;
                pbno.Text = account;
                dr.Close();
                cnn.Close();


            }

            dr.Close();
            cnn.Close();

        }
        private String[] contract = new String[10];

        private void pwn2_CheckedChanged(object sender, EventArgs e)
        {
            if (pwn2.Checked)
            {
                int connum = 0;
                pbn.Enabled = true;
                pbnn.Enabled = true;
                cmd.CommandText = "select * from 合約 where left(合約代號,1)='B'";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    pbn.Items.Add(dr.GetValue(1));
                    contract[connum++] = dr.GetValue(0).ToString();
                }
                dr.Close();
                cnn.Close();
            }
            else
            {
                pbn.Enabled = false;
                pbnn.Enabled = false;
                pbnn.Text = "";
                pbn.Items.Clear();
                pbn.Text = "請選擇";
                
            }
        }

        private void p_info_s_Click(object sender, EventArgs e)
        {
            String pow;
            String account, name,accountid;

            if (pwn1.Checked)
                pow = "自繳";
            else
                pow = "轉帳";

            cmd.CommandText = "update 客戶申裝 set 繳費方式='" + pow + "' where 門號='" + textBox1.Text + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            dr.Close();
            cnn.Close();
            if (oldpow == "自繳" && pow == "自繳")
            {
                MessageBox.Show("錢很多是嘛?");
                button14_Click_1(sender, e);
                button1_Click(sender, e);
                return;

            }
            else if (oldpow == "轉帳" && pow == "自繳")
            {
                cmd.CommandText = "delete from 客戶轉帳 where 門號='"+textBox1.Text+"'";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                dr.Close();
                cnn.Close();
            }
            else if (oldpow == "自繳" && pow == "轉帳")
            {
                name = pbn.Text;
                account = pbnn.Text;

                //機構名稱轉機構代號
                cmd.CommandText = "select * from 合約 where 機構名稱='"+name+"'";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                dr.Read();
                accountid=dr.GetValue(0).ToString();
                dr.Close();
                cnn.Close();


                cmd.CommandText = "insert into 客戶轉帳 values ('"+textBox1.Text+"','"+accountid+"','"+account+"')";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                dr.Close();
                cnn.Close();
            }else if(pow=="轉帳")
            {
                name = pbn.Text;
                account = pbnn.Text;


                //機構名稱轉機構代號
                cmd.CommandText = "select * from 合約 where 機構名稱='" + name + "'";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                dr.Read();
                accountid = dr.GetValue(0).ToString();
                dr.Close();
                cnn.Close();

                cmd.CommandText = "update 客戶轉帳 set 合約代號='" + accountid + "',帳號='"+account+"' where 門號='" + textBox1.Text + "'";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                dr.Close();
                cnn.Close();
            }
            button14_Click_1(sender, e);
            button1_Click(sender, e);
            addDB客戶異動();
        }

        private void p_info_c_Click(object sender, EventArgs e)
        {
            info_off();
            open();
        }

        private void r_info_c_Click_1(object sender, EventArgs e)
        {
            info_off();
            open();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox15.Enabled = false;
                textBox15.Text = textBox14.Text;
            }
            else
                textBox15.Enabled = true;

        }
    }
}
