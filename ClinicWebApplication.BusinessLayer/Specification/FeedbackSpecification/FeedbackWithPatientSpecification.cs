using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.FeedbackSpecification
{
    public class FeedbackWithPatientSpecification : BaseSpecification<Feedback>
    {
        public FeedbackWithPatientSpecification(int id) : base(x => x.DoctorId == id)
        {
            AddInclude(x => x.Patient);
        }
    }
}
