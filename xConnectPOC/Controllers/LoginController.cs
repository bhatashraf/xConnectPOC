using Sitecore;
using Sitecore.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using xConnectPOC.Models;

namespace xConnectPOC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(UserLoginModel loginModel)
        {
            string redirectUrl = Context.Site.StartPath;

            if (!ModelState.IsValid)
                return View(loginModel);

            string email = loginModel.Email;
            string password = loginModel.Password;

            var domain = Context.Domain;
            if (domain == null)
                return View(loginModel);

            string accountName = domain.GetFullName(email);
            var result = AuthenticationManager.Login(accountName, password);

            if (!result)
                return View(loginModel);

            var user = AuthenticationManager.GetActiveUser();

          //  Context.Item = Context.Database.GetItem(Context.Site.StartPath);

            return Redirect(redirectUrl);
        }
    }
}