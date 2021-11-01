namespace ClinicWebApplication.Web.ViewModels
{
    class AppoinmentViewModel
    {
        public DoctorViewModel Doctor { get; set; }
        public PatientViewModel Patient { get; set; }
        public string Description { get; set; }
    }
}
