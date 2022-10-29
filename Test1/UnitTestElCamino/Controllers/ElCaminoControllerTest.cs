using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElCaminoDeCostaRica.Controllers;
using System.Web.Mvc;
using ElCaminoDeCostaRica.Models;
namespace UnitTestElCamino.Controllers
{
    [TestClass]
    public class ElCaminoControllerTest
    {
        [TestMethod]
        public void stageListViewResultNotNull()
        {
            //Arrange
            StageController stage = new StageController();
            //Act
            ActionResult view= stage.stageList();
            //Assert
            Assert.IsNotNull(view);

        }

        [TestMethod]
        public void showServicesSucces()
        {
            int id = 1;
            //Arrange
            RouteController route = new RouteController();

            //Act
            ActionResult view = route.showServices(id);
            //Assert
            Assert.IsNotNull(view);

        }

        [TestMethod]
        public void feedbackCommentsConfirmSuccess()
        {
            //Arrange
            int id = 19;
            FeedbackController feedback = new FeedbackController();
            //Act
            ViewResult vista = feedback.feedbackEdit(id) as ViewResult;
            Feedback feedbackItem = vista.Model as Feedback;
            //Assert
            Assert.IsNotNull(feedbackItem);
            Assert.AreEqual("good", feedbackItem.comments);           
        }


    }
}
