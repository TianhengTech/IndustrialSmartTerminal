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
    void  getlogger(QtMsgType type, const QMessageLogContext &context, const QString &msg);
    void  setlogger();
};

#endif // LOGGER_H
