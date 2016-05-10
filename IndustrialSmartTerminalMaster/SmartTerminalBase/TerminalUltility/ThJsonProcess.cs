using System;
using Newtonsoft;
using Newtonsoft.Json;

namespace SmartTerminalBase.TerminalUltility
{
    class ThJsonProcess:DataProcessBase
    {
        /// <summary>
        /// 将对象转为json类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override string DumpToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 将json转换为指定类型数据
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="jsonstr"></param>
        /// <returns></returns>
        public override t LoadFromJson<t>(string jsonstr)
        {
            return JsonConvert.DeserializeObject<t>(jsonstr);
        }
        /// <summary>
        /// Json转为.net对象
        /// </summary>
        /// <param name="jsonstr"></param>
        /// <returns></returns>
        public object LoadFromJson(string jsonstr)
        {
            return JsonConvert.DeserializeObject(jsonstr);
        }
        
    }
}
