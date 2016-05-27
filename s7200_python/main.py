#coding:utf-8
import time
from Communication.thSnap7 import *
from Utility.bufferConverter import BufferToInt
from snap7.client import *
from Utility.terminalcommon  import *
from Database.dbConnect import dbConnect
from Database.alchemyMapping import *
from Utility.thLogging import *


plc = snap7.client.Client()
log=getlogger()
def readcycle(q):
    try:
        plc.set_connection_params('192.168.1.51',0x1000,0x1001)
        plc.connect200()
    except Exception,ex:
        pass

    while(True):
        result=plc.db_read(1,4000,8)
        print(type(result))
        q.put(result)
        time.sleep(1)

def dataprocess(proq,saveq):
    while(True):
        result=proq.get()
        show=[]
        for i in xrange(0,len(result)):
           show.append(str(BufferToInt(result[i:i+1])))
        print show
        saveq.put(show)
    return

def cloudstorage(cloudq):
    mysql=dbConnect("192.168.1.25","root","tianheng123","terminal_test")
    while True:
        result=cloudq.get()
        historydataMapping("historydata")
        historydb=historydata()
        warninfo=warninfo()
        mysql.add(historydb)
        mysql.add(warninfo)
        mysql.commit()
    return

def localstorage(q):
    return

def main():

    read_process.daemon=True
    dataprocessing_process.daemon=True
    dataprocessing_process.start()
    read_process.start()
    read_process.join()


if __name__=="__main__":
    main()