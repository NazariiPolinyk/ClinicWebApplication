using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ClinicWebApplication.Models
{
    public partial class ClinicContext : DbContext
    {
        public ClinicContext()
        {
        }

        public ClinicContext(DbContextOptions<ClinicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appoinment> Appoinments { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<MedicalCardRecord> MedicalCardRecords { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Appoinment>(entity =>
            {
                entity.ToTable("Appoinment");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.IsEnable)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Appoinments)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appoinment_Doctor");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appoinments)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appoinment_Patient");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctor");

                entity.Property(e => e.Category).IsRequired();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnType("image");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.FeedbackText).IsRequired();

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_Doctor");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_Patient");
            });

            modelBuilder.Entity<MedicalCardRecord>(entity =>
            {
                entity.ToTable("MedicalCardRecord");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.Diagnosis).IsRequired();

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.MedicalCardRecords)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MedicalCardRecord_Doctor");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.MedicalCardRecords)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MedicalCardRecord_Patient");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
