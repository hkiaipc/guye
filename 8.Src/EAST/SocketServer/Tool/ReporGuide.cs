using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Tool
{
    public partial class reporguide : Form
    {
        public reporguide()
        {
            InitializeComponent();
        }

        private string sqlstr;

        private void reporguide_Load(object sender, EventArgs e)
        {
            button4.Enabled = false;
            button7.Enabled = false;
            textBox1.Visible = false;
            ref_tbv();
        }

        //刷新数据库表名内容
        private void ref_tbv()
        {
            string str = "SELECT Name FROM SysObjects Where (type='U'and  status>=0) or (type='V' and status>=0 ) ORDER BY Name";
            DataTable dt = Tool.DB.getDt(str);
            comboBox1.Items.Clear();
            for (int i=0; i < dt.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt.Rows[i]["name"]);
            }
        }
        //选择表
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ref_dgv();
        }

        private void ref_dgv()
        {
            dataGridView1.Columns.Clear();
            string str ="SELECT Name FROM SysColumns WHERE id=Object_Id('"+comboBox1.Text+"') order by colid ";
            DataTable dtdgv = Tool.DB.getDt(str);
            dtdgv.Columns[0].ColumnName = "字段";
          //  dtdgv.Columns[0].ReadOnly = true;
            dataGridView1.DataSource = dtdgv;

            DataGridViewCheckBoxColumn dgvch = new DataGridViewCheckBoxColumn();
            dataGridView1.Columns.Insert(0, dgvch);
            dataGridView1.Columns[0].Name = "显示";

            DataGridViewTextBoxColumn dgvtx = new DataGridViewTextBoxColumn();
            dataGridView1.Columns.Add(dgvtx);
            dataGridView1.Columns[2].Name = "别名";

            DataGridViewComboBoxColumn dgvcbc = new DataGridViewComboBoxColumn();
            dgvcbc.Items.Add("不排序");
            dgvcbc.Items.Add("升序");
            dgvcbc.Items.Add("降序");
            dataGridView1.Columns.Add(dgvcbc);
            dataGridView1.Columns[3].Name = "排序";

            DataGridViewComboBoxColumn dgvcbc2 = new DataGridViewComboBoxColumn();
            for (int i = 0; i < dataGridView1.Rows.Count;i++ )
            {
                dgvcbc2.Items.Add((i + 1).ToString());
            }
            dataGridView1.Columns.Add(dgvcbc2);
            dataGridView1.Columns[4].Name = "优先级";

            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 108;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[3].Width = 80;
            dataGridView1.Columns[4].Width = 80;   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ref_dgv();
        }
        //生成
        private void button2_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            button7.Enabled = true;
            textBox1.Visible = true;
            button2.Enabled = false;

            dataGridView1.EndEdit();
            
            string file_str="";
            for(int i=0;i<dataGridView1.Rows.Count;i++)
            {
                if (Convert.ToBoolean(dataGridView1["显示", i].Value) == true)
                {
                    if (Convert.ToString(dataGridView1["别名", i].Value) == "")
                    {
                        file_str += Convert.ToString(dataGridView1["字段", i].Value) + ", ";
                    }
                    else
                    {
                        file_str += Convert.ToString(dataGridView1["字段", i].Value) + " as " + Convert.ToString(dataGridView1["别名", i].Value) + ", ";
                    }
                }
            }
            if (file_str == "")
            {
                MessageBox.Show("没有可用的字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            string order_str="";
            int order_int=0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                //选中显示有优先级
                if (Convert.ToString(dataGridView1["优先级", i].Value) != "" && Convert.ToBoolean(dataGridView1["显示", i].Value) == true && Convert.ToInt16(dataGridView1["优先级", i].Value)!=0)
                {
                    if (Convert.ToString(dataGridView1["排序", i].Value) =="升序")
                    {
                        if (order_int < Convert.ToInt16(dataGridView1["优先级", i].Value))
                        {
                            order_str = Convert.ToString(dataGridView1["字段", i].Value) + " ASC, " + order_str;
                        }
                        else
                        {
                            order_str = order_str + Convert.ToString(dataGridView1["字段", i].Value) + " ASC, ";
                        }
                        order_int=Convert.ToInt16(dataGridView1["优先级", i].Value);
                    }
                    if (Convert.ToString(dataGridView1["排序", i].Value) == "降序")
                    {
                        if (order_int < Convert.ToInt16(dataGridView1["优先级", i].Value))
                        {
                            order_str = Convert.ToString(dataGridView1["字段", i].Value) + " DESC, " + order_str;
                        }
                        else
                        {
                            order_str = order_str + Convert.ToString(dataGridView1["字段", i].Value) + " DESC, ";
                        }
                        order_int = Convert.ToInt16(dataGridView1["优先级", i].Value);
                    }
                }
            }
            if (order_str == "")
            {
                sqlstr = "SELECT " + file_str.TrimEnd(' ').TrimEnd(',') + " FROM " + comboBox1.Text;
            }
            else
            {
                sqlstr = "SELECT " + file_str.TrimEnd(' ').TrimEnd(',') + " FROM " + comboBox1.Text + " ORDER BY " + order_str.TrimEnd(' ').TrimEnd(',');
            }
            textBox1.Text = sqlstr;
        }
        //保存
        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel(*.Xml)|*.Xml|All Files(*.*)|*.*";
            dialog.FileName = "请命名导出文件.xml";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                XmlDocument doc = new XmlDocument();

                XmlTextWriter xtw = new XmlTextWriter(dialog.FileName, Encoding.UTF8);   //新建XML文件
                xtw.WriteStartDocument();
                xtw.WriteStartElement("Root");               // Root根节点
                xtw.WriteEndElement();
                xtw.WriteEndDocument();
                xtw.Close();
                doc.Load(dialog.FileName);        
           
                XmlNode xn = doc.DocumentElement;                  // 找到根节点
                XmlElement xe = doc.CreateElement("add");       // 在根节点下创建元素，如果是属性，则用XmlAttribute；

                xe.SetAttribute("value", sqlstr);
                xe.InnerText = "sqlstr";                           // 给子节点写入文本节点（值）
                xn.AppendChild(xe);                                // 根节点将其纳入

                doc.Save(dialog.FileName);                         //利用XmlDocument保存文件
                MessageBox.Show("保存成功！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
        //取消
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //上移
        private void button5_Click(object sender, EventArgs e)
        {
            exchangedgvr(-1);
        }
        //下移
        private void button6_Click(object sender, EventArgs e)
        {
            exchangedgvr(1);
        }
        //交换行
        private void exchangedgvr(int num)
        {
            dataGridView1.EndEdit();
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            int row = dataGridView1.CurrentRow.Index;

            if (row == 0&&num==-1)
            {
                return;
            }
            if (row == dataGridView1.Rows.Count-1 && num == 1)
            {
                return;
            }

            bool temp_dis = Convert.ToBoolean(dataGridView1["显示", row].Value);
            string temp_file = Convert.ToString(dataGridView1["字段", row].Value);
            string temp_name = Convert.ToString(dataGridView1["别名", row].Value);
            string temp_order = Convert.ToString(dataGridView1["排序", row].Value);
            string temp_num = Convert.ToString(dataGridView1["优先级", row].Value);

            dataGridView1["显示", row].Value = dataGridView1["显示", row + num].Value;
            dataGridView1["字段", row].Value = dataGridView1["字段", row + num].Value;
            dataGridView1["别名", row].Value = dataGridView1["别名", row + num].Value;
            dataGridView1["排序", row].Value = dataGridView1["排序", row + num].Value;
            dataGridView1["优先级", row].Value = dataGridView1["优先级", row + num].Value;

            dataGridView1["显示", row + num].Value = temp_dis;
            dataGridView1["字段", row + num].Value = temp_file;
            dataGridView1["别名", row + num].Value = temp_name;
            dataGridView1["排序", row + num].Value = temp_order;
            dataGridView1["优先级", row + num].Value = temp_num;
            
            dataGridView1.CurrentCell = dataGridView1.Rows[row + num].Cells[dataGridView1.CurrentCell.ColumnIndex];
        }

        //编辑返回
        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button4.Enabled = false;
        }
        //全选
        private void button8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1["显示", i].Value = true;
            }
        }
        //全不选
        private void button9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1["显示", i].Value = false;
            }
        }

    }
}

