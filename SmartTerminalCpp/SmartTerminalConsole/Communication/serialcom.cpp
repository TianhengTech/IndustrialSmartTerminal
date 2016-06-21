#include "serialcom.h"
/**
 * @brief 串口连接初始化
 * @param portname
 * @param baud
 * @param databits
 * @param parity
 * @param stopbits
 * @param flowcontrol
 */
serialcom::serialcom(QString portname,int baud,QSerialPort::DataBits databits,
                     QSerialPort::Parity parity,QSerialPort::StopBits stopbits,
                     QSerialPort::FlowControl flowcontrol)
{
    my_serialport->setPortName(portname);
    my_serialport->setDataBits(databits);
    my_serialport->setBaudRate(baud);
    my_serialport->setFlowControl(flowcontrol);
    my_serialport->setParity(parity);
    my_serialport->setStopBits(stopbits);
    //QObject::connect(my_serialport,SIGNAL(readyRead()),this,SLOT(my_readuart()));

}
/**
 * @brief 打开串口
 */
void serialcom::openport()
{
    my_serialport->open(QIODevice::ReadWrite);
}
/**
 * @brief 串口写数据
 * @param data
 */
void serialcom::write(QString data)
{
    my_serialport->write(data.toStdString().c_str());
}
/**
 * @brief 读串口数据
 */
void serialcom::my_readart()
{
    QByteArray requestData;
    requestData = my_serialport->readAll();
}
/**
 * @brief serialcom::~serialcom
 */
serialcom::~serialcom()
{
    my_serialport->close();
    delete my_serialport;
}
