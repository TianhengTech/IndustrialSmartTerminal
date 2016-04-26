using System.Data;
using System.IO;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace BaseClass.FileEditor
{
    internal class ExcelExport
    {
        /// <summary>
        ///     DataTable To MemoryStream
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MemoryStream RenderToExcel(DataTable table)
        {
            var ms = new MemoryStream();
            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();
                {
                    var si = PropertySetFactory.CreateSummaryInformation();
                    var sheet = workbook.CreateSheet();
                    {
                        var headerRow = sheet.CreateRow(0);
                        //// handling header.
                        foreach (DataColumn column in table.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
                            ////If Caption not set, returns the ColumnName value
                        }
                        //// handling value.
                        var rowIndex = 1;
                        foreach (DataRow row in table.Rows)
                        {
                            var dataRow = sheet.CreateRow(rowIndex);
                            foreach (DataColumn column in table.Columns)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                            }
                            rowIndex++;
                        }
                        workbook.Write(ms);
                        ms.Flush();
                        ms.Position = 0;
                    }
                }
            }
            return ms;
        }

        /// <summary>
        ///     Save Stream to File
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="fileName"></param>
        public static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                data = null;
            }
        }
    }
}