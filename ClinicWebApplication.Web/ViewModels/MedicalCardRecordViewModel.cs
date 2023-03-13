using System;

namespace ClinicWebApplication.Web.ViewModels
{
    public class MedicalCardRecordViewModel
    {
        public DoctorViewModel Doctor { get; set; }
        public string Diagnosis { get; set; }
        public DateTime DateTime { get; set; }
    }
}
