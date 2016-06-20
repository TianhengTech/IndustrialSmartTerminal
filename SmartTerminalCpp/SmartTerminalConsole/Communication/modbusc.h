#ifndef MODBUSC_H
#define MODBUSC_H

#include <QString>
#include "libmodbus/modbus.h"

using namespace std;
class modbusc
{
public:
    modbusc();
    modbus_t* modbus_rtu(string port,int baud,char parity,int data_bit,int stop_bit);
    modbus_t* modbus_tcp(const char *ip, int port);
    int mb_connect();
    int mb_set_slave( int slave);
    int mb_read_registers( int addr, int nb, uint16_t *dest);
    int mb_read_input( int addr, int nb, uint8_t *dest);
    int mb_read_inputregisters( int addr, int nb, uint16_t *dest);
    int mb_read_coil( int addr, int nb, uint8_t *dest);
    int mb_write_registers( int addr, int nb, const uint16_t *src);
    int mb_write_register(int addr, int value);
    int mb_write_coil(int addr,int status);
    int mb_write_coils(int addr, int nb, const uint8_t *src);
    void mb_close();
    void mb_freemb();
    int mb_flush();
    int mb_responsetimeout(int sec,int usec);
};

#endif // MODBUSC_H
