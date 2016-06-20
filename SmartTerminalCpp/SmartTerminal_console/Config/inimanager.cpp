#include "inimanager.h"
#include <QDebug>
using namespace std;
//写配置文件
void inimanager::writeini(QString name,QString para,map<QString,QString> inimap)
{
    QSettings cfgIniWrite(name+".ini",QSettings::IniFormat);
    cfgIniWrite.beginGroup(para);
    for(auto &kv:inimap)
    {
        cfgIniWrite.setValue(kv.first,kv.second);
    }

}
//读配置文件
map<QString,QString> inimanager::readini(QString name,QString para)
{
    QSettings cfgIniRead(name+".ini",QSettings::IniFormat);
    cfgIniRead.beginGroup(para);
    QStringList allkeys=cfgIniRead.allKeys();
    map<QString,QString> inimap;
    for(int i=0;i<allkeys.count();i++)
    {
       inimap[allkeys[i]]=cfgIniRead.value(allkeys[i]).toString();
    }
    cfgIniRead.endGroup();
    return inimap;
}
