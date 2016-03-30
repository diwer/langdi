using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LangdiDomain;
using langdiWeb.Models;
using Webdiyer.WebControls.Mvc;

namespace langdiWeb.Controllers
{
    public class ProductController : BaseController
    {

        // GET: Product
        public ActionResult Index(PagingModel pager)
        {
            var pageargs = new PageArgs(pager);
            var list = NewsRepository.FindAll().Skip(pageargs.From - 1).Take(pageargs.To - pageargs.From).ToList();
            var viewModel = new ProductViewModel()
            {
                NewsList = new PagedList<News>(list, pageargs.Index, pageargs.Count)
            };
            return View(viewModel);
        }


        // GET: Product/Create
        public ActionResult Create()
        {
            var catetgories = this.CategoryRepository.FindAll().Where(c => c.State == State.Nomal).ToList();
            var selectItemList = new List<SelectListItem>();
            foreach (var item in catetgories)
            {
                selectItemList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            ViewBag.SelectList = selectItemList;
            return View();
        }

        [ValidateInput(false)]
        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(News model, int imgId, int categoryId)
        {
            try
            {
                using (var db = new LangdiDb.LangdiDbContext())
                {
                    var img = db.Set<Image>().Find(new object[] { imgId });
                    var category = db.Set<Category>().Find(new object[] { categoryId });
                    var news = new News()
                    {
                        Title = model.Title,
                        State = State.Nomal,
                        Category = category,
                        Image = img,
                        Content = model.Content,
                        CreateTime = DateTime.Now,
                        Description = model.Description,
                        ViewCount = 0,
                        LoveCount = 0
                    };
                    db.Set<News>().Add(news);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception excp)
            {
                var selectItemList = new List<SelectListItem>();
                var catetgories = this.CategoryRepository.FindAll().Where(c => c.State == State.Nomal).ToList();
                foreach (var item in catetgories)
                {
                    var selectitem = new SelectListItem()
                    {

                        Text = item.Name,
                        Value = item.Id.ToString()
                    };
                    selectItemList.Add(selectitem);
                }
                ViewBag.SelectList = selectItemList;
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var news = this.NewsRepository.Load(id);
            var catetgories = this.CategoryRepository.FindAll().Where(c => c.State == State.Nomal).ToList();
            var selectItemList = new List<SelectListItem>();
            foreach (var item in catetgories)
            {
                var selectitem = new SelectListItem()
                {

                    Text = item.Name,
                    Value = item.Id.ToString()
                };
                selectItemList.Add(selectitem);
            }
            ViewBag.SelectList = selectItemList;
            return View(news);
        }

        // POST: Product/Edit/5
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(News model)
        {
            try
            {
                var news = this.NewsRepository.Load(model.Id);
                news.EditTime = DateTime.Now;
                news.Description = model.Description;
                news.Content = model.Content;
                news.CategoryId = model.CategoryId;
                var imgid = int.Parse(this.Request.Form["imgId"]);
                var img = this.ImgageRepository.Load(imgid);
                news.Image = img;
                news.Title = model.Title;
                NewsRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                var catetgories = this.CategoryRepository.FindAll().Where(c => c.State == State.Nomal).ToList();
                var selectItemList = new List<SelectListItem>();
                foreach (var item in catetgories)
                {
                    var selectitem = new SelectListItem()
                    {

                        Text = item.Name,
                        Value = item.Id.ToString()
                    };
                    selectItemList.Add(selectitem);
                }
                ViewBag.SelectList = selectItemList;
                var news = this.NewsRepository.Load(model.Id);
                return View(news);
            }
        }


        public ActionResult Delete(int id)
        {
            var news = this.NewsRepository.Load(id);
            if (news != null)
            {
                this.NewsRepository.Remove(news);
                this.NewsRepository.SaveChanges();
            }
            return Redirect("/product/Index");
        }

    }
}
