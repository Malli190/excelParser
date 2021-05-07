using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExelParser.Sett
{
    public class kp_creator
    {
        // Класс создающий комерческое предложение на основе входных данных
        public static string save_folder;

        public static string default_folder = "KP";
        public static string save_path = $"{get_folder()}/kp.xlsx";

        string[] cellsName = { "A", "B", "C", "D", "E", "F", "G", "H" };
        
        public kp_creator()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public void create_excel(DataTable table, string projName)
        {
            if (File.Exists(save_path))
                File.Delete(save_path);

            FileInfo fileInfo = new FileInfo(save_path);
            ExcelPackage kp_package = new ExcelPackage(fileInfo);

            var activeBook = kp_package.Workbook.Worksheets.Add("Комерческое предложение");

            int startRow = 3;
            activeBook.Column(1).Width = 5;
            activeBook.Column(2).Width = 10;
            activeBook.Column(3).Width = 40;
            activeBook.Column(4).Width = 10;
            activeBook.Cells["A1"].Value = "Комерческое предложение";
            activeBook.Cells["A2"].Value = "Проект:";
            activeBook.Cells["B2"].Value = projName;
            activeBook.Cells[$"A2:{cellsName[cellsName.Length - 1]}2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            for (int r = 0; r < table.Rows.Count; r++)
            {
                for (int c = 0; c < cellsName.Length; c++)
                {
                    activeBook.Cells[cellsName[c] + startRow.ToString()].Value = table.Rows[r].ItemArray[c];
                }

                startRow++;
            }
            activeBook.Cells["A" + startRow].Value = "-";
            
            startRow++;
            
            activeBook.Cells["A" + startRow].Value = "Дата формирования:";
            activeBook.Cells["C" + startRow].Style.Numberformat.Format = "@";
            activeBook.Cells["C" + startRow].Value = DateTime.Now.ToString();
            
            kp_package.Save();
        }
        static string get_folder()
        {
            string result = save_folder != null ? save_folder : default_folder;

            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);

            return result;
        }
    }
}
