using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExecViewCSVtoJS.Helper;


namespace ExecViewCSVtoJS.Controllers
{
    public class CSVtojsonController : Controller
    {
        // GET: CSVtojson
        public ActionResult StartMyApp()
        {
            return View(new ExecViewCSVtoJS.Models.FilePathParam());
        }
        [HttpPost]
        public ActionResult StartMyApp(ExecViewCSVtoJS.Models.FilePathParam model)
        {
            CreateJson cj = new CreateJson();

            if (!ModelState.IsValid)
            {
                return View();
            }

            cj.Executecsv(model.uploadinput, model.uploadoutput);
            return RedirectToAction("JsonFileSuccess", "CSVtojson");
        }

        public ActionResult JsonFileSuccess()
        {
            return View();
        }
    }
}
