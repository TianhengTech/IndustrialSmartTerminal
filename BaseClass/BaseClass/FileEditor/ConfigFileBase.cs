namespace BaseClass.FileEditor
{
    internal abstract class ConfigFileBase
    {
        public virtual bool IniWriteItems(string iniFile, string section, string items)
        {
            return true;
        }

        public abstract string[] IniGetAllSectionNames(string iniFile);
        public abstract string[] IniGetAllItems(string iniFile, string section);
        public abstract string[] IniGetAllItemKeys(string iniFile, string section);

        public virtual bool IniWriteValue(string iniFile, string section, string key, string value)
        {
            return true;
        }

        public virtual bool IniDeleteKey(string iniFile, string section, string key)
        {
            return true;
        }

        public virtual bool IniDeleteSection(string iniFile, string section)
        {
            return true;
        }
    }
}