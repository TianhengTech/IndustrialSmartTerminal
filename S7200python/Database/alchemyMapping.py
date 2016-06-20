from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy import Column, String, create_engine,Integer,DATETIME,Float,MetaData,Table
from sqlalchemy.orm import *
from alchemyGroup import historydata
Base = declarative_base()
def historydataMapping(table_name):
    metadata = MetaData()
    table_object = Table(table_name, metadata,
        idlab_temperature = Column(Integer, primary_key = True, nullable = False,autoincrement=True),
        TEMP = Column(String(45), nullable = False),
        TIME = Column(DATETIME, nullable = False )
    )
    mapper(historydata, table_object)
    return

class warninfo(Base):
    __tablename__="warninfo"
    idwarninfo=Column(Integer,primary_key=True,autoincrement=True)
    warnmessage=Column(String(45))
    time=Column(DATETIME)

    def __init__(self,warnmessage,time):
        self.warnmessage=warnmessage
        self.time=time





