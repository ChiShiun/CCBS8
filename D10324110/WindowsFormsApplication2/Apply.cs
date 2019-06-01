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
    public partial class Apply : Form
    {
        SqlConnection cnn = new SqlConnection("Data Source = 192.192.140.113; Initial Catalog = D10324110; User ID = sa; Password=takming");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        public static String p_id = "", p_name = "", p_documents = "", p_relationship = "", number = "";
        public int bonuscount=0;
        public int[] lv_tell = new int[3];
        private int select_num = 0;
        private int boun_submit = 0;
        private String constr = "  ";
        private String[] contract = new String[10];
        public String gettext;
        public Apply()
        {
            InitializeComponent();
        }

        public bonus[] bonusarray = new bonus[10];

        private void Apply_Load(object sender, EventArgs e)
        {
            //申裝日期
            DateTime dt =  DateTime.Now;
            textBox6.Text = String.Format("{0}", dt);
            //加值功能
            cmd.CommandText = "select * from 加值功能";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                int count = 0;
                while(dr.Read())
                {
                    bonusarray[count] = new bonus(dr.GetValue(0).ToString(), dr.GetValue(1).ToString(), Convert.ToInt32(dr.GetValue(2)), false);
                    bonuscount = count;
                    checkedListBox1.Items.Add(bonusarray[count++].getname());

                }
            }
            dr.Close();
            cnn.Close();
            //經銷商
            cmd.CommandText = "select 機構名稱 from 合約 where left(合約代號,1)='C'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while(dr.Read())
                {
                    comboBox4.Items.Add(dr.GetValue(0).ToString());
                }
            }
            dr.Close();
            cnn.Close();
            //話號等級
            cmd.CommandText = "select 等級,費用 from 話號等級";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                int i = 0;
                while (dr.Read())
                {
                    comboBox2.Items.Add(dr.GetValue(0));
                    lv_tell[i++] = int.Parse(dr.GetValue(1).ToString());
                }
            }
            dr.Close();
            cnn.Close();

            //出帳週期
            Random rnd = new Random();
            string[] billing_date = { "5", "10", "15", "20", "25" };
            int billing_index = rnd.Next(0, billing_date.Length);
            textBox7.Text = billing_date[billing_index];

            //費率方案
            cmd.CommandText = "select * from 資費方案";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string a = dr.GetValue(0).ToString();
                switch (a)
                {
                    case "P001":
                        comboBox3.Items.Add("學生方案");
                        break;
                    case "P002":
                        comboBox3.Items.Add("一般方案");
                        break;
                }
            }
            dr.Close();
            cnn.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox9.Text = lv_tell[comboBox2.SelectedIndex].ToString();
            listBox1.Items.Clear();
            select_num = 0;
            output_select_num();
        }


        public class bonus
        {
            private String bonus_no = "", bonus_name = "";
            private int bonus_money = 0;
            private Boolean bonus_check;

            public bonus(String no,String name,int money,Boolean check)
            {
                bonus_no = no;
                bonus_name = name;
                bonus_money = money;
                bonus_check = check;
            }

            public String getno()
            {
                return bonus_no;
            }

            public String getname()
            {
                return bonus_name;
            }
            public int getmoney()
            {
                return bonus_money;
            }
            public void setcheck(bool check)
            {
                bonus_check = check;
            }
            public bool getcheck()
            {
                return bonus_check;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmd.CommandText = "select * from 門號SIM卡配對 where 門號='" + listBox1.SelectedItem.ToString() + "'"; ;
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox8.Text = dr.GetValue(1).ToString();
            }
            dr.Close();
            cnn.Close();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            select_num += 1;
            listBox1.Items.Clear();
            output_select_num();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            select_num -= 1;
            listBox1.Items.Clear();
            output_select_num();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            String b = "";
            switch (comboBox3.SelectedItem.ToString())
            {
                case "學生方案":
                    b = "P001";
                    break;
                case "一般方案":
                    b = "P002";
                    break;
            }
            cmd.CommandText = "select * from 資費方案 where 資費方案='" + b + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox10.Text = dr.GetValue(1).ToString();
                textBox11.Text = dr.GetValue(6).ToString();
                textBox12.Text = dr.GetValue(7).ToString();
                textBox13.Text = dr.GetValue(8).ToString();
                textBox14.Text = dr.GetValue(9).ToString();
                textBox15.Text = dr.GetValue(10).ToString();
                textBox16.Text = dr.GetValue(11).ToString();
            }
            dr.Close();
            cnn.Close();

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if(checkedListBox1.GetItemChecked(e.Index))
            {
                
                boun_submit -= bonusarray[e.Index].getmoney();
                bonusarray[e.Index].setcheck(false);
            }
            else
            {
                boun_submit += bonusarray[e.Index].getmoney();
                bonusarray[e.Index].setcheck(true);
            }
            textBox17.Text = boun_submit.ToString();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton4.Checked)
            {
                int connum = 0;
                comboBox5.Enabled = true;
                textBox18.Enabled = true;
                cmd.CommandText = "select * from 合約 where left(合約代號,1)='B'";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox5.Items.Add(dr.GetValue(1));
                    contract[connum++] = dr.GetValue(0).ToString();
                }
                dr.Close();
                cnn.Close();
            }
            else
            {
                comboBox5.Enabled = false;
                textBox18.Enabled = false;
                textBox18.Text = "";
                comboBox5.Items.Clear();
                comboBox5.Text = "請選擇";
                constr = "";
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            constr = contract[comboBox5.SelectedIndex];

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            Apply apply = new Apply();
            apply.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Main m = new Main();
            this.Close();
            m.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox5.Text = textBox4.Text;
                textBox5.ReadOnly = true;
            }
            else
                textBox5.ReadOnly = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 8)
            {
                e.Handled = false;
                if (textBox1.TextLength == 2)
                {
                    gender.Text = "";
                }
            }
            else if (textBox1.TextLength == 10)
            {
                e.Handled = true;
                callData();
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
                        if (textBox1.TextLength == 1)
                        {
                            if ((int)e.KeyChar == 49)
                            {
                                gender.Text = "男";
                            }
                            else if ((int)e.KeyChar == 50)
                            {
                                gender.Text = "女";
                            }
                        }
                    }
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {


        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            callData();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.TextLength == 10)
            {
                if (DateTime.Now.Year - DateTime.Parse(textBox3.Text).Year < 20)
                {
                    guardian guar = new guardian();
                    guar.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("生日資料格式錯誤!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int age = DateTime.Now.Year - DateTime.Parse(textBox3.Text).Year;
            if (textBox1.TextLength != 10)
                MessageBox.Show("身份證字號尚未輸入正確","提示",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            else if(listBox1.SelectedIndex<0)
                MessageBox.Show("尚未選門號", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if(age<20 && get_info())
                MessageBox.Show("監護人資料尚未填寫完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (textBox2.Text==""|textBox4.Text==""|textBox5.Text==""|comboBox1.SelectedIndex==-1)
                MessageBox.Show("資料尚未完填寫完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //新增或更新到資料庫
                addDB客戶基本資料();
                addDB監護人資料();
                addDB客戶申裝();
                addDB客戶加值();
                addDB客戶轉帳();
                updateDB門號sim卡配對();
                updateDB門號();
                updateDBsim卡();
                this.Close();
                Main m = new Main();
                m.Show();
            }
        }
        public void updateDBsim卡()
        {
            cmd.CommandText = "update SIM卡 set 狀態='使用' where SIM卡序號='" + textBox8.Text + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            dr.Close();
            cnn.Close();
        }
        public void updateDB門號()
        {
            cmd.CommandText = "update 門號 set 狀態='使用' where 門號='" + listBox1.SelectedItem + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            dr.Close();
            cnn.Close();
        }
        public void updateDB門號sim卡配對()
        {
            cmd.CommandText = "update 門號SIM卡配對 set 狀態='使用' where 門號='" + listBox1.SelectedItem + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            dr.Close();
            cnn.Close();
        }
        public void addDB客戶轉帳()
        {
            if (radioButton4.Checked == true)
            {
                string text = constr;
                cmd.CommandText = "insert into 客戶轉帳 values('" + listBox1.SelectedItem + "','" + text + "','" + textBox18.Text + "')";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                dr.Close();
                cnn.Close();
            }
            
        }
        public void addDB客戶加值()
        {
             for (int i = 0; i <= bonuscount; i++)
            {
                if (bonusarray[i].getcheck() == true)
                {
                    cmd.CommandText = "insert into 客戶加值 values('" + listBox1.SelectedItem + "','" + bonusarray[i].getno() + "')";
                    cmd.Connection = cnn;
                    cnn.Open();
                    dr = cmd.ExecuteReader();
                    dr.Close();
                    cnn.Close();
                }
            }
        }
        public void addDB客戶申裝()
        {
            string text = "無", text1 = "無", text2 = "無";
            if (radioButton3.Checked == true)
            {
                text = "自繳";
            }
            else if (radioButton4.Checked == true)
            {
                text = "轉帳";
            }
            cmd.CommandText = "select * from 合約 where 機構名稱='" + comboBox4.Text + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                text1 = dr.GetValue(0).ToString();
                gettext = text1;
            }
            dr.Close();
            cnn.Close();
            if (comboBox3.Text == "學生方案")
            {
                text2 = "P001";
            }
            else if (comboBox3.Text == "一般方案")
            {
                text2 = "P002";
            }
            //申裝日期
            DateTime dt = DateTime.Now;
            String trans_d = dt.ToString("yyyy-mm-dd");
            cmd.CommandText = "insert into 客戶申裝 values('" + listBox1.SelectedItem + "','" + textBox1.Text + "','" + trans_d + "','" + text2 + "','" + text + "','" + textBox7.Text + "','" + text1 + "')";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            dr.Close();
            cnn.Close();
        }
        public void addDB監護人資料()
        {
            if (p_id != "")
            {
                cmd.CommandText = "insert into 監護人資料 values('" + listBox1.SelectedItem + "','" + p_id + "','" + p_name + "','" + p_documents + "','" + p_relationship + "')";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();
                dr.Close();
                cnn.Close();
            }
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                comboBox4.Enabled = true;
            }
            else
                comboBox4.Enabled = false;
        }

        public void addDB客戶基本資料()
        {
            cmd.CommandText = "select * from 客戶基本資料 where 身分證字號='" + textBox1.Text + "'";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                dr.Close();
                cnn.Close();
                cmd.CommandText = "insert into 客戶基本資料 values('" + textBox1.Text + "','" + comboBox1.Text + "','" + textBox2.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox3.Text + "','" + gender.Text + "')";
                cmd.Connection = cnn;
                cnn.Open();
                dr = cmd.ExecuteReader();

            }
            dr.Close();
            cnn.Close();
        }

        public Boolean get_info()
        {
            if(p_id== ""|p_name ==""|p_documents==""|p_relationship=="")
                return true;
            else
                return false;            
        }

        public void output_select_num()
        {
            if (select_num != 0)
                button1.Enabled = true;
            else
                button1.Enabled = false;

            cmd.CommandText = "select top 5 門號 from 門號 where 話號等級='" + comboBox2.SelectedItem.ToString() + "' and 狀態='未使用' and 門號 not in (select top " + (select_num * 5) + " 門號 from  門號 where 話號等級='" + comboBox2.SelectedItem.ToString() + "' and 狀態='未使用')";
            cmd.Connection = cnn;
            cnn.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listBox1.Items.Add(dr.GetValue(0));
            }          
            dr.Close();
            cnn.Close();

        }

        public void callData()
        {
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
                    textBox3.Text = birthday.ToString("yyyy/MM/dd");
                    textBox4.Text = houseadd;
                    textBox5.Text = commadd;

                }
            }
            dr.Close();
            cnn.Close();
        }

    }
}
