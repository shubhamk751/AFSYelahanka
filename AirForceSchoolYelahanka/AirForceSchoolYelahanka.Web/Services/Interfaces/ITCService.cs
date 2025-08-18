using AirForceSchoolYelahanka.Web.ViewModel.TCUpload;

namespace AirForceSchoolYelahanka.Web.Services.Interfaces
{
    public interface ITCService
    {
        Task<List<TCUploadViewModel>> GetTCsIssuedAsync();
    }
}
