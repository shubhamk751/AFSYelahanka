using AirForceSchoolYelahanka.Web.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AirForceSchoolYelahanka.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError(exceptionDetails?.Error,
                                "Unhandled exception at path: {Path} and TraceId: {TraceId}",
                                exceptionDetails?.Path,
                                HttpContext.TraceIdentifier);

            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var originalPath = statusCodeFeature?.OriginalPath;

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found.";
                    break;
            }

            _logger.LogWarning("Status code: {StatusCode} at path: {OriginalPath}, TraceId: {TraceId}",
                statusCode,
                originalPath,
                HttpContext.TraceIdentifier);

            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
