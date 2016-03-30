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
    public class ContacterController : BaseController
    {
        // GET: Contacter
        public ActionResult Index(PagingModel pager)
        {
            var pageargs = new PageArgs(pager);
            var list = ContacterReponsitory.FindAll().Skip(pageargs.From - 1).Take(pageargs.To - pageargs.From).ToList();
            var viewModel = new ContacterViewModel()
            {
                ContactsList = new PagedList<Contacter>(list, pageargs.Index, pageargs.Count)
            };
            return View(viewModel);
        }

        public ActionResult Detail(int id)
        {
            var Contacter = this.ContacterReponsitory.Load(id);
            return View(Contacter);
        }

        public ActionResult Delete(int id)
        {
            var Contacter = this.ContacterReponsitory.Load(id);
            this.ContacterReponsitory.Remove(Contacter);
            this.ContacterReponsitory.SaveChanges();
            return Redirect("/Contacter/Index");
        }
    }
}