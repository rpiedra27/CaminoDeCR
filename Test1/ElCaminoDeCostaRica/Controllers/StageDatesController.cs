using System.Web.Mvc;
using ElCaminoDeCostaRica.Models;
using System;

namespace ElCaminoDeCostaRIca.Controllers
{
    public class StageDatesController : Controller
    {
        Database database = new Database();
        public ActionResult addStageDate()
        {
            database.openConnection();
            ViewBag.Stages = database.stageList();
            database.closeConnection();
            return View();
        }

        [HttpPost]
        public ActionResult addStageDate(StageDates dates)
        {
            database.openConnection();
            ViewBag.Stages = database.stageList();
            database.closeConnection();
            ViewBag.Success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    CodeGenerator generator= new CodeGenerator();
                    string code = generator.generateStageCode(6);
                    dates.code = code;
                    database.openConnection();
                    ViewBag.Success = database.addStageDate(dates);
                    database.closeConnection();
                    if (ViewBag.Success)
                    {
                        ViewBag.Message = "La fecha fue creada con exito.";
                        ModelState.Clear();
                    }

                }
                return View();
            }
            catch
            {
                ViewBag.Message = "Algo salio mal y no fue posible crear la fecha.";
                return View();
            }

        }

        public ActionResult stageDateList()
        {
            database.openConnection();
            ViewBag.dates = database.stageDatesList();
            database.closeConnection();
            database.openConnection();
            ViewBag.stage= database.stageList();
            database.closeConnection();
            return View();
        }

        [HttpGet]
        public ActionResult stageDatesEdit(int? identificador)
        {
            database.openConnection();
            ViewBag.Stages = database.stageList();
            database.closeConnection();
            ActionResult vista;
            try
            {
                database.openConnection();
                StageDates dateItem = database.stageDatesList().Find(smodel => smodel.id == identificador);
                database.closeConnection();
                if (dateItem == null)
                {
                    vista = RedirectToAction("stageDateList");
                }
                else
                {
                    vista = View(dateItem);
                }
            }
            catch
            {
                vista = RedirectToAction("stageDateList");
            }
            return vista;
        }

        [HttpPost]
        public ActionResult stageDatesEdit(StageDates dates)
        {
            try
            {
                database.openConnection();
                ViewBag.Stages = database.stageList();
                database.closeConnection();
                database.openConnection();
                database.stageDatesEdit(dates);
                ViewBag.dates = database.stageDatesList();
                database.closeConnection();
                return RedirectToAction("stageDateList");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult stageDatesDelete(int identificador)
        {
            ViewBag.ExitoAlBorrar = false;
            database.openConnection();
            database.stageDatesDelete(identificador);
            database.closeConnection();
            database.openConnection();
            ViewBag.stage= database.stageList();
            database.closeConnection();
            database.openConnection();
            ViewBag.dates = database.stageDatesList();
            database.closeConnection();
            return View("stageDateList");
        }
    }
}