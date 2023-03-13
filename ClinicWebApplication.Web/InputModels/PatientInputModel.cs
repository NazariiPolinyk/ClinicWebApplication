using System;

namespace ClinicWebApplication.Web.InputModels
{
    public class PatientInputModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
