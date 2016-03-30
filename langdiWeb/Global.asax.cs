using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace langdiWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

#warning 此处很关键
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            try
            {
                string auth_param_name = "AUTHID";
                string auth_cookie_name = FormsAuthentication.FormsCookieName;

                if (HttpContext.Current.Request.Form[auth_param_name] != null)
                {
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.Form[auth_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[auth_param_name] != null)
                {
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.QueryString[auth_param_name]);
                }
                if (Context.Request.FilePath == "/") Context.RewritePath("index1.html");
            }
            catch
            { }
        }

        private void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (null == cookie)
            {
                cookie = new HttpCookie(cookie_name);
                cookie.Value = cookie_value;
                HttpContext.Current.Request.Cookies.Set(cookie);
            }
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                return;

            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                if (ticket != null)
                {
                    HttpContext.Current.User = new LangdiUserPrincipal(ticket, ticket.UserData.Split(new char[] { ',' }));
                }
            }
            catch
            {
            }
        }
    }
    public class LangdiUserPrincipal : IPrincipal
    {
        private System.Security.Principal.IIdentity identity = null;
        public System.Security.Principal.IIdentity Identity
        {
            get { return identity; }
        }

        private string[] roles { get; set; }

        public LangdiUserPrincipal(FormsAuthenticationTicket ticket, string[] roles)
        {
            identity = new FormsIdentity(ticket);
            this.roles = roles;
        }

        public bool IsInRole(string role)
        {
            foreach (string r in this.roles)
            {
                if (r == role)
                    return true;
            }

            return false;
        }
    }
}
