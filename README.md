###Requirements

- Dotnet Core 3.0
- Visual Studio 2019

### Database

 - Open Package Manager Console
 - Update-Database -Context AppDbContext (Paste this to console)

### Requests

#### Add Article
 
Example for 3rd party request service:

https://localhost:44362/api/article/AddArticle POST 

**Pre-request Script:** 

```
var currentTimestampValue = new Date();
postman.setEnvironmentVariable("currentTimestampValue", currentTimestampValue.toISOString());
```

**Body:** 

```json
{
	"CreationTime": "{{currentTimestampValue}}",
	"Title": "Sample Title",
	"Content": "Sample Content Sample Content",
	"ViewCount": 0,
	"ApplauseAmount": 0,
	"Keywords": "sample,text,life",
	"OwnerId": 1
}
```

