using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.FileEditor
{
    class Tianheng_TextManagerBase
    {
        public Tianheng_TextManagerBase()
        {

        }

        protected virtual  object Open(string filepath)
        {
            return true;
        }
        protected virtual  object Write(string filepath)
        {
            return true;
        }
        protected virtual  object ReadIniAllKeys(string filepath)
        {
            return true;
        }
        protected virtual  object Close(string filepath)
        {
            return true;
        }
        protected virtual  object IsOpen(string filepath)
        {
            return true;
        }
        protected virtual  object MakeDir(string Dirpath)
        {
            return true;
        }
        protected virtual  object MakeFile(string filepath)
        {
            return true;
        }
        public virtual bool DeleteDir(string filepath)
        {
            return true;
        }
        public virtual bool DeleteFile(string filepath)
        {
            return true;
        }
        protected virtual  object Exists(string filepath)
        {
            return true;
        }




    }
}
