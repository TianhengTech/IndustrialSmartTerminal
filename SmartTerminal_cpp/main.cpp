#include "main.h"
#include<QtSql/QSqlDatabase>
#include <QDebug>
#include <QSqlQuery>
#include <QSqlError>

using namespace std;
byte MyDB32[256];
TS7Client *Client;
queue<buffstruct> dataprocessqueue;
queue<jsonstring> datacenterqueue;

void ReadThread::run()
{
    readcycle();
}
void ProcessThread::run()
{
    dataprocess();
}
void DatacenterThread::run()
{
    datastorage();
}
//数据处理线程
void ProcessThread::dataprocess()
{
    while(1)
    {

        if(!dataprocessqueue.empty())
        {
            struct buffstruct st;
            st=dataprocessqueue.front();
            dataprocessqueue.pop();
            QJsonObject json;
            for(uint i=0;i<16;i++)
            {
                cout<<(int)st.buffer[i]<<endl;
                json.insert("DB"+QString(i),(int)st.buffer[i]);
            }
            QJsonDocument document;
            document.setObject(json);
            QByteArray byte_array = document.toJson(QJsonDocument::Compact);
            jsonstring jsonobject;
            jsonobject.json_string=byte_array;
            datacenterqueue.push(jsonobject);
            cout<<" queue size: "<<dataprocessqueue.size()<<endl;
        }
    }
}
//读PLC线程
void ReadThread::readcycle()
{
    Client=new TS7Client();
    int result=Client->ConnectTo("192.168.1.50",0,0);
    while(1)
    {
        Client->DBRead(1,0,16,&MyDB32);
//        for(uint i =0;i<sizeof(MyDB32);i++)
//        {
//            cout<<(int)MyDB32[i]<<endl;
//        }
        struct buffstruct st;
        memcpy(st.buffer,MyDB32,20);
        dataprocessqueue.push(st);
        QThread::msleep(1000);
    }
    delete Client;
    return;
}

//数据上传线程
void DatacenterThread::datastorage()
{
    QSqlDatabase db = QSqlDatabase::addDatabase("QMYSQL", "My_connection");
        db.setDatabaseName("terminal_test");
        db.setHostName("192.168.1.25");
        db.setUserName("root");
        db.setPassword("tianheng123");
        if (!db.open()) {
            qDebug() << "Connect to MySql error: " << db.lastError().text();
            return;
        }
        QSqlQuery query = QSqlQuery(db);
        QString  sql="insert into historydata (json_string,storetime) values (?,?);";
        query.prepare(sql);
        while(1)
        {
            if(!datacenterqueue.empty())
            {
                jsonstring jsonobject=datacenterqueue.front();
                query.addBindValue(jsonobject.json_string);
                query.addBindValue(jsonobject.current_date_time);
                bool result=query.exec();
                if(!result)
                qDebug() << "error: " << query.lastError().text();
                qDebug()<<result;

            }

        }


}

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

   // sqltest();
    ReadThread readthread;
    ProcessThread processthread;
 //   QObject::connect(&readthread,SIGNAL(finished()),&a,SLOT(quit()));
 //   QObject::connect(&processthread,SIGNAL(finished()),&a,SLOT(quit()));

    cout << "Start..." << endl;
    readthread.start();
    processthread.start();
    readthread.wait();
    processthread.wait();
    return a.exec();;
}

//#include "main.moc"



