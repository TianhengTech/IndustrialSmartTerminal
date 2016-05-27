from snap7 import client,snap7types
import snap7

class S7200Client():
    def __init__(self):
        self.__Client=client.Client()

    def get_client(self):
        return self.__Client

    def connect(self,address,localTsap,remoteTsap):
        self.__Client.set_connection_params(address,localTsap,remoteTsap)
        result=self.__Client.Connect()
        if(result!=None):
            print "Connection failed!"

    def get_connected(self):
        return self.__Client.get_connected()

    def disconnect(self):
        self.__Client.disconnect()

    def reconnect(self):
        self.__Client.Connect()

    def read_area(self, area, dbnumber, start, size):
        return self.__Client.read_area(area,dbnumber,start,size)

    def db_read(self, db_number, start, size):
        return self.__Client.db_read(db_number,start,size)

    def v_read(self,start,size):
        return self.__Client.db_read(1,start,size)

    def write_area(self, area, dbnumber, start, data):
        return self.__Client.write_area(area,dbnumber,start,data)