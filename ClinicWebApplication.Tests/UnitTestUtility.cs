using Microsoft.AspNetCore.Mvc;

namespace ClinicWebApplication.Tests
{
    static class UnitTestUtility
    {
        internal static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
    }
}
