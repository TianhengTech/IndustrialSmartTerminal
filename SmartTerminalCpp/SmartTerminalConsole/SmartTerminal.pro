include(D:/Qt/QxOrm/QxOrm.pri)

###############################

# QXORM Library Configuration #

###############################

isEmpty(QXORM_INCLUDE_PATH) { QXORM_INCLUDE_PATH = $$quote(D:/Qt/QxOrm/include) }

isEmpty(QXORM_LIB_PATH) { QXORM_LIB_PATH = $$quote(D:/Qt/QxOrm/lib) }


QT += core
QT -= gui

CONFIG += c++11

TARGET = SmartTerminal
CONFIG += console
CONFIG -= app_bundle
QT += axcontainer
QT += serialport


TEMPLATE = app


INCLUDEPATH += $${QXORM_INCLUDE_PATH}

PRECOMPILED_HEADER = ./Database/QxORM/precompiled.h

LIBS += -L$${QXORM_LIB_PATH}


CONFIG(debug, debug|release) {

LIBS += -lQxOrmd

} else {

LIBS += -lQxOrm

} # CONFIG(debug, debug|release)

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
    Ultility/terminalqueues.cpp \
    Database/QxORM/historydata.cpp

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
    Communication/serialcom.h \
    Database/QxORM/precompiled.h \
    Database/QxORM/export.h \
    Database/QxORM/historydata.h
LIBS += -LD:\CppProject\QTterminal\SmartTerminal -lsnap7
LIBS += -lws2_32
QT +=sql

DISTFILES += \
    README.MD \
    Config/address.ini \
    Config/database.ini

RESOURCES +=
