﻿using ClinicWebApplication.DataLayer.Models;

namespace ClinicWebApplication.BusinessLayer.Specification.MedicalCardRecordSpecification
{
    public class MedicalCardRecordWithDoctorSpecification : BaseSpecification<MedicalCardRecord>
    {
        public MedicalCardRecordWithDoctorSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Doctor);
        }
    }
}
