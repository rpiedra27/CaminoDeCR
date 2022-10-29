﻿using ElCaminoDeCostaRica.Models;
using System;
using System.Web.Mvc;

namespace ElCaminoDeCostaRica.Controllers
{
    public class StageController : Controller
    {
        Database database = new Database();
        public ActionResult addStage()
        {
            database.openConnection();
            ViewBag.Data = database.routeList();
            database.closeConnection();
            database.openConnection();
            ViewBag.sections = database.sectionList();
            database.closeConnection();
            return View();
        }

        [HttpPost]
        public ActionResult addStage(Stage stage)
        {
            database.openConnection();
            ViewBag.Data = database.routeList();
            ViewBag.sections = database.sectionList();
            database.closeConnection();
            ViewBag.Success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    database.openConnection();
                    ViewBag.newID = database.addStage(stage);
                    database.closeConnection();
                    if (ViewBag.newID != 0)
                    {
                        ViewBag.Message = "La etapa fue creada con éxito.";
                        ViewBag.route = stage.idRoute;
                        ModelState.Clear();
                        return View("addStagePath");
                    }
                }
                return View();
            }
            catch
            {
                ViewBag.Message = "Algo salio mal y no fue posible crear la etapa.";
                return View();
            }

        }

        public ActionResult stageList()
        {
            database.openConnection();
            ViewBag.stages = database.stageList();
            database.closeConnection();
            return View();
        }

        [HttpGet]
        public ActionResult stageDelete(int identificador)
        {
            ViewBag.ExitoAlBorrar = false;
            database.openConnection();
            database.stageDelete(identificador);
            ViewBag.stages = database.stageList();
            database.closeConnection();
            return View("stageList");
        }

        [HttpGet]
        public ActionResult stageEdit(int? identificador)
        {
            database.openConnection();
            ViewBag.Data = database.routeList();
            database.closeConnection();
            ActionResult vista;
            try
            {
                database.openConnection();
                Stage stageItem = database.stageList().Find(smodel => smodel.id == identificador);
                database.closeConnection();
                if (stageItem == null)
                {
                    vista = RedirectToAction("stageList");
                }
                else
                {
                    vista = View(stageItem);
                }
            }
            catch
            {
                vista = RedirectToAction("stageList");
            }
            return vista;
        }

        [HttpPost]
        public ActionResult stageEdit(Stage stage)
        {
            try
            {
                database.openConnection();
                database.stageEdit(stage);
                ViewBag.stages = database.stageList();
                database.closeConnection();
                return RedirectToAction("stageList");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult addStagePath()
        {
            return View();
        }

        /* public ActionResult Inscription(int? identificador)
         {
             bool addInscription = true;
             Inscription newInscription = new Inscription { idUser = 12345, idStage = Convert.ToInt32(identificador), idDates = 1 };
             database.openConnection();
             ViewBag.inscriptions = database.inscriptionList();
             database.closeConnection();
             foreach (var inscription in ViewBag.inscriptions)
             {
                 if (inscription.idUser == newInscription.idUser && inscription.idStage== newInscription.idStage) 
                 {
                     ViewBag.Message = "Algo salio mal y no fue posible inscribirse.";

                     addInscription = false;
                 }
             }
             if (addInscription)
             {
                 database.openConnection();
                 ViewBag.Data = database.inscription(newInscription);
                 database.closeConnection();
                 ViewBag.Message = "Se incribio con exito en la etapa" + identificador;
             }   
             database.openConnection();
             ViewBag.stages = database.stageList();
             database.closeConnection();
             return View("stageList");
         }*/
    }
}
