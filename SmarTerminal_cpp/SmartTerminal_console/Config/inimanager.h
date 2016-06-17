#ifndef INIMANAGER_H
#define INIMANAGER_H
#include <QSettings>
#include <map>
using namespace std;
class inimanager
{
public:
    inimanager();
    void static writeini(QString name,QString para,map<QString,QString> inimap);
    map<QString,QString> static readini(QString name,QString para);
};

#endif // INIMANAGER_H
