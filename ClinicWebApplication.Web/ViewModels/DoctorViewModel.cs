using System.Collections.Generic;

namespace ClinicWebApplication.Web.ViewModels
{
    public class DoctorViewModel
    {
        public string Name { get; set; }
        public int Experience { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public ICollection<FeedbackViewModel> Feedbacks { get; set; }
    }
}
