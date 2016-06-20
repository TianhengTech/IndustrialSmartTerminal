#ifndef BUFFERCONVERTER_H
#define BUFFERCONVERTER_H
#include "common.h"
typedef unsigned char byte;
class bufferconverter
{
public:
    bufferconverter();
    void static datadump(QMap<QString,vector<byte>> &datamap,QString area,int start,int range,int wordlen,byte buffer[]);
    void static reverseArray(byte inarray[],int count);
};

#endif // BUFFERCONVERTER_H
