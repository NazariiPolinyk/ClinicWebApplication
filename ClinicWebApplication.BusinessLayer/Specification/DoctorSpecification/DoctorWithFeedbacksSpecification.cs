using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.DoctorSpecification
{
    public class DoctorWithFeedbacksSpecification : BaseSpecification<Doctor>
    {
        public DoctorWithFeedbacksSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Feedbacks);
        }
    }
}
