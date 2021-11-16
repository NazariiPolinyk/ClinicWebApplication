using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.DoctorSpecification
{
    public class DoctorsByExperienceDescendingSpecification : BaseSpecification<Doctor>
    {
        public DoctorsByExperienceDescendingSpecification()
        {
            AddOrderByDescending(x => x.Experience);
        }
    }
}
