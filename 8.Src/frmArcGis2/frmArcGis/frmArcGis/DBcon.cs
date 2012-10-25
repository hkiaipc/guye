using System;
using System.Xml ;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Utilities;

namespace btGR
{
    /// <summary>
    /// DBcon 的摘要说明。
    /// </summary>
    public class DBcon
    {
        private SqlConnection con;
        string str=null;

        /// <summary>
        /// 
        /// </summary>
        public DBcon()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            XmlDocument XDoc=new XmlDocument();
            XDoc.Load("Server.xml");
            XmlNode XNode=XDoc.DocumentElement.SelectSingleNode("./SqlCon");
            for(int i=0;i<XNode.ChildNodes.Count;i++)
            {
                str=str+XNode.ChildNodes[i].Name;
                str=str+"=";
                str=str+XNode.ChildNodes[i].InnerText.Trim();
                str=str+";";
            }
            con=new SqlConnection(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection()
        {
			
            try
            {
                if(con.State!=ConnectionState.Open)
                {
                    con.Open();
                }
                return con;
            }				
            catch(Exception ex)
            {
                // 2007.05.30
                //
                //return null;
                ExceptionHandler.Handle("连接数据库失败", ex );
                return null;
            }
        }
    }
}