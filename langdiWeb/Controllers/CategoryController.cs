using LangdiDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace langdiWeb.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index()
        {
            var list = this.CategoryRepository.FindAll().Where(c=>c.State== State.Nomal).OrderBy(c => c.Order).ToList();
            return View(list);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category model)
        {
            try
            {
                var category = new Category()
                {
                    Name = model.Name,
                    Order = model.Order,
                    State = State.Nomal
                };
                this.CategoryRepository.Add(category);
                this.CategoryRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            var category = this.CategoryRepository.Load(id);
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category model)
        {
            try
            {
                var category = this.CategoryRepository.Load(model.Id);
                category.Name = model.Name;
                category.Order = model.Order;
                this.CategoryRepository.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            var category = this.CategoryRepository.Load(id);
            category.State = State.Delete;
            this.CategoryRepository.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
