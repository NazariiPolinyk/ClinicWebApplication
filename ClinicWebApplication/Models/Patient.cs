using System;
using System.Collections.Generic;

#nullable disable

namespace ClinicWebApplication.Models
{
    public partial class Patient : IModel
    {
        public Patient()
        {
            Appoinments = new HashSet<Appoinment>();
            Feedbacks = new HashSet<Feedback>();
            MedicalCardRecords = new HashSet<MedicalCardRecord>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }

        public virtual ICollection<Appoinment> Appoinments { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<MedicalCardRecord> MedicalCardRecords { get; set; }
    }
}
