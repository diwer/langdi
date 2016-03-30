using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace langdiWeb
{
    public interface IStorageProvider
    {
        void UploadFile(string path, string dest);
        void UploadFile(byte[] buffer, string dest);
        void UploadString(string content, string dest);
        void UploadStream(Stream stream, string dest);
        string DownloadText(string path);
        byte[] Download(string path);
        Stream DownloadStream(string path);

        void Delete(string path);
        void Move(string from, string dest);
    }
    public class LocalStorageProvider : IStorageProvider
    {
        public LocalStorageProvider(string savepath)
        {
            this.SavePath = savepath;
        }
        private string savePath = "";
        public string SavePath
        {
            get { return savePath; }
            set
            {
                if (value.StartsWith("~"))
                {
                    savePath = HttpContext.Current.Server.MapPath(value);
                }
                else
                {
                    savePath = value;
                }

                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest">temps/日期/Guid.扩展名</param>
        /// <returns>完整的磁盘路径</returns>
        private string GetDestPath(string dest)
        {
            string path = Path.Combine(SavePath, dest);
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return path;
        }

        public void UploadFile(string path, string dest)
        {
            File.Copy(path, GetDestPath(dest));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="dest">temps/日期/Guid.扩展名</param>
        public void UploadFile(byte[] buffer, string dest)
        {
            File.WriteAllBytes(GetDestPath(dest), buffer);
        }

        public void UploadString(string content, string dest)
        {
            File.WriteAllText(GetDestPath(dest), content, Encoding.UTF8);
        }

        public void UploadStream(System.IO.Stream stream, string dest)
        {
            byte[] buffer = new byte[1024];

            using (FileStream fs = new FileStream(GetDestPath(dest), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                int readed = 0;
                while ((readed = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, readed);
                }
            }
        }

        public string DownloadText(string path)
        {
            return File.ReadAllText(Path.Combine(SavePath, path), Encoding.UTF8);
        }

        public byte[] Download(string path)
        {
            return File.ReadAllBytes(Path.Combine(SavePath, path));
        }

        public Stream DownloadStream(string path)
        {
            return new FileStream(Path.Combine(SavePath, path), FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public void Delete(string path)
        {
            File.Delete(Path.Combine(SavePath, path));
        }

        public void Move(string from, string dest)
        {
            from = GetDestPath(from);
            dest = GetDestPath(dest);

            File.Move(from, dest);
        }
    }
}