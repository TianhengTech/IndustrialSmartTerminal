using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.FileEditor
{
    abstract class TextManagerBase
    {
        public TextManagerBase()
        {

        }
        FileStream file;

        public virtual object Open(string filepath)
        {
                file = new FileStream(filepath, FileMode.Open);
                return file;
        }
        public virtual bool Write(string fileName, string content)
        {
            return true;
        }
        public abstract string Read(string fileName);
        public virtual  void Close(string filepath)
        {
            if(file!=null)
            file.Close();
            return;
        }
        public virtual  object IsOpen(string filepath)
        {
            return true;
        }
        public virtual bool CreateDir(string Dirpath)
        {
            return true;
        }
        public virtual bool CreateFile(string filepath)
        {
            return true;
        }
        public virtual void DeleteDir(System.IO.DirectoryInfo dir)
        {
            return;
        }
        public virtual bool DeleteFile(string fileName)
        {
            return true;
        }
        public virtual bool Exists(string filepath)
        {
            return true;
        }
        public virtual bool CopyDir(string fromDir, string toDir)
        {
            return true;
        }




    }
}
