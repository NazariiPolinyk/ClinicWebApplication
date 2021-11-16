using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.DoctorSpecification
{
    public class DoctorsByCategorySpecification : BaseSpecification<Doctor>
    {
        public DoctorsByCategorySpecification(string category) : base(x => x.Category == category)
        {

        }
    }
}
