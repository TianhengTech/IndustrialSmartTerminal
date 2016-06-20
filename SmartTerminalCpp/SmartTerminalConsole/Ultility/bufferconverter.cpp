#include "bufferconverter.h"


bufferconverter::bufferconverter()
{

}

//将字节数据以VECTOR的形式保存在字典中
void bufferconverter::datadump(QMap<QString,vector<byte>> &datamap,QString area,int start,int range,int wordlen,byte buffer[])
{
        QString wtype="BWD";
        QString key;
        for(int i=0;i<range;i=i+wordlen)
        {
            key=area+wtype.at(sqrt(wordlen))+QString::number(start+i);
            vector<byte> bytes(range);
            bytes[0]=1;
            for(int j=0;j<wordlen;j++)
            {
                bytes[j]=buffer[i+wordlen-1-j];
            }
            datamap[key]=bytes;
        }
}

void bufferconverter::reverseArray(byte inarray[],int count)
{
    int temp, i, j, k = (count - 1) / 2;
    for (i = 0; i < k; i++)
    {
        j = count - 1 - i;
        temp = inarray[i];
        inarray[i] = inarray[j];
        inarray[j] = temp;
    }
}
