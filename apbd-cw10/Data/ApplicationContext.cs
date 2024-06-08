using apbd_cw10.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw10.Data;

public class ApplicationContext : DbContext
{
    protected ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Prescription_Medicament> Prescription_Medicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Doctor>(e =>
        {
            e.ToTable("doctors");
            e.HasKey(e => e.IdDoctor);
            e.Property(e => e.IdDoctor).ValueGeneratedOnAdd();
            e.Property(e => e.FirstName).HasMaxLength(100);
            e.Property(e => e.LastName).HasMaxLength(100);
            e.Property(e => e.Email).HasMaxLength(100);

            e.HasData(new List<Doctor>()
            {
                new() { IdDoctor = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new() { IdDoctor = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" },
                new() { IdDoctor = 3, FirstName = "Emily", LastName = "Johnson", Email = "emily.johnson@example.com" },
                new() { IdDoctor = 4, FirstName = "Michael", LastName = "Brown", Email = "michael.brown@example.com" }
            });
        });

        modelBuilder.Entity<Patient>(e =>
        {
            e.ToTable("patients");
            e.HasKey(e => e.IdPatient);
            e.Property(e => e.IdPatient).ValueGeneratedOnAdd();
            e.Property(e => e.FirstName).HasMaxLength(100);
            e.Property(e => e.LastName).HasMaxLength(100);
            e.Property(e => e.Birthday).HasMaxLength(100);

            e.HasData(new List<Patient>()
            {
                new() { IdPatient = 1, FirstName = "John", LastName = "Doe", Birthday = new DateTime(2003, 7, 10) },
                new() { IdPatient = 2, FirstName = "Jane", LastName = "Smith", Birthday = new DateTime(2003, 9, 15) },
                new() { IdPatient = 3, FirstName = "Emily", LastName = "Johnson", Birthday = new DateTime(2003, 5, 31) },
                new() { IdPatient = 4, FirstName = "Michael", LastName = "Brown", Birthday = new DateTime(2003, 11, 17) }
            });
        });

        modelBuilder.Entity<Medicament>(e =>
        {
            e.ToTable("medicaments");
            e.HasKey(e => e.IdMedicament);
            e.Property(e => e.IdMedicament).ValueGeneratedOnAdd();
            e.Property(e => e.Name);
            e.Property(e => e.Description);
            e.Property(e => e.Type);

            e.HasData(new List<Medicament>()
            {
                new() { IdMedicament = 1, Name = "Aspirin", Description = "Pain reliever and anti-inflammatory", Type = "Tablet" },
                new() { IdMedicament = 2, Name = "Amoxicillin", Description = "Antibiotic for bacterial infections", Type = "Capsule" },
                new() { IdMedicament = 3, Name = "Lisinopril", Description = "Used to treat high blood pressure", Type = "Tablet" },
                new() { IdMedicament = 4, Name = "Metformin", Description = "Used to treat type 2 diabetes", Type = "Tablet" }
            });
        });

        modelBuilder.Entity<Prescription>(e =>
        {
            e.ToTable("prescriptions");
            e.HasKey(e => e.IdPrescription);
            e.Property(e => e.IdPrescription).ValueGeneratedOnAdd();
            e.Property(e => e.Date);
            e.Property(e => e.DueDate);

            e.HasOne(e => e.Doctor)
                .WithMany(e => e.Prescriptions)
                .HasForeignKey(e => e.IdDoctor)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(e => e.Patient)
                .WithMany(e => e.Prescriptions)
                .HasForeignKey(e => e.IdPatient)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasData(new List<Prescription>()
            {
                new() { IdPrescription = 1, Date = new DateTime(2023, 1, 1), DueDate = new DateTime(2023, 1, 31), IdPatient = 1, IdDoctor = 1 },
                new() { IdPrescription = 2, Date = new DateTime(2023, 2, 1), DueDate = new DateTime(2023, 2, 28), IdPatient = 2, IdDoctor = 2 },
                new() { IdPrescription = 3, Date = new DateTime(2023, 3, 1), DueDate = new DateTime(2023, 3, 31), IdPatient = 3, IdDoctor = 3 },
                new() { IdPrescription = 4, Date = new DateTime(2023, 4, 1), DueDate = new DateTime(2023, 4, 30), IdPatient = 4, IdDoctor = 4 }
            });
        });


        modelBuilder.Entity<Prescription_Medicament>(e =>
        {
            e.ToTable("prescriptions_medicaments");
            e.HasKey(e => new { e.IdPrescription, e.IdMedicament }); // Composite key definition
            e.Property(e => e.Dose);
            e.Property(e => e.Details);

            e.HasOne(e => e.Medicament)
                .WithMany(e => e.Prescriptions_Medicaments)
                .HasForeignKey(e => e.IdMedicament);

            e.HasOne(e => e.Prescription)
                .WithMany(e => e.Prescription_Medicaments)
                .HasForeignKey(e => e.IdPrescription)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasData(new List<Prescription_Medicament>()
            {
                new() { IdPrescription = 1, IdMedicament = 1, Dose = 100, Details = "Take one tablet daily after meals" },
                new() { IdPrescription = 1, IdMedicament = 2, Dose = 250, Details = "Take one capsule every 8 hours" },
                new() { IdPrescription = 2, IdMedicament = 3, Dose = 10, Details = "Take one tablet daily in the morning" },
                new() { IdPrescription = 3, IdMedicament = 4, Dose = 500, Details = "Take one tablet twice daily with meals" },
                new() { IdPrescription = 4, IdMedicament = 1, Dose = 100, Details = "Take one tablet daily after meals" },
                new() { IdPrescription = 4, IdMedicament = 3, Dose = 10, Details = "Take one tablet daily in the morning" }
            });
        });
    }
}