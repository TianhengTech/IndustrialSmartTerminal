#include "serialcom.h"

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
void serialcom::openport()
{
    my_serialport->open(QIODevice::ReadWrite);
}

void serialcom::write(QString data)
{
    my_serialport->write(data.toStdString().c_str());
}
void serialcom::my_readart()
{
    QByteArray requestData;
    requestData = my_serialport->readAll();
}

serialcom::~serialcom()
{
    my_serialport->close();
    delete my_serialport;
}
