using NHibernate;
using FluentNHibernate;
namespace SmartTerminalBase.DataBase
{
    /// <summary>
    /// Nhibernate辅助类
    /// </summary>
    public class FluentNHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private static ISession _session;
        private static object _objLock = new object();

        private FluentNHibernateHelper()
        {
        }

        /// <summary>
        /// 创建ISessionFactory
        /// </summary>
        /// <returns></returns>
        public static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
            {
                lock (_objLock)
                {
                    if (_sessionFactory == null)
                    {
                        //配置ISessionFactory
                        _sessionFactory = FluentNHibernate.Cfg.Fluently.Configure()
                            //数据库配置
                            .Database(
                                //方言
                                FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard
                                    //连接字符串
                                    .ConnectionString(
                                        c => c.Server("192.168.1.25")
                                            .Password("tianheng123")
                                            .Username("root")
                                            .Database("Shop")
                                    )
                            //是否显示sql
                            //.ShowSql()
                            )
                            //映射程序集
                            .Mappings(m => m.FluentMappings
                                .AddFromAssembly(System.Reflection.Assembly.Load("SmartTerminalBase"))
                                .ExportTo("c:\\"))
                            .BuildSessionFactory();
                    }
                }
            }
            return _sessionFactory;
        }

        /// <summary>
        /// 重置Session
        /// </summary>
        /// <returns></returns>
        public static ISession ResetSession()
        {
            if (_session.IsOpen)
                _session.Close();
            _session = _sessionFactory.OpenSession();
            return _session;
        }

        /// <summary>
        /// 打开ISession
        /// </summary>
        /// <returns></returns>
        public static ISession GetSession()
        {
            GetSessionFactory();
            if (_session == null)
            {
                lock (_objLock)
                {
                    if (_session == null)
                    {
                        _session = _sessionFactory.OpenSession();
                    }
                }
            }
            return _session;
        }
    }
}