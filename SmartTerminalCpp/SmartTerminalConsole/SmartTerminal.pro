QT += core
QT -= gui

CONFIG += c++11

TARGET = SmartTerminal
CONFIG += console
CONFIG -= app_bundle
QT += axcontainer
QT += serialport
TEMPLATE = app

SOURCES += main.cpp \
    Communication/snap7.cpp \
    Logger/logger.cpp \
    Config/inimanager.cpp \
    Ultility/common.cpp \
    Ultility/bufferconverter.cpp \
    File/excelexport.cpp \
    Communication/libmodbus/modbus-data.c \
    Communication/libmodbus/modbus-rtu.c \
    Communication/libmodbus/modbus-tcp.c \
    Communication/libmodbus/modbus.c \
    Communication/modbusc.cpp \
    File/fileeditor.cpp \
    Communication/serialcom.cpp \
    Ultility/terminalqueues.cpp

HEADERS += \
    main.h \
    Communication/snap7.h \
    Ultility/common.h \
    Logger/logger.h \
    Config/inimanager.h \
    Ultility/bufferconverter.h \
    Communication/snap7client.h \
    File/excelexport.h \
    Communication/libmodbus/config.h \
    Communication/libmodbus/modbus-private.h \
    Communication/libmodbus/modbus-rtu-private.h \
    Communication/libmodbus/modbus-rtu.h \
    Communication/libmodbus/modbus-tcp-private.h \
    Communication/libmodbus/modbus-tcp.h \
    Communication/libmodbus/modbus-version.h \
    Communication/libmodbus/modbus.h \
    Communication/modbusc.h \
    File/fileeditor.h \
    Communication/serialcom.h
LIBS += -LD:\CppProject\QTterminal\SmartTerminal -lsnap7
LIBS += -lws2_32
QT +=sql

DISTFILES += \
    README.MD
