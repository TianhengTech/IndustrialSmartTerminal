using FluentNHibernate;
using FluentNHibernate.Automapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace SmartTerminalBase.DataBase
{
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
                 .Database(FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard
                             //连接字符串
                 .ConnectionString(
                      c => c.Server(Properties.Settings.Default.database_ip)
                     .Password(Properties.Settings.Default.password)
                     .Username(Properties.Settings.Default.id)
                     .Database(Properties.Settings.Default.database_name)                                        
                                    )                             //是否显示sql
                     //.ShowSql()
                     )
                             //映射程序集

                      .Mappings(
                          m =>
                          {
                             // m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.Load("SmartTerminalBase")).ExportTo(".\\Mappings");
                              m.HbmMappings.AddFromAssembly(System.Reflection.Assembly.Load("SmartTerminalBase"));
                          }                       
                        )                         
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
