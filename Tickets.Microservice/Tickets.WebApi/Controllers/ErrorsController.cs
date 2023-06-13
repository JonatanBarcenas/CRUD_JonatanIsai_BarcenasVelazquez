using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Passengers.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : Controller
    {
        private readonly ILogger _logger;

        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        [Route("/error-development")]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                return NotFound();
            }

            var exceptionHandlesFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            _logger.LogError(exceptionHandlesFeature.Error, "Unhandled Exception");

            return Problem(
                detail: exceptionHandlesFeature.Error.StackTrace,
                title: exceptionHandlesFeature.Error.Message
                );
        }
        [Route("/error")]
        public IActionResult handleError()
        {
            var exceptionHandlesFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError(exceptionHandlesFeature.Error, "Unhandled Exception");

            return Problem();
        }
    }
}
