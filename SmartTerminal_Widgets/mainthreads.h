#ifndef MAINTHREADS_H
#define MAINTHREADS_H
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include "snap7.h"
#include <QtCore>
#include <QThread>
#include <QJsonDocument>
#include <QJsonObject>
#include <queue>
#include <QtSql/QSqlDatabase>
#include <QDebug>
#include <QSqlError>
#include <QSqlQuery>

struct buffstruct
{
    byte buffer[256];
};
#include <QString>
#include<QDateTime>
struct jsonstring
{
    QString json_string;
    QDateTime current_date_time = QDateTime::currentDateTime();
};
class ReadThread:public QThread
{
    Q_OBJECT
public:
    void readcycle();
protected:
    void run();
};

class ProcessThread:public QThread
{
    Q_OBJECT
public:
    void dataprocess();
    ~ProcessThread();
protected:
    void run();
signals:
    void signal_showvalue(QString);
};

class DatacenterThread:public QThread
{
    Q_OBJECT
public:
    void datastorage();
protected:
    void run();
};

#endif // MAINTHREADS_H
