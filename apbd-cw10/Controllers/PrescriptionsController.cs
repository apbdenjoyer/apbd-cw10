using apbd_cw10.Data;
using apbd_cw10.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw10.Controllers
{
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
                p => p.IdPatient == request.Patient.IdPatient);

            if (patient == null)
            {
                patient = new Patient()
                {
                    FirstName = request.Patient.FirstName,
                    LastName = request.Patient.LastName,
                    Birthday = request.Patient.Birthday,
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                request.Patient.IdPatient = patient.IdPatient;
                
                /*Console.WriteLine($"Added new patient: {patient.IdPatient}, {patient.FirstName}, {patient.LastName}");*/
            }

            foreach (var medicament in request.Medicaments)
            {
                if (!await _context.Medicaments.AnyAsync(m => m.IdMedicament == medicament.IdMedicament))
                {
                    return NotFound($"Medicament with Id {medicament.IdMedicament} not found.");
                }
            }

            if (request.Medicaments.Count > 10)
            {
                return BadRequest($"Too many medicaments: {request.Medicaments.Count}, Allowed maximum: 10");
            }

            if (request.Prescription.DueDate < request.Prescription.Date)
            {
                return BadRequest(
                    $"Time travel detected - DueDate happening before Date: {request.Prescription.DueDate} < {request.Prescription.Date}");
            }


            if (!await _context.Prescriptions.AnyAsync(p => p.IdPrescription == request.Prescription.IdPrescription))
            {
                return BadRequest($"Prescription with ID {request.Prescription.IdPrescription} already exists.");
            }

            var prescription = new Prescription()
            {
                Date = request.Prescription.Date,
                DueDate = request.Prescription.DueDate,
                IdPatient = request.Patient.IdPatient,
                IdDoctor = 1
            };

            _context.Prescriptions.Add(prescription);

            await _context.SaveChangesAsync();

            /*if (_context.Prescriptions.Contains(prescription))
            {
                Console.WriteLine(
                    $"Added new prescription: {prescription.IdPrescription}, {prescription.Date}, {prescription.DueDate}, {prescription.IdPatient}");
            }*/

            return Created();
        }
    }

    public class AddPrescription
    {
        public PatientDto Patient { get; set; }
        public List<MedicamentDto> Medicaments { get; set; }
        public PrescriptionDto Prescription { get; set; }
    }
}