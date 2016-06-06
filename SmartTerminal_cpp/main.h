#ifndef MAIN_H
#define MAIN_H
#endif // MAIN_H
#include <QCoreApplication>
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include "snap7.h"
#include "snap7client.h"
#include <QtCore>
#include <QThread>
#include <QJsonDocument>
#include <QJsonObject>
#include <queue>
#include <map>


using namespace std;
//extern queue<byte[256]> dataprocessqueue;
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

