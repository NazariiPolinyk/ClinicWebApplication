using ClinicWebApplication.Interfaces;

#nullable disable

namespace ClinicWebApplication.DataLayer.Models
{
    public partial class Feedback : IEntity
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string FeedbackText { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
