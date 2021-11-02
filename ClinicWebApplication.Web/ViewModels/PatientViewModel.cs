using System;
using System.Collections.Generic;

namespace ClinicWebApplication.Web.ViewModels
{
    public class PatientViewModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<MedicalCardRecordViewModel> MedicalCardRecords { get; set; }
    }
}
