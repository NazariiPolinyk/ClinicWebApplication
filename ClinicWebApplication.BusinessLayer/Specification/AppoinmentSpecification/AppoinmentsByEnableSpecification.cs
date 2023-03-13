using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.AppoinmentSpecification
{
    public class AppoinmentsByEnableSpecification : BaseSpecification<Appoinment>
    {
        public AppoinmentsByEnableSpecification() : base(x => x.IsEnable == true)
        {

        }
    }
}
