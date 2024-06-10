using System.ComponentModel.DataAnnotations;

namespace apbd_cw10.Models;

public class Doctor
{
    
    
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    [EmailAddress] public string Email { get; set; } = string.Empty;
    public ICollection<Prescription> Prescriptions { get; set; } = new HashSet<Prescription>();
}

public class DoctorDto
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    [EmailAddress] public string Email { get; set; } = string.Empty;
}