using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.FileEditor
{
    class FileManagerBase
    {
        public FileManagerBase()
        {

        }
        protected object Open(string filepath)
        {
            return true;
        }
        protected object Write(string filepath)
        {
            return true;
        }
        protected object Read(string filepath)
        {
            return true;
        }
        protected object Close(string filepath)
        {
            return true;
        }
        protected object IsOpen(string filepath)
        {
            return true;
        }
        protected object MakeDir(string Dirpath)
        {
            return true;
        }
        protected object MakeFile(string filepath)
        {
            return true;
        }
        protected object Exists(string filepath)
        {
            return true;
        }




    }
}
