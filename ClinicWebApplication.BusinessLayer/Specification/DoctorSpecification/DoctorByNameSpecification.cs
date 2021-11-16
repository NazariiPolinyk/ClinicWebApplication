using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.DoctorSpecification
{
    public class DoctorByNameSpecification : BaseSpecification<Doctor>
    {
        public DoctorByNameSpecification(string name) : base(x => x.Name == name)
        {
            AddInclude(x => x.Feedbacks);
        }
    }
}
