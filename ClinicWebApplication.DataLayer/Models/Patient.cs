using System;
using System.Collections.Generic;
using ClinicWebApplication.Interfaces;

#nullable disable

namespace ClinicWebApplication.DataLayer.Models
{
    public partial class Patient : IEntity, IAccount
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
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        public virtual ICollection<Appoinment> Appoinments { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<MedicalCardRecord> MedicalCardRecords { get; set; }
    }
}
