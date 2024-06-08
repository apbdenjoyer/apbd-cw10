using apbd_cw10.Data;
using apbd_cw10.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw10.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly ApplicationContext _context;

    public PrescriptionsController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription(AddPrescription request)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(
            p => p.IdPatient == request.PatientDto.IdPatient);

        if (patient == null)
        {
            patient = new Patient()
            {
                FirstName = request.PatientDto.FirstName,
                LastName = request.PatientDto.LastName,
                Birthday = request.PatientDto.Birthday,
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        foreach (var medicament in request.MedicamentDtos)
        {
            if (!await _context.Medicaments.AnyAsync(m => m.IdMedicament == medicament.IdMedicament))
            {
                return NotFound($"Medicament with Id {medicament.IdMedicament} not found.");
            }
        }

        if (request.MedicamentDtos.Count > 10)
        {
            return BadRequest($"Too many medicaments: {request.MedicamentDtos.Count}, Allowed maximum: 10");
        }

        if (request.PrescriptionDto.DueDate < request.PrescriptionDto.Date)
        {
            return BadRequest($"Time travel detected - DueDate happening before Date: {request.PrescriptionDto.DueDate} < {request.PrescriptionDto.Date}");
        }

        var prescription = new Prescription()
        {
            Date = request.PrescriptionDto.Date,
            DueDate = request.PrescriptionDto.DueDate,
            IdPatient = request.PatientDto.IdPatient,
        };
        

        _context.Add(prescription);
        await _context.SaveChangesAsync();
        return Created();
    }
}

public class AddPrescription
{
    public PatientDto PatientDto { get; set; }
    public List<MedicamentDto> MedicamentDtos { get; set; }
    public PrescriptionDto PrescriptionDto { get; set; }
}