using LangdiDomain;
using langdiWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace langdiWeb.Controllers
{
    public class PortalController : BaseController
    {
        // GET: Portal
        public ActionResult ProductList(int? cateId, PagingModel pager)
        {
            if (!cateId.HasValue || cateId < 0)
                cateId = 0;
            var pageargs = new PageArgs(pager);
            List<News> list = null;
            if (cateId != 0)
            {
                list = NewsRepository.FindAll().Where(n => n.CategoryId == cateId).ToList();
            }
            else
            {
                list = NewsRepository.FindAll().ToList();
            }

            var viewModel = new ProductViewModel()
            {
                cateId = cateId.Value,
                CateList = CategoryRepository.FindAll().Where(c => c.State == State.Nomal).ToList(),
                NewsList = new PagedList<News>(list, pageargs.Index, pageargs.Count)

            };
            return View(viewModel);
        }

        public ActionResult Detail(int id)
        {
            var news = this.NewsRepository.Load(id);
            if (news != null)
            {
                return View(news);
            }
            throw new HttpException(404, "");
        }
        public PartialViewResult Recommond(int categoryId)
        {
            var list = this.NewsRepository.Find(Specification<News>.Eval(n => n.CategoryId == categoryId)).Take(3).ToList();
            return PartialView(list);
        }

        public ActionResult Video(PagingModel pager)
        {
            pager.PageSize = 1;
            var pageargs = new PageArgs(pager);

            var list = VideoRepository.FindAll();
            return View(new PagedList<Video>(list, pageargs.Index, pageargs.Count));
        }


        public JsonResult AddContact(Contacter tact)
        {
            try { 
            this.ContacterReponsitory.Add(tact);
            this.ContacterReponsitory.SaveChanges();
            return Json("success");
            }catch(Exception ex)
            {
#if DEBUG
                return Json(ex.Message);
#else
                return Json("网络错误,请稍后再试");
#endif

            }
        }

    }
}