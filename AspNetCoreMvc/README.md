# Summary 
- bing
- bang
- boom

# Initial Setup & Run
1. Configure your DSN in [appsettings.json](appsettings.json)
2. Configure your org slug and project slug in [deploy.ps1](deploy.ps1)
3. Powershell `pwsh`
4. `cd AspNetCoreMvc` 
5. `./deploy.ps1` builds the Sentry release and runs the app
6. `http://localhost:62920/Home/handled`
or
7. Postman `localhost:62920/Home/checkout`
```
{ 
	"cart": [
		{ 
			"id": "wrench"
		}
	],'
	"email": "mikejo@msn.com"
}
// X-transaction-ID: 11223
// X-session-ID: 22334
```

# GIF
here