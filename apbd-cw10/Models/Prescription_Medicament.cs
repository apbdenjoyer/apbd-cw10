namespace apbd_cw10.Models;

public class Prescription_Medicament
{
    public int IdMedicament { get; set; }
    public Medicament Medicament { get; set; } = null!;
    public int IdPrescription { get; set; }
    public Prescription Prescription { get; set; } = null!;
    public int Dose { get; set; } = 0;
    public string Details { get; set; } = String.Empty;
}