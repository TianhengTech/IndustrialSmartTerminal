#ifndef EXCELEXPORT_H
#define EXCELEXPORT_H
using namespace std;
#include <QString>
class ExcelExport
{
public:
    ExcelExport();
    void fexport(QString filename,QStringList rowcontent,int nocolumn);
};

#endif // EXCELEXPORT_H
