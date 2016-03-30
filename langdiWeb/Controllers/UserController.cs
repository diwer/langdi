using LangdiDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace langdiWeb.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            var currentuser = this.GetCurrentUser();
            if (currentuser != null && currentuser.Role == Role.Manager)
            {
                var list = this.UserRepository.FindAll().Where(u => u.State == State.Nomal).ToList();
                return View(list);
            }
            else
            {
                throw new HttpException(404, "");
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            var currentuser = this.GetCurrentUser();
            if (currentuser != null && currentuser.Role == Role.Manager)
            {
                return View();
            }
            else
            {
                throw new HttpException(404, "");
            }
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User model)
        {
            try
            {
                var currentuser = this.GetCurrentUser();
                if (currentuser != null && currentuser.Role == Role.Manager)
                {
                    var user = new User()
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        Role = model.Role
                    };
                    this.UserRepository.Add(user);
                    this.UserRepository.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new HttpException(404, "");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            var currentuser = this.GetCurrentUser();
            if ((currentuser != null && currentuser.Role == Role.Manager) || (currentuser != null && currentuser.Id == id))
            {
                var user = this.UserRepository.Load(id);

                return View(user);
            }
            else
            {
                throw new HttpException(404, "");
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(User model)
        {
            try
            {
                var currentuser = this.GetCurrentUser();
                if ((currentuser != null && currentuser.Role == Role.Manager) || (currentuser != null && currentuser.Id == model.Id))
                {
                    var user = this.UserRepository.Load(model.Id);
                    user.Password = StringHelper.Md5(model.Password);
                    user.Role = model.Role;
                    this.UserRepository.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    throw new HttpException(404, "");
                }
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            var currentuser = this.GetCurrentUser();
            if (currentuser != null && currentuser.Role == Role.Manager)
            {
                var user = this.UserRepository.Load(id);
                user.State = State.Delete;
                this.UserRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                throw new HttpException(404, "");
            }
        }


    }
}
