using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.FileEditor
{
    abstract class ConfigFileBase
    {

        public virtual bool INIWriteItems(string iniFile, string section, string items)
        {
            return true;
        }
        public abstract string[] INIGetAllSectionNames(string iniFile);
        public abstract string[] INIGetAllItems(string iniFile, string section);
        public abstract string[] INIGetAllItemKeys(string iniFile, string section);
        public virtual bool INIWriteValue(string iniFile, string section, string key, string value)
        {
            return true;
        }
        public virtual bool INIDeleteKey(string iniFile, string section, string key)
        {
            return true;
        }
        public virtual bool INIDeleteSection(string iniFile, string section)
        {
            return true;
        }

    }
}
