using LangdiDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace langdiWeb.Models
{
    public class ProductViewModel
    {
        public int cateId;

        public List<Category> CateList { get; set; }
        public PagedList<News> NewsList { get; set; }
    }
    public class ContacterViewModel
    {
        public PagedList<Contacter> ContactsList { get; set; }
    }
    public class CateListViewModel
    {
        public List<Category> CateList { get; set; }
        public int CateId { get; set; }
    }
}