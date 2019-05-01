=======
To run this sample, edit [appsettings.json](appsettings.json) and **set your DSN** to see the events in Sentry.

Running the Demo
1. Configure your DSN in [appsettings.json](appsettings.json)
2. Configure your org slug and project slug in [deploy.ps1](deploy.ps1)
3. Run `./deploy.ps1` from PowerShell `pwsh`

![Alt Text](configure-launch-demo.gif)

Check [Program.cs](Program.cs) to see some Sentry customization done via the `WebHostBuilder`.

### In this sample we demonstrate the following:

* Initialize the SDK via the ASP.NET Integration (Program.cs UseSentry())
* Configure the SDK via the framework configuration system (appsettings.json, env var, etc)
* Using a custom `ExceptionProcessor`. (See `SpecialExceptionProcessor.cs` which takes dependencies vai DI)
* Adding custom data to be reported with the exception (See `HomeController.cs` `PostIndex` action, the catch block adding to `Data` property of the exception)
* Capturing an exception manually via an injected `IHub`. (See `HomeController.cs` `Contact` action.)
* Captures an unhandled exception coming from a view (See `About.cshtml` which throws an exception that goes through the `SpecialExceptionProcessor.cs`)
* Including the request payload with the event (done via appsettings.json `IncludeRequestPayload`)
* Sentry commits/releases integration using `sentry-cli` (done in deploy.ps1)



# New
1. `pwsh`
2. `cd AspNetCoreMvc` 
2. `./deploy.ps1`
3. `http://localhost:62920/Home/handled`
or
4. Postman
```
{ 
	"cart": [
		{ 
			"id": "wrench"
		}
	],
	"email": "mikejo@msn.com"
}
```

PR Notes...
for json deserialization...
Install-Package Newtonsoft.Json
https://www.newtonsoft.com/json
JsonConvert.DeserializeObject(order); ?

// name = (string) jUser["name"];

// if (order.GetType().GetProperty("cart") != null)



var User = new { email = email };
// scope.User = new { email = email }; // FAILS
// scope.User = User; // FAILS


JToken jCart = order["cart"];
JToken jEmail = order["email"];


https://weblog.west-wind.com/posts/2013/dec/13/accepting-raw-request-body-content-with-aspnet-web-api