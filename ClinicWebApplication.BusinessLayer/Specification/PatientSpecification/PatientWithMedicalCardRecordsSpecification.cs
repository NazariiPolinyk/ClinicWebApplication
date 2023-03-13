using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.PatientSpecification
{
    public class PatientWithMedicalCardRecordsSpecification : BaseSpecification<Patient>
    {
        public PatientWithMedicalCardRecordsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.MedicalCardRecords);
        }
    }
}
