using AutoMapper;
using ClinicWebApplication.DataLayer.Models;
using ClinicWebApplication.Web.InputModels;
using ClinicWebApplication.Web.ViewModels;

namespace ClinicWebApplication.Web.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ConfigureMappingProfile();
        }
        private void ConfigureMappingProfile()
        {
            CreateMap<Patient, PatientViewModel>()
                .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Phone,
                opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.BirthDate,
                opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.MedicalCardRecords,
                opt => opt.MapFrom(src => src.MedicalCardRecords)).ReverseMap();

            CreateMap<Doctor, DoctorViewModel>()
                .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Category,
                opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Experience,
                opt => opt.MapFrom(src => src.Experience))
                .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Feedbacks,
                opt => opt.MapFrom(src => src.Feedbacks)).ReverseMap();

            CreateMap<Appoinment, AppoinmentViewModel>()
                .ForMember(dest => dest.Patient,
                opt => opt.MapFrom(src => src.Patient))
                .ForMember(dest => dest.Doctor,
                opt => opt.MapFrom(src => src.Doctor))
                .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Description)).ReverseMap();

            CreateMap<Feedback, FeedbackViewModel>()
                .ForMember(dest => dest.Patient,
                opt => opt.MapFrom(src => src.Patient))
                .ForMember(dest => dest.FeedbackText,
                opt => opt.MapFrom(src => src.FeedbackText)).ReverseMap();

            CreateMap<MedicalCardRecord, MedicalCardRecordViewModel>()
                .ForMember(dest => dest.Doctor,
                opt => opt.MapFrom(src => src.Doctor))
                .ForMember(dest => dest.Diagnosis,
                opt => opt.MapFrom(src => src.Diagnosis))
                .ForMember(dest => dest.DateTime,
                opt => opt.MapFrom(src => src.DateTime)).ReverseMap();
        }
    }
}
