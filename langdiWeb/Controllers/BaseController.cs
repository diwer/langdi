using LangdiDomain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using langdiWeb.Models;

namespace langdiWeb.Controllers
{
    public class BaseController : Controller
    {
        public LocalStorageProvider StorageProvider = new LocalStorageProvider("~/upload");
        public IRepository<News, int> NewsRepository = new Repository<News, int>();
        public IRepository<Contacter, int> ContacterReponsitory = new Repository<Contacter, int>();
        public IRepository<Category, int> CategoryRepository = new Repository<Category, int>();
        public IRepository<User, int> UserRepository = new Repository<User, int>();
        public IRepository<Image, int> ImgageRepository = new Repository<Image, int>();
        public IRepository<Video, int> VideoRepository = new Repository<Video, int>();
        // GET: Base
        public PartialViewResult LoginStatu()
        {
            var user = new User();
            if (string.IsNullOrEmpty(this.User.Identity.Name))
                user = null;
            else
                user = this.UserRepository.Load(int.Parse(this.User.Identity.Name));
            return PartialView("_LoginStatu", user);
        }
        public PartialViewResult LeftMenu()
        {
            return PartialView("_leftMenu");
        }
        public User GetCurrentUser()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.UserRepository.Load(int.Parse(this.User.Identity.Name));
            }
            else
            {
                return null;
            }
        }

        public JsonResult UploadFile(HttpPostedFileBase[] file)
        {
            JsonResult result = new JsonResult();

            if (file == null || file.Length == 0)
            {
                result.Data = new { success = false, msg = "请选择一个文件" };
                return result;
            }

            foreach (var f in file)
            {
                try
                {
                    string folder = "tempfiles";
                    if (".jpg.jpeg.gif.png".IndexOf(Path.GetExtension(f.FileName).ToLower()) != -1)
                    {
                        folder = "tempimgs";
                    }

                    byte[] buffer = new byte[f.ContentLength];
                    using (BinaryReader br = new BinaryReader(f.InputStream))
                    {
                        br.Read(buffer, 0, buffer.Length);
                    }

                    string filepath = string.Format("{0}/{1}/{2}{3}", folder, DateTime.UtcNow.ToString("yyyy-MM-dd"), Guid.NewGuid().ToString(), Path.GetExtension(f.FileName));
                    StorageProvider.UploadFile(buffer, filepath);
                    var filesize = ((buffer.Length / 1024) / (double)1024).ToString("F");
                    var img = new Image()
                    {
                        Alt = Path.GetExtension(f.FileName),
                        State = State.Nomal,
                        Src = filepath
                    };
                    this.ImgageRepository.Add(img);
                    this.ImgageRepository.SaveChanges();
                    result.Data = new { success = true, imgid = img.Id, filepath = img.Src };
                }
                catch (Exception e)
                {
                    //TODO 上传文件失败，记录日志

                    result.Data = new { success = false, msg = e.Message };
                }
            }
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }


        public PartialViewResult catelist(int id)
        {
            var list = CategoryRepository.FindAll().Where(c => c.State == State.Nomal).ToList();
            var obj = new CateListViewModel()
            {
                CateId = id,
                CateList = list
            };


            return PartialView(obj);
        }
    }

}