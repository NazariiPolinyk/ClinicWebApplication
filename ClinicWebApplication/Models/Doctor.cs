using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication.Models
{
    public partial class Doctor : IEntity
    {
        public Doctor()
        {
            Appoinments = new HashSet<Appoinment>();
            Feedbacks = new HashSet<Feedback>();
            MedicalCardRecords = new HashSet<MedicalCardRecord>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Experience { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Appoinment> Appoinments { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<MedicalCardRecord> MedicalCardRecords { get; set; }
    }
}
