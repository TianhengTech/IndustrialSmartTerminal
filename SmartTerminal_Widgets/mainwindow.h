#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "mainthreads.h"

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    void init();

private slots:
    void on_pushButton_clicked();
    void read_received(QString);

private:
    Ui::MainWindow *ui;
    ProcessThread *processthread;
    ReadThread *readthread;
    DatacenterThread *datacenterthread;


};


#endif // MAINWINDOW_H
