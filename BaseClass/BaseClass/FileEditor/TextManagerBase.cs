using System.IO;

namespace BaseClass.FileEditor
{
    internal abstract class TextManagerBase
    {
        private FileStream _file;

        public virtual object Open(string filepath)
        {
            _file = new FileStream(filepath, FileMode.Open);
            return _file;
        }

        public virtual bool Write(string fileName, string content)
        {
            return true;
        }

        public abstract string Read(string fileName);

        public virtual void Close(string filepath)
        {
            if (_file != null)
                _file.Close();
        }

        public virtual object IsOpen(string filepath)
        {
            return true;
        }

        public virtual bool CreateDir(string dirpath)
        {
            return true;
        }

        public virtual bool CreateFile(string filepath)
        {
            return true;
        }

        public virtual void DeleteDir(DirectoryInfo dir)
        {
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