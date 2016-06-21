#ifndef MAIN_H
#define MAIN_H
#endif // MAIN_H
#include <QCoreApplication>
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include "Communication/snap7.h"
#include "Communication/snap7client.h"
#include "Ultility/common.h"
#include <QtCore>
#include <QThread>
#include <QJsonDocument>
#include <QJsonObject>
#include <queue>
#include <map>
#include<QtSql/QSqlDatabase>
#include <QDebug>
#include <QSqlQuery>
#include <QSqlError>
#include <Logger/logger.h>
#include<Config/inimanager.h>
#include "Ultility/bufferconverter.h"
#include <vector>

using namespace std;

class ReadThread:public QThread
{
    Q_OBJECT
public:
    void readcycle();
    void datadump(dataobject st,QString area,int start,int range,int wordlen,byte buffer[]);
protected:
    void run();
};

class ProcessThread:public QThread
{
    Q_OBJECT
public:
    void dataprocess();
protected:
    void run();
};

class DatacenterThread:public QThread
{
    Q_OBJECT
public:
    void datastorage();
    void datastorage_orm();
protected:
    void run();
};

