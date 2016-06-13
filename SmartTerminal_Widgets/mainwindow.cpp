#include "mainwindow.h"
#include "ui_mainwindow.h"




MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    this->init();

}
void MainWindow::init()
{
    ui->statusBar->addWidget(ui->label);
    readthread=new ReadThread();
    processthread=new ProcessThread();
    datacenterthread = new DatacenterThread();
    connect(processthread,SIGNAL(signal_showvalue(QString)),this,SLOT(read_received(QString)));
    readthread->start();
    processthread->start();
    datacenterthread->start();


}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_pushButton_clicked()
{
    ui->label->setText("accept..");
}

void MainWindow::read_received(QString s)
{
    ui->textEdit->setText(s);
}
