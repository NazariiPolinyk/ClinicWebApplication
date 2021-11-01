using System;

namespace ClinicWebApplication.Web.ViewModels
{
    class MedicalCardRecordViewModel
    {
        public DoctorViewModel Doctor { get; set; }
        public string Diagnosis { get; set; }
        public DateTime DateTime { get; set; }
    }
}
