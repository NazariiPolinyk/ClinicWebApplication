using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.PatientSpecification
{
    public class PatientByNameSpecification : BaseSpecification<Patient>
    {
        public PatientByNameSpecification(string name) : base(x => x.Name == name)
        {
            AddInclude(x => x.MedicalCardRecords);
        }
    }
}
