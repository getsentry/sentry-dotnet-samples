using System;
using System.Diagnostics;
using System.Collections; 
using System.Collections.Generic; 
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sentry.Samples.AspNetCore.Mvc.Models;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Sentry;
using Microsoft.AspNetCore.Http.Internal;
using Sentry.Samples.AspNetCore.Mvc;


public class User
{
    public string Email { get; set; }
}
public static class Store
{
    public static JObject inventory = new JObject
    {
        { "wrench", 1 },
        { "nails", 1 },
        { "hammer", 1 }
    };
}
public class Item
{
    public string id;
    public Item(JToken item)
    {
        id = item["id"].ToString();
    }
    public string getId()
    {
        return id;
    }
}
public class Order
{
    public List<Item> cart;
    public string email;
    public Order(string body)
    {
        JObject order = JObject.Parse(body);
        email = order["email"].ToString();
        cart = new List<Item> {};
        foreach (var item in order["cart"])
        {
            Item cartItem = new Item(item);
            cart.Add(cartItem);
        };
    }

    public List<Item> getCart()
    {
        return cart;
    }

    public string getEmail()
    {
        return email;
    }
}

   

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

        [HttpPost]
        public async Task<String> checkout()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                reader.Close();
                // OLD
                // JObject order = JObject.Parse(body);
                // JToken cart = order["cart"];

                // NEW
                Order order = new Order(body);
                List<Item> cart = order.getCart();
                // _logger.LogInformation("\nXXXXXXXXX LENGTH " + cart.);


                // MIDDLEWARE - User, Tags (transaction_id, session_id), Headers
                String email = order.getEmail(); //["email"].ToString();
                String transaction_id = Request.Headers["X-transaction-ID"];
                String session_id = Request.Headers["X-session-ID"];
                SentrySdk.ConfigureScope(scope =>
                {
                    scope.User = new Sentry.Protocol.User
                    {
                        Email = email
                    };
                });
                SentrySdk.ConfigureScope(scope =>
                {
                    scope.SetTag("transaction_id", transaction_id);
                });
                SentrySdk.ConfigureScope(scope =>
                {
                    scope.SetTag("session_id", session_id);
                });
                SentrySdk.ConfigureScope(scope => 
                {
                    scope.SetExtra("inventory", Store.inventory);
                });

                _logger.LogInformation("\n*********** BEFORE " + Store.inventory.ToString());

                JObject tempInventory = Store.inventory;
                foreach (Item item in cart)
                {
                    if (Store.inventory[item.getId()].ToObject<int>() <= 0)
                    {
                        throw new Exception("Not enough inventory for " + item.getId());
                    }
                    else
                    {
                        tempInventory[item.getId()] = tempInventory[item.getId()].ToObject<int>() - 1;
                    }
                }
                Store.inventory = tempInventory;

                _logger.LogInformation("\n*********** AFTER " + Store.inventory.ToString());
                return "SUCCESS: checkout";
            }


        }


        [HttpGet]
        public async Task<String> handled([FromServices] IHub sentry)
        {
            try
            {
                // TODO - consider math exception as its a more readable error on Sentry ("Can't divide by 0" vs "NullExceptionValue")
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
