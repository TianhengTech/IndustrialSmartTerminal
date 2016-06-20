#ifndef COMMON_H
#define COMMON_H
#include<QString>
#include<QTime>
#include <queue>
#include "Communication/snap7.h"
#include <QMap>
#include <vector>
using namespace std;
struct buffstruct
{
    byte buffer[256];
};

struct dataobject
{

    QString json_string;
    QMap<QString,vector<byte>> data;
    QDateTime current_date_time = QDateTime::currentDateTime();
};

#endif // COMMON_H
