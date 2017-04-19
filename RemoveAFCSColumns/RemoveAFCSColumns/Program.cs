using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace RemoveAFCSColumns
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\CCrowe\Documents\AFCS Folder\Facilities");
            foreach (var file in di.GetFiles("*xls*"))
            {
                Excel.Application xl = new Excel.Application();
                xl.Visible = false;
                xl.DisplayFullScreen = true;
                xl.AskToUpdateLinks = false;
                Excel.Workbook wb = xl.Workbooks.Open(file.FullName);
                xl.AskToUpdateLinks = false;
                Excel.Worksheet res = wb.Sheets["Resources"];
                res.Activate();
                int lastColumnDeleted = 19;
                for (int i = 18; i < 40; i++)
                {
                    if ((res.Cells[1, i] as Excel.Range).Value != null)
                    {
                        string columnLabel = res.Cells[1, i].Value2.ToString();
                        if (columnLabel.Contains("EQUIPMENT #") && columnLabel.Contains("HRS"))
                        {
                            (res.Columns[i] as Excel.Range).Delete();
                            lastColumnDeleted = i-1;
                            i = i - 1;
                        }
                    }
                }
                int lastRow = GetLastRow(res);
                res.Range[res.Cells[1, lastColumnDeleted], res.Cells[2, lastColumnDeleted]]
                    .Borders[Excel.XlBordersIndex.xlEdgeRight]
                    .Weight = Excel.XlBorderWeight.xlMedium;
                res.Range[res.Cells[3, lastColumnDeleted], res.Cells[lastRow, lastColumnDeleted]]
                    .Borders[Excel.XlBordersIndex.xlEdgeRight]
                    .Weight = Excel.XlBorderWeight.xlThin;
                res.Range[res.Cells[2, 1], res.Cells[2, lastColumnDeleted]]
                    .Borders[Excel.XlBordersIndex.xlEdgeRight]
                    .Weight = Excel.XlBorderWeight.xlMedium;
                for (int i = lastColumnDeleted + 1; i <= 50; i++)
                {
                    (res.Columns[i] as Excel.Range).Delete();
                }
                (res.Cells[1, lastColumnDeleted] as Excel.Range).Select();
                (res.Cells[lastRow, lastColumnDeleted] as Excel.Range).Select();
                res.Range["A1"].Select();
                (wb.Sheets["Facility"] as Excel.Worksheet).Activate();
                BorderTest(res, lastColumnDeleted,xl);
                wb.Save();
                wb.Close(true);
                xl.Quit();
            }
        }
        static int GetLastRow(Excel.Worksheet res)
        {
            int lastRow = res.UsedRange.Rows.Count - 6;
            for (int i = 1; i <= res.UsedRange.Rows.Count; i++)
            {
                if (res.Range["A" + i.ToString()] != null)
                {
                    if (res.Range["A" + i.ToString()].Value2 == "Key - Editable Columns are bold")
                    {
                        lastRow = i - 2;
                    }   
                }
            }
            return lastRow;
        }

        static void BorderTest(Excel.Worksheet res, int lastColumn,Excel.Application xl)
        {
            var test = HasBorder(res.Range["E" + 4.ToString()]);
            for (int i = 1; i <= res.UsedRange.Rows.Count; i++)
            {
                if (HasBorder(res.Range["D" + i.ToString()]))
                {
                    if (!HasBorder(res.Cells[i,lastColumn]))
                    {
                        xl.Visible = true;
                        MessageBox.Show("No Border:" + i.ToString());
                        xl.Visible = false;
                    }
                }
                if (HasBorder(res.Cells[i, lastColumn]))
                {
                    if (!HasBorder(res.Range["D" + i.ToString()]))
                    {
                        xl.Visible = true;
                        MessageBox.Show("Border goes too far" + i.ToString());
                        xl.Visible = false;
                    }
                }
            }
        }

        static bool HasBorder(Excel.Range range)
        {
            var v1 = range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle;
            var v2 = (int)Excel.XlLineStyle.xlContinuous;
            if (range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle == (int)Excel.XlLineStyle.xlContinuous)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
