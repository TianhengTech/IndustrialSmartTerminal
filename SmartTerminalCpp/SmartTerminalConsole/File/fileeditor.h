#ifndef FILEEDITOR_H
#define FILEEDITOR_H

#include <QString>
class fileeditor
{
public:
    fileeditor();
    bool writefile(QString filename,QString content);
    QString readfile(QString file);

};

#endif // FILEEDITOR_H
