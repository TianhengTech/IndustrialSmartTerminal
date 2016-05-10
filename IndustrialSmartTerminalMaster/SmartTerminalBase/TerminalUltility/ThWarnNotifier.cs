using System;
using SmartTerminalBase.DataBase;

namespace SmartTerminalBase.TerminalUltility
{
    class ThWarnNotifier
    {
        private int interval,threshold;
        public ThWarnNotifier(int inTerval,int threShold)
        {
            interval = inTerval;
            threshold = threShold;
        }
        /// <summary>
        /// Return the number of warning message within certain time.
        /// </summary>
        /// <param name="interval">The time interval in minutes</param>
        /// <returns></returns>
        private int  CheckNumberOfInfo(int inTerval)
        {
            var i = ThSqliteManager.GetSingle("select count(*) from warninfo where storetime<datetime('now','localtime') and storetime>datetime('now','localtime','-"+inTerval+" minute');");
            return (int)i;
        }

        public void StartNotifier()
        {
            int num=CheckNumberOfInfo(interval);
            if (num > threshold)
            {
                //Send to database
            }
            else
            {
                return;
            }
        }
    }
}
