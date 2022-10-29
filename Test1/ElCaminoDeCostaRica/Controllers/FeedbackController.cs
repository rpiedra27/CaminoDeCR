using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ElCaminoDeCostaRica.Models;

namespace ElCaminoDeCostaRica.Controllers
{
    public class FeedbackController : Controller
    {
        Database database = new Database();
        public ActionResult addFeedback()
        {
            database.openConnection();
            ViewBag.Services = database.serviceList();
            database.closeConnection();
            database.openConnection();
            ViewBag.Surveys = database.surveyList();
            database.closeConnection();
            return View();
        }

        [HttpPost]
        public ActionResult addFeedback(Feedback feedback)
        {
            database.openConnection();
            ViewBag.Services = database.serviceList();
            database.closeConnection();
            database.openConnection();
            ViewBag.Surveys = database.surveyList();
            database.closeConnection();
            ViewBag.Success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    database.openConnection();
                    ViewBag.Success = database.addFeedback(feedback);
                    database.closeConnection();
                    if (ViewBag.Success)
                    {
                        ViewBag.Message = "El feedback fue creada con exito.";
                        ModelState.Clear();
                    }

                }

                return View();
            }
            catch
            {
                ViewBag.Message = "Algo salio mal y no fue posible crear el feedback.";
                return View();
            }

        }

        public ActionResult feedbackList()
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
            return View();
        }

        [HttpGet]
        public ActionResult feedbackDelete(int identificador)
        {
           
            ViewBag.ExitoAlBorrar = false;
            database.openConnection();
            database.feedbackDelete(identificador);
            database.closeConnection();
            return View("feedbackList");
        }

        [HttpGet]
        public ActionResult feedbackEdit(int? identificador)
        {
            database.openConnection();
            ViewBag.Services = database.serviceList();
            database.closeConnection();
            database.openConnection();
            ViewBag.Surveys = database.surveyList();
            database.closeConnection();
            ActionResult vista;
            try
            {
                database.openConnection();
                Feedback feedbackItem = database.feedbackList().Find(smodel => smodel.idSurvey == identificador);
                database.closeConnection();
                if (feedbackItem == null)
                {
                    vista = RedirectToAction("feedbackList");
                }
                else
                {
                    vista = View(feedbackItem);
                }
            }
            catch
            {
                vista = RedirectToAction("feedbackList");
            }
            return vista;
        }

        [HttpPost]
        public ActionResult feedbackEdit(Feedback feedback)
        {
            try
            {
                database.openConnection();
                database.feedbackEdit(feedback);
                database.closeConnection();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult sendFeedback(int survey, int serviceId, int questionId)
        {
            database.openConnection();
            Feedback feedback = database.feedbackList().Find(smodel => smodel.idSurvey == survey &&  smodel.idService == serviceId && 
            smodel.idQuestion == questionId);
            database.closeConnection();
            database.openConnection();
            string servicio = database.getServiceName(feedback.idService);
            database.closeConnection();
            database.openConnection();
            string question = database.getQuestion(feedback.idQuestion);
            database.closeConnection();
            database.openConnection();
            string email =database.getSupplierEmail(feedback.idService);
            database.closeConnection();
            string format = "Servicio: " + servicio + "<br />" + "Pregunta: " + question + "<br />" + "Rating: " + feedback.rating +
                "<br />" + "Comentarios: " + feedback.comments + ".";
            Mail mail = new Mail();
            mail.sendFeedback(email, format);
            ViewBag.Success = true;
            ViewBag.Message = "Feedback enviado correctamente!";


            database.openConnection();
            ViewBag.feedbacks = database.feedbackList();
            database.closeConnection();
            database.openConnection();
            ViewBag.services = database.serviceList();
            database.closeConnection();
            database.openConnection();
            ViewBag.suppliers = database.supplierList();
            database.closeConnection();


            return View("feedbackList");
        }
    }
}