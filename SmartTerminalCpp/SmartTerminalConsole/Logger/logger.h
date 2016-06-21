#ifndef LOGGER_H
#define LOGGER_H
#include <stdlib.h>
#include <stdio.h>
#include <qcoreapplication.h>
#include <qfile.h>
#include <qmutex.h>
#include <qtextstream.h>

class logger
{
public:
    void  setlogger();
private:
    void  getlogger(QtMsgType type, const QMessageLogContext &context, const QString &msg);
};

#endif // LOGGER_H
