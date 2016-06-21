#include "modbusc.h"
#include "libmodbus/modbus.h"
modbus_t *mb;

modbusc::modbusc()
{

}
/**
 * @brief MODBUS RTU初始化
 * @param port
 * @param baud
 * @param parity
 * @param data_bit
 * @param stop_bit
 * @return MODBUS对象指针
 */
modbus_t * modbusc::modbus_rtu(string port,int baud=9600,char parity='N',int data_bit=8,int stop_bit=1)
{
    mb=modbus_new_rtu(port.c_str(),baud,parity,data_bit,stop_bit);
    return mb;
}
/**
 * @brief MODBUS_TCP初始化
 * @param ip
 * @param port
 * @return MODBUS对象指针
 */
modbus_t * modbusc::modbus_tcp(const char *ip,int port)
{
    mb=modbus_new_tcp(ip,port);
    return mb;
}
/**
 * @brief 设置从机号
 * @param slave
 * @return 操作影响返回值，若为-1则失败并置errno
 */
int modbusc::mb_set_slave(int slave)
{
    return modbus_set_slave(mb,slave);
}
/**
 * @brief 尝试连接
 * @return 操作影响返回值，若为-1则失败并置errno
 */
int modbusc::mb_connect()
{
    return modbus_connect(mb);
}
/**
 * @brief 读线圈
 * @param addr 线圈地址
 * @param nb 要读的字节数
 * @param dest 缓存区指针
 * @return 操作影响返回值，若为-1则失败并置errno
 */
int modbusc::mb_read_coil(int addr, int nb, uint8_t *dest)
{
    return modbus_read_bits(mb,addr,nb,dest);
}
/**
 * @brief 读输入
 * @param addr 输入地址
 * @param nb 要读的字节数
 * @param dest 缓存区指针
 * @return 操作影响返回值，若为-1则失败并置errno
 */
int modbusc::mb_read_input(int addr, int nb, uint8_t *dest)
{
    return modbus_read_input_bits(mb,addr,nb,dest);
}
/**
 * @brief 读输入寄存器
 * @param addr 寄存器地址
 * @param nb 要读的寄存器数量
 * @param dest 缓存指针(字）
 * @return 操作影响返回值，若为-1则失败并置errno
 */

int modbusc::mb_read_inputregisters(int addr, int nb, uint16_t *dest)
{
    return modbus_read_input_registers(mb,addr,nb,dest);
}
/**
 * @brief 读保持寄存器
 * @param addr 寄存器地址
 * @param nb 要读的寄存器数量
 * @param dest 缓存指针(字）
 * @return 操作影响返回值，若为-1则失败并置errno
 */
int modbusc::mb_read_registers(int addr, int nb, uint16_t *dest)
{
    return modbus_read_registers(mb,addr,nb,dest);
}
/**
 * @brief 写线圈
 * @param addr 线圈地址
 * @param status 线圈状态，必须为TRUE或FALSE
 * @return 操作影响返回值，若为-1则失败并置errno
 */
int modbusc::mb_write_coil(int addr, int status)
{
    return modbus_write_bit(mb,addr,status);
}
/**
 * @brief 写多个线圈
 * @param addr 线圈地址
 * @param nb 线圈数量
 * @param src TRUE或FALSE数组
 * @return 操作影响返回值，若为-1则失败并置errno
 */
int modbusc::mb_write_coils(int addr, int nb, const uint8_t *src)
{
    return modbus_write_bits(mb,addr,nb,src);
}
/**
 * @brief 写多个保持寄存器
 * @param addr 寄存器地址
 * @param nb 寄存器数量
 * @param src 写入值
 * @return 操作影响返回值，若为-1则失败并置errno
 */
int modbusc::mb_write_registers(int addr, int nb, const uint16_t *src)
{
    return modbus_write_registers(mb,addr,nb,src);
}
/**
 * @brief 写单个保持寄存器
 * @param addr 寄存器地址
 * @param value 值
 * @return 操作影响返回值，若为-1则失败并置errno
 */
int modbusc::mb_write_register(int addr, int value)
{
    return modbus_write_register(mb,addr,value);
}
/**
 * @brief 设置应答超时时间，超过会触发异常；
 * 在byte超时设置时为收到第一个字节的时间
 * 在byte超时没设置的情况下为完整接收到相应的总时间；
 * @param sec
 * @param usec 0 to 999999.
 * @return 操作影响返回值，若为-1则失败并置errno
 */
int modbusc::mb_responsetimeout(int sec, int usec)
{
    return modbus_set_response_timeout(mb,sec,usec);
}
/**
 * @brief 一条连续消息两个字节间的超时时间
 * @param sec
 * @param usec 0 to 999999
 * @return
 */
int modbusc::mb_bytetimeout(int sec, int usec)
{
    return modbus_set_byte_timeout(mb,sec,usec);
}
/**
 * @brief 关闭连接
 */
void modbusc::mb_close()
{
     reutn modbus_close(mb);
}
/**
 * @brief modbusc::mb_flush
 * @return 操作影响返回值，若为-1则失败并置errno
 */
int modbusc::mb_flush()
{
    return modbus_flush(mb);
}
/**
 * @brief 释放MODBUS对象资源
 */
void modbusc::mb_freemb()
{
    modbus_free(mb);
}


