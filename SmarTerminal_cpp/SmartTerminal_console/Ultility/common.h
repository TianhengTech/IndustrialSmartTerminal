#ifndef COMMON_H
#define COMMON_H
#include<QString>
#include<QTime>
#include <queue>
#include "Communication/snap7.h"


struct buffstruct
{
    byte buffer[256];
};

struct jsonstring
{
    QString json_string;
    QDateTime current_date_time = QDateTime::currentDateTime();
};

#endif // COMMON_H
