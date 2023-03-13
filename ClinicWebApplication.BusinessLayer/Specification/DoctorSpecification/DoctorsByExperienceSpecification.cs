using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.DoctorSpecification
{
    public class DoctorsByExperienceSpecification : BaseSpecification<Doctor>
    {
        public DoctorsByExperienceSpecification()
        {
            AddOrderBy(x => x.Experience);
        }
    }
}
