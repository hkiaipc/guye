using System;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

using System.Runtime.InteropServices;

namespace Tool
{
    public class Export
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public static void saveAs(DataGridView gridView, string name, DateTimePicker dateTimePicker1, DateTimePicker dateTimePicker2) //导出Excel
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel(*.xls)|*.xls|All Files(*.*)|*.*";
            dialog.FileName = "请命名导出文件.xls";
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                object missing = System.Reflection.Missing.Value;
                try
                {
                    if (xlApp == null)
                    {
                        MessageBox.Show("无法创建Excel对象，可能未安装Excel");
                        return;
                    }

                    Microsoft.Office.Interop.Excel.Workbooks xlBooks = xlApp.Workbooks;
                    Microsoft.Office.Interop.Excel.Workbook xlBook = xlBooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                    Microsoft.Office.Interop.Excel.Worksheet xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets[1];
                    Microsoft.Office.Interop.Excel.Range range = null;
                    Microsoft.Office.Interop.Excel.Range range1 = null;
                    //****** 抬头 *********************************************************************************

                    range = xlSheet.get_Range("A1", "E1");

                    range = xlSheet.get_Range("F1", xlSheet.Cells[1, gridView.Columns.Count]);
                    range.Merge(missing);
                    range1.Merge(missing);// 合并单元格 
                    range.Columns.AutoFit();
                    range1.Columns.AutoFit();// 设置列宽为自动适应                   
                    // 设置单元格左边框加粗 
                    range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThick;
                    // 设置单元格右边框加粗 
                    range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThick;
                    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;// 设置单元格水平居中
                    range.Value2 = name;
                    range1.Value2 = "开始日期：" + dateTimePicker1.Value.ToShortDateString() + "结束日期：" + dateTimePicker2.Value.ToShortDateString();
                    range.Font.Size = 18;                        // 设置字体大小 
                    range.Font.ColorIndex = 5;                  // 设置字体颜色                     
                    range.Interior.ColorIndex = 6; // 设置单元格背景色 
                    range.RowHeight = 25;           // 设置行高 
                    range.ColumnWidth = 10;         // 设置列宽
                    for (int i = 0; i < gridView.ColumnCount; i++)
                    {
                        xlSheet.Cells[2, i + 1] = gridView.Columns[i].HeaderText.ToString();

                        range = (Microsoft.Office.Interop.Excel.Range)xlSheet.Cells[1, i + 1];
                        range.Interior.ColorIndex = 15;//背景颜色 
                        range.Font.Bold = true;//粗体 
                        range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//居中 
                        //加边框 
                        range.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, null);
                        //range.ColumnWidth = 4.63;//设置列宽 
                        range.EntireColumn.AutoFit();//自动调整列宽 
                        range.EntireRow.AutoFit();//自动调整行高 


                    }

                    //-----------------------设置单元格--------------------------------------------------------------------------------
                    //标题栏 
                    range = xlSheet.get_Range(xlSheet.Cells[2, 1], xlSheet.Cells[2, gridView.Columns.Count]);
                    range.Interior.ColorIndex = 45;//设置标题背景色为 浅橙色 
                    range.Font.Bold = true;//标题字体加粗

