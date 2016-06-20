__author__ = 'UserX'
import logging,os,sys
from logging.handlers import RotatingFileHandler,TimedRotatingFileHandler

def script_path():
  path = os.path.realpath(sys.argv[0])
  if os.path.isfile(path):
      path = os.path.dirname(path)
  return os.path.abspath(path)

def getlogger():
    LOGGING_MSG_FORMAT  = '[%(asctime)s] [%(levelname)s] [%(module)s] [%(funcName)s] [%(lineno)d] %(message)s'
    logging.basicConfig(level=logging.DEBUG,  format='%(asctime)s %(filename)s[line:%(lineno)d] %(levelname)s %(message)s',
                datefmt='%a, %d %b %Y %H:%M:%S',
                filename='system.log',
                filemode='w')
    log=logging.getLogger()
    log_path = os.path.join(script_path(),'logs')
    if not os.path.exists(log_path):
      os.makedirs(log_path)
    log_file = os.path.join(log_path,'xxx.log')
    logger = logging.handlers.TimedRotatingFileHandler(log_file,'midnight',1)
    logger.formatter=LOGGING_MSG_FORMAT
    log.addHandler(logger)
    return log