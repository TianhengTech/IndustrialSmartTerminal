#include "fileeditor.h"

#include <QFile>
#include<QDataStream>
#include<QTextStream>
fileeditor::fileeditor()
{

}
//写入文件
/**
 * @brief 写文件
 * @param 文件名
 * @param 写入的内容
 * @return
 */
bool fileeditor::writefile(QString filename, QString content)
{
    QFile file(filename);
    if (!file.open(QIODevice::WriteOnly | QIODevice::Text))
        return false;

    QTextStream out(&file);
    out << content;
    return true;
}
//读文件
/**
 * @brief 读文件
 * @param 文件名
 * @return 文件内容
 */
QString fileeditor::readfile(QString filename)
{
    QFile file(filename);
     if (!file.open(QIODevice::ReadOnly | QIODevice::Text))
         return "";

     QTextStream in(&file);
     QString line = in.readLine();
     while (!line.isNull()) {
         line.append(in.readLine());
     }
     return line;
}
