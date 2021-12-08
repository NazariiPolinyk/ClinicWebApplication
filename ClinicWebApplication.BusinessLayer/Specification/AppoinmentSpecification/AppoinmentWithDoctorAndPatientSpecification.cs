using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.AppoinmentSpecification
{
    public class AppoinmentWithDoctorAndPatientSpecification : BaseSpecification<Appoinment>
    {
        public AppoinmentWithDoctorAndPatientSpecification(int id) : base(x => x.DoctorId == id && x.IsEnable == true)
        {
            AddInclude(x => x.Doctor);
            AddInclude(x => x.Patient);
        }
    }
}
