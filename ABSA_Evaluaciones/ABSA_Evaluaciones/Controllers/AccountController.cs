using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ABSA_Evaluaciones.Models;
using ABSA_Evaluaciones;

namespace ABSA_Evaluaciones.Controllers
{
    public class AccountController : Controller
    {
        
        private ABSA_EvaluacionEntities databaseManager = new ABSA_EvaluacionEntities();

        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                if (this.Request.IsAuthenticated)
                {
                    return this.RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return this.View();
        }


        //POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(sp_logeo_Result model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var loginInfo = this.databaseManager.sp_logeo(model.chrUsrEval, model.chrPasEval).ToList();

                    if (loginInfo != null && loginInfo.Count() > 0)
                    {
                        var loginDetails = loginInfo.First();
                        //Log In
                        this.SignInUser(loginDetails.chrNomEval, false);

                        return this.RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("ErrorIngreso", "Usuario o contraseña incorrecta.");
                    }

                }
                else
                {
                    ModelState.AddModelError("ErrorIngreso", "Usuario o contraseña incorrecta.");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return this.View(model);

        }

        private void SignInUser(string usuario, bool isPersistent)
        {
            var claims = new List<Claim>();
            try
            {
                claims.Add(new Claim(ClaimTypes.Name, usuario));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {
            try
            {
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                authenticationManager.SignOut();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return this.RedirectToAction("Login", "Account");
        }
    }
}