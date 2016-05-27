__author__ = 'UserX'
def BufferToInt(buffer):
        result=0;
        buffer.reverse()
        for i in xrange(0,len(buffer)):
            result=result+(buffer[i]<<8*i)
        return result