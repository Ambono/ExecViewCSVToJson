using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExecViewCSVtoJS;
using ExecViewCSVtoJS.Controllers;
using ExecViewCSVtoJS.Models;

namespace ExecViewCSVtoJS.Tests.Controllers
{
    [TestClass]
    public class CSVtoJsonControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var ctrl = new CSVtojsonController();
            var model = new FilePathParam();
            model.uploadoutput = "@C:/Users/myfile.txt";
            model.uploadinput = "@C:/file.csv";
            var expected = "StartMyApp";

            // Act  
            var actionResult = ctrl.StartMyApp(model) as RedirectToRouteResult;
            System.Diagnostics.Debugger.Launch();
            // Assert
            Assert.AreEqual("CSVtojson", actionResult.RouteValues["controller"]);
            Assert.AreEqual(expected, actionResult.RouteValues["action"]);
        }

        [TestMethod]
        public void TestMethod2()
        {
            // Arrange
            var ctrl = new CSVtojsonController();
            var model = new FilePathParam();
            model.uploadoutput = "@C:/Users/myfile.js";
            model.uploadinput = "@C:/file.csv";
            var expected = "JsonFileSuccess";

            // Act   
            var actionResult = ctrl.StartMyApp(model) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("CSVtojson", actionResult.RouteValues["controller"]);
            Assert.AreEqual(expected, actionResult.RouteValues["action"]);
        }

    }
}
