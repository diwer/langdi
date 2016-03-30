using LangdiDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace langdiWeb.Controllers
{
    public class VideoController : BaseController
    {
        // GET: Video
        public ActionResult Index()
        {
            var list = VideoRepository.FindAll().ToList();
            return View(list);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Video video)
        {
            var v = new Video();
            v.Title = video.Title;
            v.Code = video.Code;
            VideoRepository.Add(v);
            VideoRepository.SaveChanges();
            return Redirect("/Video/index");
        }

        public ActionResult Delete(int id)
        {
            var video = VideoRepository.Load(id);
            VideoRepository.Remove(video);
            VideoRepository.SaveChanges();
            return Redirect("/Video/index");
        }

    }
}