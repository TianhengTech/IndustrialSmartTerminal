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
#include<Ultility/terminalqueues.h>
#include <Logger/logger.h>
#include<Config/inimanager.h>
using namespace std;
extern map<int,string> mapstudent;

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
protected:
    void run();
};

class DatacenterThread:public QThread
{
    Q_OBJECT
public:
    void datastorage();
protected:
    void run();
};

