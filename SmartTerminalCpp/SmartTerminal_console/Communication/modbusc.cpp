#include "modbusc.h"
#include "libmodbus/modbus.h"
modbus_t *mb;

modbusc::modbusc()
{

}
modbus_t * modbusc::modbus_rtu(string port,int baud=9600,char parity='N',int data_bit=8,int stop_bit=1)
{
    mb=modbus_new_rtu(port.c_str(),baud,parity,data_bit,stop_bit);
    return mb;
}
modbus_t * modbusc::modbus_tcp(const char *ip,int port)
{
    mb=modbus_new_tcp(ip,port);
    return mb;
}
int modbusc::mb_set_slave(int slave)
{
    return modbus_set_slave(mb,slave);
}
int modbusc::mb_connect()
{
    return modbus_connect(mb);
}
int modbusc::mb_read_coil(int addr, int nb, uint8_t *dest)
{
    return modbus_read_bits(mb,addr,nb,dest);
}
int modbusc::mb_read_input(int addr, int nb, uint8_t *dest)
{
    return modbus_read_input_bits(mb,addr,nb,dest);
}
int modbusc::mb_read_inputregisters(int addr, int nb, uint16_t *dest)
{
    return modbus_read_input_registers(mb,addr,nb,dest);
}
int modbusc::mb_read_registers(int addr, int nb, uint16_t *dest)
{
    return modbus_read_registers(mb,addr,nb,dest);
}
int modbusc::mb_write_coil(int addr, int status)
{
    return modbus_write_bit(mb,addr,status);
}
int modbusc::mb_write_coils(int addr, int nb, const uint8_t *src)
{
    return modbus_write_bits(mb,addr,nb,src);
}
int modbusc::mb_write_registers(int addr, int nb, const uint16_t *src)
{
    return modbus_write_registers(mb,addr,nb,src);
}
int modbusc::mb_write_register(int addr, int value)
{
    return modbus_write_register(mb,addr,value);
}
int modbusc::mb_responsetimeout(int sec, int usec)
{
    return modbus_set_response_timeout(mb,sec,usec);
}

void modbusc::mb_close()
{
     modbus_close(mb);
}
int modbusc::mb_flush()
{
    return modbus_flush(mb);
}
void modbusc::mb_freemb()
{
    modbus_free(mb);
}