                    for (int i = 0; i < gridView.RowCount; i++)
                    {
                        for (int j = 0; j < gridView.ColumnCount; j++)
                        {
                            if (gridView[j, i].Value == typeof(string))
                            {
                                xlSheet.Cells[i + 3, j + 1] = "" + gridView[i, j].Value.ToString();
                                range = (Microsoft.Office.Interop.Excel.Range)xlSheet.Cells[i + 3, j + 1];
                                range.Font.Size = 9;//字体大小 
                                //加边框 
                                range.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, null);
                                range.EntireColumn.AutoFit();//自动调整列宽 
                            }
                            else
                            {
                                xlSheet.Cells[i + 3, j + 1] = gridView[j, i].Value.ToString();
                                range = (Microsoft.Office.Interop.Excel.Range)xlSheet.Cells[i + 3, j + 1];
                                range.Font.Size = 9;//字体大小 
                                //加边框 
                                range.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, null);
                                range.EntireColumn.AutoFit();//自动调整列宽 
                            }
                        }
                    }
                    range = xlSheet.get_Range(xlSheet.Cells[gridView.Rows.Count + 2, 1], xlSheet.Cells[gridView.Rows.Count + 2, gridView.Columns.Count]);
                    range.Merge(missing);         // 合并单元格 
                    // range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThick;
                    // 设置单元格右边框加粗 
                    range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThick;
                    range.RowHeight = 20;
                    range.Value2 = "导出时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    range.HorizontalAlignment = XlHAlign.xlHAlignRight;// 设置单元格水平居右

                    //***** 格式设定 ******************************************************************************



                    if (xlSheet != null)
                    {
                        xlSheet.SaveAs(dialog.FileName, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                        xlApp.Visible = true;
                    }


                }
                catch (Exception)
                {
                    xlApp.Quit();
                    throw;
                }
            }
        }

        public static void saveAs(DataGridView gridView, string name) //导出Excel
        {
            if (gridView.RowCount < 1)
            {
                MessageBox.Show("没有可导出的数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (gridView.RowCount >300)
            {
                if (MessageBox.Show("导出数据过多，这可能需要几分钟？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    return;
                }
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Filter = "Excel(*.xls)|*.xls|All Files(*.*)|*.*";
            dialog.FileName = "*.xls";
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                object missing = System.Reflection.Missing.Value;
                try
                {
                    if (xlApp == null)
                    {
                        MessageBox.Show("无法创建Excel对象，可能您的计算机未安装Excel");
                        return;
                    }

                    Microsoft.Office.Interop.Excel.Workbooks xlBooks = xlApp.Workbooks;
                    Microsoft.Office.Interop.Excel.Workbook xlBook = xlBooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                    Microsoft.Office.Interop.Excel.Worksheet xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets[1];
                    Microsoft.Office.Interop.Excel.Range range = null;
                    //  Microsoft.Office.Interop.Excel.Range range1 = null;
                    //****** 抬头 *********************************************************************************

                    range = xlSheet.get_Range(xlSheet.Cells[1, 1], xlSheet.Cells[1, gridView.Columns.Count]);

                    //合并单元格
                    range.Merge(missing);
                    //自适应列宽
                    range.Columns.AutoFit();
                    // 设置单元格左边框加粗 
                    //   range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThick;
                    // 设置单元格右边框加粗 
                    //   range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThick;
                    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;// 设置单元格水平居中
                    range.Value2 = name;
                    range.Font.Bold = true;//粗体 
                    //range1.Value2 = "开始日期：" + this.dateTimePicker1.Value.ToShortDateString() + "结束日期：" + this.dateTimePicker2.Value.ToShortDateString();
                    range.Font.Size = 14;                        // 设置字体大小 
                    //   range.Font.ColorIndex = 5;                  // 设置字体颜色                     
                    //   range.Interior.ColorIndex = 6; // 设置单元格背景色 
                    range.RowHeight = 25;           // 设置行高 
                    range.ColumnWidth = 10;         // 设置列宽


                    for (int i = 0; i < gridView.ColumnCount; i++)
                    {
                        xlSheet.Cells[2, i + 1] = gridView.Columns[i].HeaderText.ToString();
                        range = (Microsoft.Office.Interop.Excel.Range)xlSheet.Cells[2, i + 1];
                        //       range.Interior.ColorIndex = 15;//背景颜色
                        range.Font.Size = 12;//字体大小 
                        //加边框 
                        range.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, null);
                        range.EntireColumn.AutoFit();//自动调整列宽 
                        range.EntireRow.AutoFit();//自动调整行高 
                        range.HorizontalAlignment = XlHAlign.xlHAlignCenter;//居中

                    }

                    //-----------------------设置单元格--------------------------------------------------------------------------------
                    //标题栏 
                    //    range = xlSheet.get_Range(xlSheet.Cells[2, 1], xlSheet.Cells[2, gridView.Columns.Count]);
                    ////    range.Interior.ColorIndex = 45;//设置标题背景色为 浅橙色 
                    //    range.Font.Bold = true;//标题字体加粗


                    for (int i = 0; i < gridView.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j < gridView.ColumnCount; j++)
                        {
                            xlSheet.Cells[i + 3, j + 1] = "" + gridView[j, i].Value.ToString();
                            range = (Microsoft.Office.Interop.Excel.Range)xlSheet.Cells[i + 3, j + 1];
                            range.Font.Size = 10;//字体大小 
                            //加边框 
                            range.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, null);
                            range.EntireColumn.AutoFit();//自动调整列宽 
                            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        }

                    }

                    //***** 格式设定 *****************************************************************************
                    if (xlSheet != null)
                    {
                        xlSheet.SaveAs(dialog.FileName, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                     //   xlApp.Visible = true;
                    }


                }
                catch (Exception)
                {
                    // MessageBox.Show(ex.ToString());
                    xlApp.Quit();
                    throw;
                }
                try
                {
                    if (xlApp != null)
                    {
                        int lpdwProcessId;
                        GetWindowThreadProcessId(new IntPtr(xlApp.Hwnd), out lpdwProcessId);
                        System.Diagnostics.Process.GetProcessById(lpdwProcessId).Kill();
                        System.Diagnostics.Process.Start(dialog.FileName); 
                    }
                }
                catch { }

                
            

            }
        }


    }


}
