using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sentry.Samples.AspNetCore.Mvc.Models;
// CURRENT
// POST http://localhost:62920/Home/PostIndex
// POST http://localhost:62920/Home/PostIndexUnhandled
// POST http://localhost:62920/Home/PostIndex

// GOAL simplified for thinkocapo/sentry-dotnet-samples and Back-End Demo Spec, eCommerce Checkout
// GET  http://localhost:62920/Home/handled
// GET  http://localhost:62920/Home/unhandled
// POST http://localhost:62920/Home/PostIndex


namespace Sentry.Samples.AspNetCore.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IGameService gameService, ILogger<HomeController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
         
        // _logger.LogWarning("Param is null!", @params);
        [HttpGet]
        public async Task<String> handled([FromServices] IHub sentry)
        {
            try
            {
                // int n1 = 1;
                // int n2 = 0;
                // int ans = n1 / n2;
                throw null; // System.NullReferenceException - Object reference not set to an instance of an object.
            }
            catch (Exception exception)
            {
                exception.Data.Add("detail",
                    new
                    {
                        Reason = "There's a 'throw null' hard-coded in the try block"
                    });

                var id = sentry.CaptureException(exception); // ViewData["Message"] = "An exception was caught and sent to Sentry! Event id: " + id;
            }
            return "SUCCESS: back-end error handled gracefully";
        }


        [HttpGet]
        public async Task<String> unhandled()
        {
            int n1 = 1;
            int n2 = 0;
            int ans = n1 / n2;
            return "FAILURE: Server-side Error";
        }


        [HttpGet]
        public async Task<String> checkout()
        {
            _logger.LogWarning("******* Checking out *******");
            return "SUCCESS: checkout";
        }



        // Example: An exception that goes unhandled by the app will be captured by Sentry:
        [HttpPost]
        public async Task PostIndex(string @params)
        {
            try
            {
                if (@params == null)
                {
                    _logger.LogWarning("Param is null!", @params);
                }

                await _gameService.FetchNextPhaseDataAsync();
            }
            catch (Exception e)
            {
                var ioe = new InvalidOperationException("Bad POST! See Inner exception for details.", e);

                ioe.Data.Add("inventory",
                    // The following anonymous object gets serialized:
                    new
                    {
                        SmallPotion = 3,
                        BigPotion = 0,
                        CheeseWheels = 512
                    });

                throw ioe;
            }
        }

        // Example: An exception that goes unhandled by the app will be captured by Sentry:
        [HttpPost]
        public async Task PostIndexUnhandled(string @params)
        {
            if (@params == null)
            {
                _logger.LogWarning("Param is null!", @params);
            }
            await _gameService.FetchNextPhaseDataAsync();
        }

        // Example: The view rendering throws: see about.cshtml
        public IActionResult About()
        {
            return View();
        }

        // Example: To take the Sentry Hub and submit errors directly:
        public IActionResult Contact(
            // Hub holds a Client and Scope management
            // Errors sent with the hub will include all context collected in the current scope
            [FromServices] IHub sentry)
        {
            try
            {
                // Some code block that could throw
                throw null;
            }
            catch (Exception e)
            {
                e.Data.Add("detail",
                    new
                    {
                        Reason = "There's a 'throw null' hard-coded here!",
                        IsCrazy = true
                    });

                var id = sentry.CaptureException(e);

                ViewData["Message"] = "An exception was caught and sent to Sentry! Event id: " + id;
            }
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
