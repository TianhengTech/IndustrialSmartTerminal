#include <QCoreApplication>
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include "snap7.h"
#include "snap7client.h"
#include <QtCore>
#include <QThread>
#include "main.h"
using namespace std;
byte MyDB32[256];
TS7Client *Client;

void ReadThread::run()
{
    this->readcycle();
}
void ProcessThread::run()
{
    dataprocess();
}
void ProcessThread::dataprocess()
{
    while(1)
    {
        cout<<"process"<<endl;
        sleep(1);
    }

}

void ReadThread::readcycle()
{
    Client=new TS7Client();
    int result=Client->ConnectTo("192.168.1.50",0,0);
    Client->DBRead(1,0,16,&MyDB32);
    for(int i =0;i<sizeof(MyDB32);i++)
    {
        cout<<(int)MyDB32[i]<<endl;
    }
    cout<<result;
    getchar();
    delete Client;
    return;
}

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);
    ReadThread readthread;
    ProcessThread processthread;
    QObject::connect(&readthread,SIGNAL(finished()),&a,SLOT(quit()));
    QObject::connect(&processthread,SIGNAL(finished()),&a,SLOT(quit()));

    cout << "Hello world!" << endl;
    readthread.start();
    processthread.start();
    readthread.wait();
    processthread.wait();

    return a.exec();
}

//#include "main.moc"



