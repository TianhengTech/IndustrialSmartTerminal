#ifndef SNAP7CLIENT_H
#define SNAP7CLIENT_H
#define NUM_THREADS 5

struct buffstruct
{
    byte buffer[256];
};
#include <QString>
#include<QDateTime>
struct jsonstring
{
    QString json_string;
    QDateTime current_date_time = QDateTime::currentDateTime();
};

class snap7Client
{
    public:
        snap7Client();
        virtual ~snap7Client();
        void Usage();
        int ConnectTo();

    protected:

    private:
};

#endif // SNAP7CLIENT_H
