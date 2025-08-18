using System.ComponentModel.DataAnnotations;

namespace AirForceSchoolYelahanka.Web.ViewModel.TCUpload
{
    public class TCUploadViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Student Name is required")]
        public string StudentName { get; set; }
        [Required(ErrorMessage = "Admission No is required")]
        public string AdmissionNo { get; set; }
        [Required(ErrorMessage = "Class is required")]
        public string Class { get; set; }
        public string Section { get; set; }
        [Required(ErrorMessage = "TC Issued Date is required")]
        public DateTime IssuedOn { get; set; }
        [Required(ErrorMessage = "Date of Leaving is required")]
        public DateTime DateOfLeaving { get; set; }
        public string? Remarks { get; set; }
        [Required(ErrorMessage = "Please upload the TC PDF file")]
        // For uploading
        public IFormFile? TCFile { get; set; }

        // For listing
        public string? FilePath { get; set; }
    }
}
