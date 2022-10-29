using ElCaminoDeCostaRica.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ElCaminoDeCostaRica.Controllers
{
    public class RouteController : Controller
    {
        Database database = new Database();
        public ActionResult addRoute()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addRoute(Route route)
        {
            ViewBag.Success = false;
            try
            {
                if (!ModelState.IsValid)
                {
                    database.openConnection();
                    ViewBag.Success = database.addRoute(route);
                    database.closeConnection();
                    if (ViewBag.Success)
                    {
                        ViewBag.Message = "La ruta " + route.id + " fue creada con exito.";
                        ModelState.Clear();
                    }
                }
                ViewBag.Message = "La ruta " + route.id + " fue creada con exito.";
                return View();
            }
            catch
            {
                ViewBag.Message = "Algo salio mal y no fue posible crear la ruta.";
                return View();
            }

        }

        public ActionResult routeList()
        {
            database.openConnection();
            ViewBag.routes = database.routeList();
            database.closeConnection();
            return View();
        }

        [HttpGet]
        public ActionResult routeDelete(int identificador)
        {
            ViewBag.ExitoAlBorrar = false;
            database.openConnection();
            database.routeDelete(identificador);
            ViewBag.routes = database.routeList();
            database.closeConnection();
            return View("routeList");
        }

        [HttpGet]
        public ActionResult routeEdit(int? identificador)
        {

            ActionResult vista;
            try
            {
                database.openConnection();
                Route routeItem = database.routeList().Find(smodel => smodel.id == identificador);
                database.closeConnection();
                if (routeItem == null)
                {
                    vista = RedirectToAction("routeList");
                }
                else
                {
                    vista = View(routeItem);
                }
            }
            catch
            {
                vista = RedirectToAction("routeList");
            }
            return vista;
        }

        [HttpPost]
        public ActionResult routeEdit(Route route)
        {
            try
            {
                database.openConnection();
                database.routeEdit(route);
                ViewBag.routes = database.routeList();
                database.closeConnection();
                return RedirectToAction("routeList");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult addSection()
        {
            database.openConnection();
            ViewBag.Routes = database.routeList();
            database.closeConnection();
            return View();
        }

        [HttpPost]
        public ActionResult addSection(Section section)
        {
            database.openConnection();
            ViewBag.Routes = database.routeList();
            database.closeConnection();
            ViewBag.Success = false;
            try
            {
                if (!ModelState.IsValid)
                {
                    database.openConnection();
                    ViewBag.Success = database.addSection(section);
                    database.closeConnection();
                    if (ViewBag.Success)
                    {
                        ViewBag.Message = "La seccion " + section.name + " fue creada con exito.";
                        ModelState.Clear();
                    }
                }
                ViewBag.Message = "La seccion " + section.name + " fue creada con exito.";
                database.openConnection();
                ViewBag.sections = database.sectionList();
                database.closeConnection();
                return View("sectionList");
            }
            catch
            {
                ViewBag.Message = "Algo salio mal y no fue posible crear la seccion.";
                return View();
            }

        }

        public ActionResult sectionList()
        {
            database.openConnection();
            ViewBag.sections = database.sectionList();
            database.closeConnection();
            return View();
        }

        [HttpGet]
        public ActionResult sectionDelete(int identificador)
        {
            ViewBag.ExitoAlBorrar = false;
            database.openConnection();
            database.deleteSection(identificador);
            ViewBag.sections = database.sectionList();
            database.closeConnection();
            database.openConnection();
            ViewBag.Routes = database.routeList();
            database.closeConnection();
            return View("sectionList");
        }

        [HttpGet]
        public ActionResult sectionEdit(int? identificador)
        {

            ActionResult vista;
            try
            {
                database.openConnection();
                Section sectionItem = database.sectionList().Find(smodel => smodel.id == identificador);
                database.closeConnection();
                database.openConnection();
                ViewBag.Routes = database.routeList();
                database.closeConnection();
                if (sectionItem == null)
                {
                    vista = RedirectToAction("sectionList");
                }
                else
                {
                    vista = View(sectionItem);
                }
            }
            catch
            {
                vista = RedirectToAction("sectionList");
            }
            return vista;
        }

        [HttpPost]
        public ActionResult sectionEdit(Section section)
        {
            try
            {
                database.openConnection();
                ViewBag.Routes = database.routeList();
                database.closeConnection();
                database.openConnection();
                database.sectionEdit(section);
                ViewBag.sections = database.sectionList();
                database.closeConnection();
                return RedirectToAction("sectionList");
            }
            catch
            {
                return View();
            }
        }

        // GET: /Route/ 
        public ActionResult Index()
        {
            database.openConnection();
            ViewBag.routeInfo = database.routeList();
            database.closeConnection();
            database.openConnection();
            ViewBag.pathInfo = database.getPathInfo();
            database.closeConnection();
            database.openConnection();
            ViewBag.placeInfo = database.getPlaceInfo();
            database.closeConnection();
            database.openConnection();
            ViewBag.stageInfo = database.stageList();
            database.closeConnection();
            database.openConnection();
            ViewBag.siteInfo = database.siteList();
            database.closeConnection();
            ViewBag.cookie = HttpContext.Request.Cookies["userLoginInfo"];
            database.openConnection();
            ViewBag.dates = database.getDatesList();
            database.closeConnection();
            database.openConnection();
            ViewBag.inscriptions = database.getInscriptions();
            database.closeConnection();
            return View();
        }
        [HttpGet]
        public ActionResult Inscription(int? identificador, int? dates)
        {
            database.openConnection();
            ViewBag.routeInfo = database.routeList();
            database.closeConnection();
            database.openConnection();
            ViewBag.pathInfo = database.getPathInfo();
            database.closeConnection();
            database.openConnection();
            ViewBag.placeInfo = database.getPlaceInfo();
            database.closeConnection();
            database.openConnection();
            ViewBag.stageInfo = database.stageList();
            database.closeConnection();
            database.openConnection();
            ViewBag.dates = database.getDatesList();
            database.closeConnection();
            bool addInscription = true;
            ViewBag.Success = false;
            if (dates == null || dates == -1)
            {
                ViewBag.Message = "No hay fechas disponible para la etapa seleccionada.";
            }
            else
            {
                Inscription newInscription = new Inscription { idUser = Convert.ToInt32(TempData.Peek("userId")), idStage = Convert.ToInt32(identificador), idDates = Convert.ToInt32(dates) };
                database.openConnection();
                ViewBag.inscriptions = database.inscriptionList();
                database.closeConnection();
                foreach (var inscription in ViewBag.inscriptions)
                {
                    if (inscription.idUser == newInscription.idUser && inscription.idStage == newInscription.idStage && inscription.idDates == newInscription.idDates)
                    {
                        ViewBag.Message = "Ya se encuentra inscrito para esta fecha.";
                        database.openConnection();
                        ViewBag.dates = database.getDatesList();
                        database.closeConnection();
                        database.openConnection();
                        ViewBag.inscriptions = database.getInscriptions();
                        database.closeConnection();

                        addInscription = false;
                    }
                }
                if (addInscription)
                {
                    database.openConnection();
                    ViewBag.Data = database.inscription(newInscription);
                    database.closeConnection();
                    ViewBag.Message = "Se inscribio correctamente.";
                    ViewBag.Success = true;
                    Mail mail = new Mail();
                    CodeGenerator codeGenerator = new CodeGenerator();
                    string code = codeGenerator.generateStageCode(6);
                    mail.sendInscriptionCode(Convert.ToString(TempData.Peek("userEmail")), code,Convert.ToString(newInscription.idStage));
                }
                database.openConnection();
                ViewBag.stages = database.stageList();
                database.closeConnection();
            }

            return View("Index");
        }

        [HttpGet]
        public ActionResult showServices(int? identificador)
        {
            if(Convert.ToInt32(identificador) != 0)
            {
                ViewBag.id = Convert.ToInt32(identificador);
                TempData["stageId"] = Convert.ToInt32(identificador);
                database.openConnection();
                ViewBag.services = database.stageServices(Convert.ToInt32(identificador));
                database.closeConnection();
                database.openConnection();
                ViewBag.categories = database.categoryList();
                database.closeConnection();
                database.openConnection();
                ViewBag.suppliers = database.supplierList();
                database.closeConnection();
                database.openConnection();
                ViewBag.ratings = database.getRatings();
                database.closeConnection();

                return View();
            }
            else
            {
                ViewBag.Message1 = "Debe seleccionar una etapa";
                ViewBag.Success = false;
                database.openConnection();
                ViewBag.routeInfo = database.routeList();
                database.closeConnection();
                database.openConnection();
                ViewBag.pathInfo = database.getPathInfo();
                database.closeConnection();
                database.openConnection();
                ViewBag.placeInfo = database.getPlaceInfo();
                database.closeConnection();
                database.openConnection();
                ViewBag.stageInfo = database.stageList();
                database.closeConnection();
                database.openConnection();
                ViewBag.dates = database.getDatesList();
                database.closeConnection();
                database.openConnection();
                List<StageDates> dates2 = (List<StageDates>)database.getDatesList();
                database.closeConnection();
                database.openConnection();
                ViewBag.inscriptions = database.getInscriptions();
                database.closeConnection();
                return View("Index");
            }
         
        }

        [HttpGet]
        public ActionResult doSurvey(int? identificador)
        {
            TempData["serviceId"] = Convert.ToInt32(identificador);
            return View();
        }

        [HttpPost]
        public ActionResult doSurvey(int? identificador, string userCode, string submitButton)
        {
            identificador = Convert.ToInt32(TempData.Peek("serviceId"));
            if (string.Equals(submitButton, "Confirmar"))
            {
                var today = DateTime.Today;
                database.openConnection();
                string code = database.getCode(today.ToString(), Convert.ToInt32(TempData.Peek("stageId")));
                database.closeConnection();
                if (String.Equals(code, userCode))
                {

                    database.openConnection();
                    int category = database.getServiceCategory(Convert.ToInt32(identificador));
                    database.closeConnection();
                    database.openConnection();
                    int survey = database.getIdSurvey(Convert.ToInt32(identificador), category);
                    database.closeConnection();
                    TempData["serviceId"] = Convert.ToInt32(identificador);
                    TempData["categoryId"] = category;
                    TempData["surveyId"] = survey;
                    database.openConnection();
                    ViewBag.questions = database.questionList();
                    database.closeConnection();
                    database.openConnection();
                    ViewBag.surveys = database.surveyList().Find(smodel => smodel.id == survey);
                    database.closeConnection();
                    return RedirectToAction("Questions", new { identificador = survey });
                }
                else
                {
                    ViewBag.Success = false;
                    ViewBag.Message = "Código incorrecto!";
                    return View();
                }
            }
            else
            {
                ViewBag.Success = false;
                database.openConnection();
                ViewBag.routeInfo = database.routeList();
                database.closeConnection();
                database.openConnection();
                ViewBag.pathInfo = database.getPathInfo();
                database.closeConnection();
                database.openConnection();
                ViewBag.placeInfo = database.getPlaceInfo();
                database.closeConnection();
                database.openConnection();
                ViewBag.stageInfo = database.stageList();
                database.closeConnection();
                database.openConnection();
                ViewBag.dates = database.getDatesList();
                database.closeConnection();
                database.openConnection();
                ViewBag.inscriptions = database.getInscriptions();
                database.closeConnection();
                return RedirectToAction("showServices", new { identificador = TempData.Peek("stageId") });
            }
            
        }

        [HttpGet]
        public ActionResult Questions(int? identificador)
        {
            try
            {
                database.openConnection();
                ViewBag.surveys = database.surveyList().Find(smodel => smodel.id == identificador);
                database.closeConnection();
                database.openConnection();
                Survey surveyItem = database.surveyList().Find(smodel => smodel.id == identificador);
                database.closeConnection();
                database.openConnection();
                ViewBag.questions = database.questionList();
                database.closeConnection();
                if (surveyItem == null)
                {
                    return RedirectToAction("showServices", new { identificador = TempData.Peek("stageId") });
                }
                else
                {
                    return View(surveyItem.id);
                }

            }
            catch
            {
                return RedirectToAction("showServices", new { identificador = TempData.Peek("stageId") });
            }
        }

        [HttpPost]
        public ActionResult Questions(FormCollection form)
        {
            database.openConnection();
            ViewBag.questions = database.questionList();
            database.closeConnection();
            database.openConnection();
            ViewBag.cat = database.categoryList();
            database.closeConnection();
            database.openConnection();
            ViewBag.ser = database.serviceList();
            database.closeConnection();
            try
            {
                int count = 0;
                foreach (var question in ViewBag.questions)
                {
                    if (question.idSurvey == Convert.ToInt32(TempData.Peek("surveyId")))
                    {
                        
                        Feedback feedback = new Feedback { idQuestion = question.id, idSurvey = Convert.ToInt32(TempData.Peek("surveyId")), 
                            idService = Convert.ToInt32(TempData.Peek("serviceId")), rating = Convert.ToInt32(form["Calificacion" + count]), 
                            comments = Convert.ToString(form["Comentarios"]), day = DateTime.Today };
                        database.openConnection();
                        database.addFeedback(feedback);
                        database.closeConnection();
                        count = +1;
                    }

                }
            }
            catch
            {
                database.openConnection();
                ViewBag.services = database.stageServices(Convert.ToInt32(TempData.Peek("serviceId")));
                database.closeConnection();
                database.openConnection();
                ViewBag.categories = database.categoryList();
                database.closeConnection();
                database.openConnection();
                ViewBag.suppliers = database.supplierList();
                database.closeConnection();
                return RedirectToAction("showServices", new { identificador = TempData.Peek("stageId") });
            }
            database.openConnection();
            ViewBag.services = database.stageServices(Convert.ToInt32(TempData.Peek("serviceId")));
            database.closeConnection();
            database.openConnection();
            ViewBag.categories = database.categoryList();
            database.closeConnection();
            database.openConnection();
            ViewBag.suppliers = database.supplierList();
            database.closeConnection();
            database.openConnection();
            ViewBag.surveys = database.surveyList();
            database.closeConnection();
            TempData["success"] = true;
            TempData["message"] = "Encuesta completada correctamente!";
            return RedirectToAction("showServices", new { identificador = TempData.Peek("stageId")});
        }

        [HttpPost]
        public String shareSite(string data)
        {
            String result = "";
            string[] info = data.Split('|');

            Match match = Regex.Match(info[0], "^(.+)@(.+)$");
            if (match.Success)
            {
                Mail mail = new Mail();
                mail.shareSite(info[0], info[1]);
                result = "Sitio compartido correctamente";
            }
            else
            {
                result = "Formato inválido";
            }

            return result;
        }

        [HttpGet]
        public ActionResult showFeedbacks(int? identificador)
        {
            database.openConnection();
            ViewBag.feedbacks = database.feedbackList();
            database.closeConnection();
            database.openConnection();
            ViewBag.services = database.serviceList();
            database.closeConnection();
            database.openConnection();
            ViewBag.suppliers = database.supplierList();
            database.closeConnection();
            List<Feedback> feedbacks = new List<Feedback>();
            foreach(var feedback in ViewBag.feedbacks)
            {
                if(feedback.idService == Convert.ToInt32(identificador))
                {
                    feedbacks.Add(feedback);
                }
            }
            ViewBag.feedbackService = feedbacks;
            return View();
        }
    }
}