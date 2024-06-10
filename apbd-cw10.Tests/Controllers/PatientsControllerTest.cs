using System.Net;
using apbd_cw10.Controllers;
using apbd_cw10.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace apbd_cw10.Tests.Controllers;

[TestClass]
[TestSubject(typeof(PatientsController))]
public class PatientsControllerTest
{

   [Fact]
   public void GetAllPatientInfo_PatientIsNull_ReturnsNotFound()
   {
      /*Arrange*/
      var mockContext = new ApplicationContext(new DbContextOptions<ApplicationContext>());
      var mockController = new PatientsController(mockContext);

      var testPatientId = -1;
      var result = mockController.GetAllPatientInfo(testPatientId);
      Assert.Equals(result, HttpStatusCode.NotFound);
   }
   
   [Fact]
   public void GetAllPatientInfo_PatientExists_ReturnsOk()
   {
      /*Arrange*/
      var mockContext = new ApplicationContext(new DbContextOptions<ApplicationContext>());
      var mockController = new PatientsController(mockContext);

      var testPatientId = 1;
      var result = mockController.GetAllPatientInfo(testPatientId);
      Assert.Equals(result, HttpStatusCode.OK);
   }
}