﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ElCaminoDeCostaRica.Models;

namespace ElCaminoDeCostaRica.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            ViewBag.LoginSuccess = false;
            Database database = new Database();
            database.openConnection();
            if (!database.checkEmail(user.email))
            {                
                if (database.checkPassword(user.email, user.password))
                {
                    ViewBag.LoginSuccess = true;
                    TempData["userId"] = database.getUserId(user.email);
                    TempData["userEmail"] = user.email;
                    ModelState.Clear();

                    int userType = database.getUserType(user.email);
                    switch(userType)
                    {
                        case 0:
                            TempData["Menu"] = "user";
                            break;

                        case 1:
                            TempData["Menu"] = "admin";
                            break;
                    }
                    //Save cookie with user login information
                    HttpCookie userLoginInfo = new HttpCookie("userLoginInfo");
                    userLoginInfo["username"] = user.email;
                    userLoginInfo["password"] = user.password;
                    userLoginInfo["id"] = database.getUserID(user.email).ToString();
                    userLoginInfo.Expires.Add(new TimeSpan(0, 10, 0));
                    Response.Cookies.Add(userLoginInfo);

                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    ViewBag.Message = "Credenciales incorrectas!";
                }
            }
            else
            {
                ViewBag.Message = "El correo electrónico no se encuentra registrado!";
            }
            database.closeConnection();
            ModelState.Clear();
            return View();
        }

        public ActionResult Close()
        {
            HttpContext.Response.Cookies.Remove("UserInfo");
            return View("Login");
        }

        public ActionResult UserDashBoard()
        {
            if (Session["id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public string sendPassword(string email)
        {
            string result = "";

            Database database = new Database();
            database.openConnection();
            if (!database.checkEmail(email))
            {
                result = database.getPassword(email);
                database.closeConnection();
                Mail mail = new Mail();
                mail.sendPassword(email, result);
                result = "Correo enviado!";
            }
            else
            {
                database.closeConnection();
                Match match = Regex.Match(email, "^(.+)@(.+)$");
                if (!match.Success)
                {
                    result = "Formato de correo invalido";
                }
                else
                {
                    result = "Correo no registrado!";
                }
            }
            return result;
        }
    }
}