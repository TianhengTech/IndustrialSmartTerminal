#include "bufferconverter.h"


bufferconverter::bufferconverter()
{

}

/**
 * @brief 将字节数据VECTOR的形式保存在字典中,VECTOR为字节数组
 * @param datamap 要存入的字典
 * @param area PLC数据区域
 * @param start 起始地址
 * @param range 数据总长度
 * @param wordlen 单个数据块长度(以字节计）
 * @param buffer 数据缓存
 */
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
/**
 * @brief 反转数组
 * @param 数组
 * @param 长度
 */
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
