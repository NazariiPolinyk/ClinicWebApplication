using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication.Models
{
    public partial class Feedback : IModel
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string FeedbackText { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
