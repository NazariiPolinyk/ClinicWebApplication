using ClinicWebApplication.Interfaces;

#nullable disable

namespace ClinicWebApplication.DataLayer.Models
{
    public partial class Appoinment : IEntity
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Description { get; set; }
        public bool? IsEnable { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
