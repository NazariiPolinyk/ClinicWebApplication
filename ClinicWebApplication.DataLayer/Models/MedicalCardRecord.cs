using System;
using ClinicWebApplication.Interfaces;

#nullable disable

namespace ClinicWebApplication.DataLayer.Models
{
    public partial class MedicalCardRecord : IEntity
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Diagnosis { get; set; }
        public DateTime DateTime { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
