namespace ClinicWebApplication.Web.ViewModels
{
    public class AppoinmentViewModel
    {
        public DoctorViewModel Doctor { get; set; }
        public PatientViewModel Patient { get; set; }
        public string Description { get; set; }
    }
}
