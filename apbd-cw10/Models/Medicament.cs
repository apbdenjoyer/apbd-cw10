namespace apbd_cw10.Models;

public class Medicament
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public ICollection<Prescription_Medicament> Prescriptions_Medicaments { get; set; } = new HashSet<Prescription_Medicament>();
}

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}