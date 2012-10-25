using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;


namespace Tool
{
    /// <summary>
    /// CDbConnection 的摘要说明。
    /// </summary>
    /// 
    public class DB
    {
        
        public static string sqlcon;
       // private static string sqlcon = "Data Source=127.0.0.1;initial catalog=LXDB;user id=sa;password=";
        public DB()
        {
        }
        //strSql like "select count(*) from table" or "select max(id) from table"
        public static string getStr(string strSql)
        {
            try
            {
                SqlConnection sCon = new SqlConnection(sqlcon);
                sCon.Open();

                SqlCommand sCmd = new SqlCommand();
                sCmd.Connection = sCon;
                sCmd.CommandText = strSql;
                object str = sCmd.ExecuteScalar();

                if (str == null)
                {
                    sCon.Close();
                    return null;
                }
                else
                {
                    sCon.Close();
                    return str.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;

            }
        }


        //select
        public static DataTable getDt(string strSql)
        {
            try
            {
                SqlConnection sCon = new SqlConnection(sqlcon);
                sCon.Open();

                SqlCommand sCmd = new SqlCommand();
                sCmd.Connection = sCon;
                sCmd.CommandText = strSql;

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = sCmd;

                DataTable dt = new DataTable();
                sda.Fill(dt);
                //sCmd.ExecuteNonQuery();
                sCon.Close();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        //edit

        public static void runCmd(string strSql)
        {
            try
            {
                SqlConnection sCon = new SqlConnection(sqlcon);
                sCon.Open();

                SqlCommand sCmd = new SqlCommand();
                sCmd.Connection = sCon;
                sCmd.CommandText = strSql;
                sCmd.ExecuteNonQuery();

                sCon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}

