namespace AirForceSchoolYelahanka.Web.Models
{
    public class TCUpload
    {
        public int Id { get; set; }
        public string AdmissionNo { get; set; }
        public string StudentName { get; set; }
        public string Class { get; set; }
        public string? Section { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime DateOfLeaving { get; set; }
        public string? Remarks { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedOn { get; set; }
        public string UploadedBy { get; set; }
    }
}
