QT += core
QT -= gui

CONFIG += c++11

TARGET = SmartTerminal
CONFIG += console
CONFIG -= app_bundle

TEMPLATE = app

SOURCES += main.cpp \
    snap7.cpp \
    snap7client.cpp

HEADERS += \
    snap7.h \
    snap7client.h \
    main.h
LIBS += -LD:\CppProject\QTterminal\SmartTerminal -lsnap7
