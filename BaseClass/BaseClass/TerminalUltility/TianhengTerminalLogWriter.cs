using System;
using log4net;

namespace BaseClass.TerminalUltility
{
    internal class TerminalLogWriter
    {
        /// <summary>
        ///  输出日志到Log4Net
        /// </summary>
        /// <param name="t">Class type</param>
        /// <param name="msg">log Message</param>
        public static void WriteErroLog(Type t, string msg)
        {
            var log = LogManager.GetLogger(t);
            log.Error(msg);
        }

        public static void WriteWarnLog(Type t, string msg)
        {
            var log = LogManager.GetLogger(t);
            log.Warn(msg);
        }

        public static void WriteInfoLog(Type t, string msg)
        {
            var log = LogManager.GetLogger(t);
            log.Info(msg);
        }
    }
}