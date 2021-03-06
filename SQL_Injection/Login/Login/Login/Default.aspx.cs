﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Login
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string name = TextBox1.Text;
            string pwd = TextBox2.Text;
            UserLogin(name, pwd);
        }


        private void UserLogin(string _loginname, string _loginpwd)
        {
            SqlConnection conn = new SqlConnection
            {
                ConnectionString = "User ID=sa;Initial Catalog=Test;Data Source= (local);Password=sql"
            };

            SqlCommand cmd;
            string sql;

            if (CheckBox1.Checked)
            {
                sql = "select * from usertable Where userid='" + _loginname + "' and password='" + _loginpwd + "'"; //创建查询语句
                cmd = new SqlCommand(sql, conn);
            }
            else
            {
                sql = "select * from usertable where userid=@username and password=@password"; //使用参数控制传递，避免SQL注入
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 10);
                cmd.Parameters["@username"].Value = _loginname;
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 10);
                cmd.Parameters["@password"].Value = _loginpwd;
            }
            Label4.Text = sql;
            try
            {
                // 打开连接
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlDataReader sqlDataReader = cmd.ExecuteReader();    //从数据库中读取数据流存入reader中                                              
                if (sqlDataReader.Read()) //从sdr中读取下一行数据,如果没有数据,sdr.Read()返回flase 
                {
                    Label3.Text = "登录成功。您的用户名:" + sqlDataReader[0] + "，您的密码:" + sqlDataReader[1];
                    Button2.Enabled = true;
                }
                else
                {
                    Label3.Text = "登录失败。输入账号或密码错误";
                    Button2.Enabled = false;
                }
                sqlDataReader.Close();

            }
            catch (Exception ee)
            {
                Response.Write("连接数据库失败！" + ee.ToString());
            }
            finally
            {
                conn.Close();
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Main.aspx");
        }
    }
}