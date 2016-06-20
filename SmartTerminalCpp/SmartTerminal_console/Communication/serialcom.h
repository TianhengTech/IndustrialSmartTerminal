#ifndef SERIALCOM_H
#define SERIALCOM_H
#include <QtSerialPort/QSerialPort>
#include <QtSerialPort/QSerialPortInfo>
//串口通信类
class serialcom
{
public:
    serialcom(QString portname,int baud=9600,QSerialPort::DataBits databit=QSerialPort::Data8,
              QSerialPort::Parity parity=QSerialPort::NoParity,
              QSerialPort::StopBits stopbit=QSerialPort::OneStop,
              QSerialPort::FlowControl flowcontrol=QSerialPort::NoFlowControl);
    void write(QString data);
    void openport();
private:
    QSerialPort *my_serialport;
    ~serialcom();
private slots:
    void my_readart();//串口接收数据槽

};

#endif // SERIALCOM_H
