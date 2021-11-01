﻿using System;
using System.Collections.Generic;
using ClinicWebApplication.Interfaces;

#nullable disable

namespace ClinicWebApplication.DataLayer.Models
{
    public partial class Patient : IEntity
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