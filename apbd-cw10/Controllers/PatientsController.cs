using apbd_cw10.Data;
using apbd_cw10.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw10.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly ApplicationContext _context;

    public PatientsController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllPatientInfo(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Prescription_Medicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient == null)
        {
            return NotFound($"Patient with ID {id} does not exist.");
        }

        var patientWithHistory = new PatientWithHistoryDto
        {
            Patient = new PatientDto
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Birthday = patient.Birthday
            },
            Prescriptions = patient.Prescriptions.Select(prescription => new PrescriptionDto
            {
                IdPrescription = prescription.IdPrescription,
                Date = prescription.Date,
                DueDate = prescription.DueDate,
                Doctor = new DoctorDto
                {
                    IdDoctor = prescription.Doctor.IdDoctor,
                    FirstName = prescription.Doctor.FirstName
                },
                Medicaments = prescription.Prescription_Medicaments.Select(pm => new MedicamentDto
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose,
                    Description = pm.Medicament.Description
                }).ToList()
            }).ToList()
        };

        return Ok(patientWithHistory);
    }
}


public class PatientWithHistoryDto
{
    public PatientDto Patient { get; set; }
    public List<PrescriptionDto> Prescriptions { get; set; }
}


public class PrescriptionDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
    public DoctorDto Doctor { get; set; }
}

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Dose { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class DoctorDto
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = string.Empty;
}