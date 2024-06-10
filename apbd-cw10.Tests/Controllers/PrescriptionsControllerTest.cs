using System;
using System.Collections.Generic;
using System.Net;
using apbd_cw10.Controllers;
using apbd_cw10.Data;
using apbd_cw10.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using PrescriptionDto = apbd_cw10.Controllers.PrescriptionDto;
using MedicamentDto = apbd_cw10.Models.MedicamentDto;
using apbd_cw10.Models;

namespace apbd_cw10.Tests.Controllers;

[TestClass]
[TestSubject(typeof(PrescriptionsController))]
public class PrescriptionsControllerTest
{
    [Fact]
    public void AddPrescription_MedicamentDoesntExist_ReturnsNotFound()
    {
        /*Arrange*/
        var mockContext = new ApplicationContext(new DbContextOptions<ApplicationContext>());
        var mockController = new PrescriptionsController(mockContext);

        AddPrescription request = new AddPrescription()
        {
            Patient = new PatientDto()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Birthday = DateTime.Now
            },
            Medicaments = new List<MedicamentDto>()
            {
                new MedicamentDto()
                {
                    IdMedicament = -1,
                    Name = "TestMedicament",
                    Description = "TestDescription",
                    Type = "TestType"
                }
            },
            Prescription = new PrescriptionDto()
            {
                Date = DateTime.Now,
                DueDate = DateTime.Now
            }
        };
        var result = mockController.AddPrescription(request);
        Assert.Equals(result, HttpStatusCode.NotFound);
    }

    [Fact]
    public void AddPrescription_TooManyMedicaments_ReturnsBadRequest()
    {
        /*Arrange*/
        var mockContext = new ApplicationContext(new DbContextOptions<ApplicationContext>());
        var mockController = new PrescriptionsController(mockContext);

        AddPrescription request = new AddPrescription()
        {
            Patient = new PatientDto()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Birthday = DateTime.Now
            },
            Medicaments = new List<MedicamentDto>(),
            Prescription = new PrescriptionDto()
            {
                Date = DateTime.Now,
                DueDate = DateTime.Now
            }
        };

        for (var i = 0; i < 11; i++)
        {
            request.Medicaments.Add(new MedicamentDto()
            {
                IdMedicament = i,
                Name = "Name " + i,
                Description = "Description " + i,
                Type = "Type " + i
            });
        }

        var result = mockController.AddPrescription(request);
        Assert.Equals(result, HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public void AddPrescription_DueDateBeforeDate_ReturnsBadRequest()
    {
        /*Arrange*/
        var mockContext = new ApplicationContext(new DbContextOptions<ApplicationContext>());
        var mockController = new PrescriptionsController(mockContext);

        AddPrescription request = new AddPrescription()
        {
            Patient = new PatientDto()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Birthday = DateTime.Now
            },
            Medicaments = new List<MedicamentDto>()
            {
                new MedicamentDto()
                {
                    IdMedicament = 1,
                    Name = "TestMedicament",
                    Description = "TestDescription",
                    Type = "TestType"
                }
            },
            Prescription = new PrescriptionDto()
            {
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(-1)
            }
        };
        var result = mockController.AddPrescription(request);
        Assert.Equals(result, HttpStatusCode.BadRequest);
    }
}