#include "main.h"
#include <qmap>
#include <QxMemLeak.h>
#include "./DataBase/QxORM/precompiled.h"
#include "./DataBase/QxORM/historydata.h"

using namespace std;
//成员定义
byte DataBuffer[256];
TS7Client *Client;
extern queue<buffstruct> dataprocessqueue;
extern queue<dataobject> datacenterqueue;
map<QString,QString> addr;
map<QString,QString> dbset;


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

/**
 * @brief 数据处理线程
 */
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
            dataobject jsonobject;
            jsonobject.json_string=byte_array;
            datacenterqueue.push(jsonobject);
            cout<<" queue size: "<<dataprocessqueue.size()<<endl;
        }
    }
}

/**
 * @brief //读PLC线程
 */
void ReadThread::readcycle()
{
    Client=new TS7Client();
    Client->ConnectTo("192.168.1.50",0,0);
    QString Key,Value;
    int start,end,range,wordlen;
    while(1)
    {
        for(auto &kv : addr)
        {
            struct dataobject st;
            Key=kv.first;Value=kv.second;
            if(Key.contains("_"))
            {
                start=Value.split("-")[0].toInt();//起始地址
                end=Value.split("-")[1].toInt();//结束地址
                range=end-start+1;//数据长度
            }
            else
            {
                start=Value.toInt();
                end=start;
                range=1;
            }
            if(Key.contains("W"))
            {
                wordlen=2;
            }
            else if(Key.contains("B"))
            {
                wordlen=1;
            }
            else if(Key.contains("D"))
            {
                wordlen=3;
            }

            if(Key.contains("DB"))
            {
                if(Client->DBRead(1,start,range,&DataBuffer)==0)
                    bufferconverter::datadump(st.data,QString("DB"),start,range,wordlen,DataBuffer);//数据存储到st中字典
                continue;
            }
            else if(Key.contains("M"))
            {
                if(Client->MBRead(start,range,&DataBuffer)==0)
                    bufferconverter::datadump(st.data,QString("M"),start,range,wordlen,DataBuffer);
                continue;
            }
            //......
        }
        QThread::msleep(1000);
    }
}



/**
 * @brief 数据上传线程
 */
void DatacenterThread::datastorage()
{
    QSqlDatabase db = QSqlDatabase::addDatabase("QMYSQL", "My_connection");
        db.setDatabaseName(dbset["dbname"]);
        db.setHostName(dbset["ip"]);
        db.setUserName(dbset["user"]);
        db.setPassword(dbset["password"]);
        if (!db.open())
        {
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
                dataobject jsonobject=datacenterqueue.front();
                query.addBindValue(jsonobject.json_string);
                query.addBindValue(jsonobject.current_date_time);
                bool result=query.exec();
                if(!result)
                qDebug() << "error: " << query.lastError().text();
                qDebug()<<result;
            }
        }
}

/**
 * @brief 初始化
 */
void init()
{
    addr=inimanager::readini("./Config/address","AreaDB");//读取DB配置文件
    dbset=inimanager::readini("./Config/database","dbset");//读数据库配置文件
}
/**
 * @brief 数据上传ORM
 */
void DatacenterThread::datastorage_orm()
{
    qx::QxSqlDatabase::getSingleton()->setDriverName("QMYSQL");
    qx::QxSqlDatabase::setHostName(dbset["ip"]);//"192.168.1.25"
    qx::QxSqlDatabase::setDatabaseName(dbset["dbname"]);
    qx::QxSqlDatabase::getSingleton()->setUserName(dbset["user"]);
    qx::QxSqlDatabase::setPassword(dbset["password"]);

    while(1)
    {
        if(!datacenterqueue.empty())
        {
            try
            {
                dataobject jsonobject=datacenterqueue.front();
                historydata data=new historydata();
                data.json=jsonobject.json_string;
                data.time=jsonobject.current_date_time;
                qx::dao::insert(data);
                delete(data);
            }
            catch
            {
                delete(data);
                qDebug("数据库上传错误");
            }

        }
    }
}
/**
 * @brief 主函数
 * @param argc
 * @param argv
 * @return
 */
int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);
    init();


   // sqltest();

//    ReadThread readthread;
//    ProcessThread processthread;
// //   QObject::connect(&readthread,SIGNAL(finished()),&a,SLOT(quit()));
// //   QObject::connect(&processthread,SIGNAL(finished()),&a,SLOT(quit()));

//    cout << "Start..." << endl;
//    readthread.start();
//    processthread.start();
//    readthread.wait();
//    processthread.wait();
    return a.exec();;
}

//#include "main.moc"



