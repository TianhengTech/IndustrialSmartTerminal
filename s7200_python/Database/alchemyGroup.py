__author__ = 'UserX'
from sqlalchemy import Column, String, create_engine,Integer,DATETIME,Float,Table,MetaData
from sqlalchemy.orm import mapper,clear_mappers
class historydata(object):
    def __init__(self,idlab_temperature,TEMP,TIME):
        self.TEMP=TEMP;
        self.TIME=TIME;
        self.idlab_temperature=idlab_temperature;




