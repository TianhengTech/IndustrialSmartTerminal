__author__ = 'UserX'
from sqlalchemy import create_engine
from datetime import datetime
from sqlalchemy.orm import session
from sqlalchemy.orm import sessionmaker

def dbConnect(url,id,password,databse):
     DB_CONNECT_STRING = 'mysql+mysqldb://'+id+':'+password+'@'+url+'/'+databse
     engine=create_engine(DB_CONNECT_STRING,encoding="utf8",echo=False)
     return session.Session(bind=engine)
