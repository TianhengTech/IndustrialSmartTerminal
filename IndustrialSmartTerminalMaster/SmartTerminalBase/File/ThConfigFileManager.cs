using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SmartTerminalBase.FileEditor
{
    internal class ThConfigFileManager : ConfigFileBase
    {
        /// <summary>
        ///     以字典形式返回键值对
        /// </summary>
        /// <param name="allKeyValue"></param>
        /// <returns></returns>
        public Dictionary<string, string> ConvertToDictionary(string[] allKeyValue)
        {
            var results = new Dictionary<string, string>();
            foreach (var Key_Value in allKeyValue)
            {
                var keyValue = Key_Value.Split('=');
                var key = keyValue[0];
                var value = keyValue[1];
                results[key] = value;
            }
            return results;
        }

        #region DLL库

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault,
            [In, Out] char[] lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString,
            string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)] //可以没有此行  
        private static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

        [DllImport("kernel32")] //返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize,
            string lpFileName);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, byte[] retVal,
            int size, string filePath);

        #endregion

        #region 读

        public static Dictionary<string, int> addr = new Dictionary<string, int>();
        public static Dictionary<string, string> warnmsg = new Dictionary<string, string>();

        public string ReadIniData(string section, string Key, string iniFilePath)
        {
            var NoText = "";
            if (File.Exists(iniFilePath))
            {
                var temp = new StringBuilder();
                GetPrivateProfileString(section, Key, NoText, temp, 255, iniFilePath);
                return temp.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     获取INI文件中指定节点(Section)中的所有条目(key=value形式)
        /// </summary>
        /// <param name="iniFile">Ini文件</param>
        /// <param name="section">节点名称</param>
        /// <returns>指定节点中的所有项目,没有内容返回string[0]</returns>
        public override string[] IniGetAllItems(string iniFile, string section)
        {
            //返回值形式为 key=value,例如 Color=Red  
            uint MAX_BUFFER = 32767; //默认为32767  

            var items = new string[0]; //返回值  

            //分配内存  
            var pReturnedString = Marshal.AllocCoTaskMem((int) MAX_BUFFER*sizeof(char));

            var bytesReturned = GetPrivateProfileSection(section, pReturnedString, MAX_BUFFER, iniFile);

            if (!(bytesReturned == MAX_BUFFER - 2) || (bytesReturned == 0))
            {
                var returnedString = Marshal.PtrToStringAuto(pReturnedString, (int) bytesReturned);
                items = returnedString.Split(new[] {'\0'}, StringSplitOptions.RemoveEmptyEntries);
            }

            Marshal.FreeCoTaskMem(pReturnedString); //释放内存  

            return items;
        }

        /// <summary>
        ///     获取INI文件中指定节点(Section)中的所有条目的Key列表
        /// </summary>
        /// <param name="iniFile">Ini文件</param>
        /// <param name="section">节点名称</param>
        /// <returns>如果没有内容,反回string[0]</returns>
        public override string[] IniGetAllItemKeys(string iniFile, string section)
        {
            var value = new string[0];
            const int SIZE = 1024*10;

            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            var chars = new char[SIZE];
            var bytesReturned = GetPrivateProfileString(section, null, null, chars, SIZE, iniFile);

            if (bytesReturned != 0)
            {
                value = new string(chars).Split(new[] {'\0'}, StringSplitOptions.RemoveEmptyEntries);
            }
            chars = null;

            return value;
        }

        /// <summary>
        ///     读取INI文件中指定INI文件中的所有节点名称(Section)
        /// </summary>
        /// <param name="iniFile">Ini文件</param>
        /// <returns>所有节点,没有内容返回string[0]</returns>
        public override string[] IniGetAllSectionNames(string iniFile)
        {
            uint MAX_BUFFER = 32767; //默认为32767  

            var sections = new string[0]; //返回值  

            //申请内存  
            var pReturnedString = Marshal.AllocCoTaskMem((int) MAX_BUFFER*sizeof(char));
            var bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, iniFile);
            if (bytesReturned != 0)
            {
                //读取指定内存的内容  
                var local = Marshal.PtrToStringAuto(pReturnedString, (int) bytesReturned);

                //每个节点之间用\0分隔,末尾有一个\0  
                sections = local.Split(new[] {'\0'}, StringSplitOptions.RemoveEmptyEntries);
            }

            //释放内存  
            Marshal.FreeCoTaskMem(pReturnedString);

            return sections;
        }

        public void ReadAddrIniFile(string path)
        {
            try
            {
                var sections = IniGetAllSectionNames(path);
                foreach (var section in sections)
                {
                    var allKeys = IniGetAllItems(path, section);
                    foreach (var Key_Value in allKeys)
                    {
                        var _Key_Value = Key_Value.Split('=');
                        var key = _Key_Value[0];
                        var value = _Key_Value[1];
                        addr[key] = Convert.ToInt32(value);
                    }
                }
            }
            catch
            {
                Trace.Write("配置文件读取失败!");
            }
        }

        public void ReadInfoIniFile(string path)
        {
            try
            {
                var sections = IniGetAllSectionNames(path);
                foreach (var section in sections)
                {
                    var allKeys = IniGetAllItems(path, section);
                    foreach (var Key_Value in allKeys)
                    {
                        var keyValue = Key_Value.Split('=');
                        var key = keyValue[0];
                        var value = keyValue[1];
                        warnmsg[key] = value;
                    }
                }
            }
            catch
            {
                Trace.Write("配置文件读取失败!");
            }
        }

        #endregion

        #region 写

        /// <summary>
        ///     在INI文件中，将指定的键值对写到指定的节点，如果已经存在则替换
        /// </summary>
        /// <param name="iniFile">INI文件</param>
        /// <param name="section">节点，如果不存在此节点，则创建此节点</param>
        /// <param name="items">键值对，多个用\0分隔,形如key1=value1\0key2=value2</param>
        /// <returns></returns>
        public override bool IniWriteItems(string iniFile, string section, string items)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(items))
            {
                throw new ArgumentException("必须指定键值对", "items");
            }

            return WritePrivateProfileSection(section, items, iniFile);
        }

        /// <summary>
        ///     在INI文件中，指定节点写入指定的键及值。如果已经存在，则替换。如果没有则创建。
        /// </summary>
        /// <param name="iniFile">INI文件</param>
        /// <param name="section">节点</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>操作是否成功</returns>
        public override bool IniWriteValue(string iniFile, string section, string key, string value)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称", "key");
            }

            if (value == null)
            {
                throw new ArgumentException("值不能为null", "value");
            }

            return WritePrivateProfileString(section, key, value, iniFile);
        }

        /// <summary>
        ///     在INI文件中，删除指定节点中的指定的键。
        /// </summary>
        /// <param name="iniFile">INI文件</param>
        /// <param name="section">节点</param>
        /// <param name="key">键</param>
        /// <returns>操作是否成功</returns>
        public override bool IniDeleteKey(string iniFile, string section, string key)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称", "key");
            }

            return WritePrivateProfileString(section, key, null, iniFile);
        }

        /// <summary>
        ///     在INI文件中，删除指定的节点。
        /// </summary>
        /// <param name="iniFile">INI文件</param>
        /// <param name="section">节点</param>
        /// <returns>操作是否成功</returns>
        public override bool IniDeleteSection(string iniFile, string section)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            return WritePrivateProfileString(section, null, null, iniFile);
        }

        /// <summary>
        ///     在INI文件中，删除指定节点中的所有内容。
        /// </summary>
        /// <param name="iniFile">INI文件</param>
        /// <param name="section">节点</param>
        /// <returns>操作是否成功</returns>
        public bool IniEmptySection(string iniFile, string section)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            return WritePrivateProfileSection(section, string.Empty, iniFile);
        }

        #endregion
    }
}