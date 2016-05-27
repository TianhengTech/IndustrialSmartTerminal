__author__ = 'UserX'
from multiprocessing import Queue,Process
from main import  readcycle,dataprocess,cloudstorage,localstorage
import snap7
dataprocessqueue=Queue()
datacenterqueue=Queue()
localdataqueue=Queue()

read_process=Process(target=readcycle,args=(dataprocessqueue,))
dataprocessing_process=Process(target=dataprocess,args=(dataprocessqueue,datacenterqueue,))
cloudstorage_process=Process(target=cloudstorage,args=(datacenterqueue,))
localstorag_process=Process(target=localstorage,args=(localdataqueue,))
