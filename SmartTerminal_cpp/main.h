#ifndef MAIN_H
#define MAIN_H
#include <QThread>
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

#endif // MAIN_H
