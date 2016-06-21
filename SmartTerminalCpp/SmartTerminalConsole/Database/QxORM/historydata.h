#ifndef _QX_ST_HISTORYDATA_H_
#define _QX_ST_HISTORYDATA_H_

#include <QString>
#include <QDateTime>
#include "precompiled.h"

class historydata
{
public:
    long id;
    QString json;
    QDateTime time;
    historydata():id(0){;}
    virtual ~historydata(){;}
};
QX_REGISTER_HPP_SMARTTERMINAL(historydata, qx::trait::no_base_class_defined, 0)

#endif // _QX_ST_HISTORYDATA_H_
