using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.MedicalCardRecordSpecification
{
    public class MedicalCardRecordsByDateDescendingSpecification : BaseSpecification<MedicalCardRecord>
    {
        public MedicalCardRecordsByDateDescendingSpecification()
        {
            AddOrderByDescending(x => x.DateTime);
        }
    }
}
