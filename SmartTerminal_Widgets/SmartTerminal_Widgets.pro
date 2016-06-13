#-------------------------------------------------
#
# Project created by QtCreator 2016-06-12T09:31:13
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = SmartTerminal_Widgets
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    snap7.cpp \
    mainthreads.cpp

HEADERS  += mainwindow.h \
    snap7.h \
    mainthreads.h

FORMS    += mainwindow.ui

QT +=sql
LIBS += -lsnap7
