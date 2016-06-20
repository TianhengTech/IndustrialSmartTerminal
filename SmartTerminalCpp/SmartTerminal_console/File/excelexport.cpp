#include "excelexport.h"
#include <QAxObject>
#include <QDebug>
ExcelExport::ExcelExport()
{

}
void ExcelExport::fexport(QString filename,QStringList rowcontent,int nocolumn)
{
    QAxObject excel("Excel.Application");
    excel.setProperty("Visible", true);
    QAxObject *work_books = excel.querySubObject("WorkBooks");
    work_books->dynamicCall("Open (const QString&)", QString("sheets.xlsx"));
    QVariant title_value = excel.property("Caption");  //获取标题
    qDebug()<<QString("excel title : ")<<title_value;
    QAxObject *work_book = excel.querySubObject("ActiveWorkBook");
    //QAxObject *work_sheets = work_book->querySubObject("Sheets");  //Sheets也可换用WorkSheets

    QAxObject *work_sheet = work_book->querySubObject("Sheets(int)", 0);
    for(int i =0 ;i<rowcontent.count();i++)
    {
        for(int j =0;j<nocolumn;j++)
        {
            QAxObject *cell = work_sheet->querySubObject("Cells(int,int)", i, j);
            cell->setProperty("Value", rowcontent[i]);  //设置单元格值
        }
    }
    work_book->dynamicCall("SaveAs(const QString&)", filename);  //另存为另一个文件
    work_book->dynamicCall("Close(Boolean)", false);  //关闭文件
    excel.dynamicCall("Quit(void)");  //退出

}
