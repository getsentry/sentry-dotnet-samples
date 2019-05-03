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
4. Postman `localhost:62920/Home/checkout`
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

### Q's to Answer
// MIDDLEWARE
// var body = reader.ReadToEnd();
// _logger.LogInformation(body);
// reader.Close();
// if (body != "") then set the email

// - get; set; not working

//  consider math exception as its a more readable error on Sentry ("Can't divide by 0" vs "NullExceptionValue")
			// int n1 = 1; // or a different exception type (see deprecated code or online examples)