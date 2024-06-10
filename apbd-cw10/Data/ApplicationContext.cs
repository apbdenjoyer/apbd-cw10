using apbd_cw10.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace apbd_cw10.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

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
                e.Property(e => e.FirstName).HasMaxLength(100);
                e.Property(e => e.LastName).HasMaxLength(100);
                e.Property(e => e.Email).HasMaxLength(100);

                e.HasData(new List<Doctor>
                {
                    new() { IdDoctor = 1, FirstName = "Pan", LastName = "Doktor", Email = "p.doktor@example.com" }
                });
            });

            modelBuilder.Entity<Patient>(e =>
            {
                e.ToTable("patients");
                e.HasKey(e => e.IdPatient);
                e.Property(e => e.FirstName).HasMaxLength(100);
                e.Property(e => e.LastName).HasMaxLength(100);
                e.Property(e => e.Birthday).HasMaxLength(100);

                e.HasData(new List<Patient>
                {
                    new() { IdPatient = 1, FirstName = "Student", LastName = "Debil", Birthday = new DateTime(2003, 7, 10) }
                });
            });

            modelBuilder.Entity<Medicament>(e =>
            {
                e.ToTable("medicaments");
                e.HasKey(e => e.IdMedicament);
                e.Property(e => e.Name);
                e.Property(e => e.Description);
                e.Property(e => e.Type);

                e.HasData(new List<Medicament>
                {
                    new() { IdMedicament = 1, Name = "Ibuprofen", Description = "Smaczny nawet w dużych ilościach.", Type = "Tabletka" }
                });
            });

            modelBuilder.Entity<Prescription>(e =>
            {
                e.ToTable("prescriptions");
                e.HasKey(e => e.IdPrescription);
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

                e.HasData(new List<Prescription>
                {
                    new() { IdPrescription = 1, Date = new DateTime(2023, 1, 1), DueDate = new DateTime(2023, 1, 31), IdPatient = 1, IdDoctor = 1 }
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
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(e => e.Prescription)
                    .WithMany(e => e.Prescription_Medicaments)
                    .HasForeignKey(e => e.IdPrescription)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasData(new List<Prescription_Medicament>
                {
                    new() { IdPrescription = 1, IdMedicament = 1, Dose = 100, Details = "Jak najwięcej" }
                });
            });
        }
    }
}
