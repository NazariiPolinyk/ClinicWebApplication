using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ClinicWebApplication.Web.MappingProfiles;

namespace ClinicWebApplication.Tests
{
    static class UnitTestUtility
    {
        internal static T GetTestObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
        internal static IMapper CreateTestMapper()
        {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                return mapper;
        }
    }
}
