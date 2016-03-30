using LangdiDomain;
using langdiWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace langdiWeb.Controllers
{
    public class SignController : Controller
    {
        private IRepository<User, int> UserRepository = new Repository<User, int>();
        // GET: Sign
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if(this.User.Identity.IsAuthenticated)
                    return Redirect("/product/");
                var user = UserRepository.Find(Specification<User>.Eval(u => u.UserName == model.UserName)).SingleOrDefault();
                if (user != null && user.Password == StringHelper.Md5(model.PassWord))
                {
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                    return Redirect("/product/");
                }
                else
                {
                    //var tuser = new User()
                    //{
                    //    UserName = "admins",
                    //    Password = StringHelper.Md5("admin"),
                    //    Role = Role.Manager,
                    //    State = State.Nomal
                    //};
                    //this.UserRepository.Add(tuser);
                    //this.UserRepository.SaveChanges();
                    ViewBag.ErrorMsg = "用户名或密码错误";
                    return View();
                }
            }
            ViewBag.Error = ModelState.Values;
            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/sign/login");
        }
    }
}