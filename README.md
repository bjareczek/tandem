# tandem

Tandem RESTful API endpoints

POST Entity (first, middle, last, phone, email)
	
	Generate GUID userId on POST… or PUT?
	
	URL will be:
		
		POST /tandem/api/v1/users
		
		Body will contain json with user data
		
	Possible return codes will be:
		
		201 - Created
		400 - Bad Request (verify we have proper payload and required data points)
		500 - Internal Error
		
	Note: if I had more time to do this exercise there would be both authentication and authorization of some sort in place.
	
		Authentication:  most likely we have a BFF in place that will talk with this API.  The BFF will have already authenticated the user at login.  There is a token exchange between the UI web app and the BFF.  Authentication between the BFF and this API may be in the form of a subscription key and a white listed API (list of valid Azure IPs).
	
		Authorization:  At minimum if this is user data to be served up to a customer facing web app, we could do some user authorization in the BFF.  Our BFF User Identity for the logged in user will have the User Id, so we can make sure the requested user id passed in matches the User Id held in the ASP.Net Core User Identity object.

GET Notes (userId, phone, email)

	URL will be:
		
		GET /tandem/api/v1/users/{id}
			
	Possible response codes will be:
	
		200 - Ok
		400 - Bad request (verify we have proper payload and required data points)
		404 - Not Found (if no user for user id parameter)
		500 - Internal Error
		
GET cosmos health check

	Do a quick cosmos connectivity check.
	
	Possible http response codes will be:
		200 - Ok
		500 - Internal Error
		
	Return payload options:
		1. { 
				"cosmosConnectivity": "ok",
				"provisionedThroughput": 400
			}
		2. { "cosmosConnectivity": "faulty" }
		
	Allow Anonymous requests even if we had Authentication and Authorization in place
	
	NOTE:  if I had more time, I might make this health check more robust.  We could pass back what I've learned to be pretty useful information like:
	
		1. Application build version
		2. Actual application status (sometimes an API can be "up", but we can query for faulting web app status as well)
	
Logger - we want trace logging!

	.Net core has a solid logger, so we can use it.
	Log freely.  When using Kusto to query application insights, these trace logs can be very telling and paint a very clear picture of what a user was doing (or attempting to accomplish).
	Allow exceptions to rise.  Application Insights will give us more than enough information for now.
	
Swagger

	Publish our API contracts so our API is a pleasure to consume by other developers
	
Cosmos Db

	Will use userId as the partition key even though we know at this point that we will query the user notes by emailAddress.  It is very common for systems to later allow users to have more than one email address for an account, so not a best practice to use emailAddress as a unique identifier.
	
	We will use default * index for now as it looks like any of the current fields could be used in a search.
	
	Use appSettings to hold the cosmos URL
	
	NOTE:  depending on our concurrency requirements, I would consider using the document's etag property in the API service if we possibly have competing resources updating the same document at the same time.
	
	I would use Azure Key Vault to store and retrieve at least the Production cosmos URL for the best security.  For now appSettings is good enough to allow us to work locally with ease and to be able to update the cosmos db URL in our Azure Function appSettings in Azure Portal.
	
		
GENERAL API NOTES:

	If I had more time, I would consider use of APIM for several reasons such as:
	
		1. Security
		2. Versioning
		3. API Contracts can be published to other developers here, thus not requiring swagger to be integrated with all API web apps (not that there is anything wrong with the latter approach)
	
	APIM does make API versioning very easy to manage.  This can become much more apparent when we have both mobile and web clients using the same API, with possibly many mobile clients using different versions of our API.
	
	Docker:  we don't use Docker or Kubernetes at ABC Supply, although we do have a very robust DevOps deployment platform using Azure DevOps.  I currently use Docker on my MacBook for the sole purpose of hosting a local SQL Server instance when I don't want to fire up VMWare Fusion.  I've been a Windows guy for many years, but this years MacBook Pro 16' is AWESOME!  I had to get it.
	
	Mutate/modify endpoint:  I decided to forego this optional feature, but can tell you that I would use a PUT endpoint using the proper RESTful URL format just like the create POST endpoint, but of course this would be a PUT method in the same /tandem/api/v1/users controller (and underlying service).  The HTTP response codes would also include:
	
		200 - Ok
		404 - resource not found if we get a bad user id
		401 -- Unauthorized
		403 -- IF we were supporting multiple roles and we implemented permissions, for which I would consider using ASP.Net core's built in User Identity object but in the BFF, closer to the UI.
		500 - Internal Error
		
	
	Mediator Pattern:  I will be honest and say I have never been at a place where we used the Mediator Pattern.  From a quick search it looks like it is the same or similar to the CRQS pattern, which I have read about in the past but again, I have never been worked in a place that we implemented this pattern.  I did work at a place for several years that we studied and practiced the SOLID principles more than just in theory.  I believe that this pattern overlaps with the SRP, but I also recall that there is more to the CRQS pattern like:  I should be able to POST the same object multiple times and get the same exact result every time.  Again, to be totally honest, I've never implemented this pattern personally and it has been a long time since I read about it.
	
	AutoMapper:  We use this currently at ABC Supply (for the past 2 years) and I've used it previously. In prior years I only used the most basic mapping capabilities when I needed to map slightly different payloads to the same objects.  At ABC Supply, however, I've actually implemented some pretty complex logic using the Before and After events to determine an order payload's OrderType and OrderStatus.  There are some very wonky business rules used to determine these data points.  Our cosmos db is inserted and updated several hundred thousand times a day because of the different stages of an order and because of delivery status updates.  I wanted to put this logic in our Order Ingest micro service but that is an Azure v1 function and I was not given the go-ahead to put the logic there (priorities).  I would like our cosmos db order documents to already have the current OrderType and OrderStatus data points already -- maybe some day.
	
	FluentValidation:  Again to be honest, I have not used this library.  I have always used the built in ModelState, but I'd be happy to learn about FluentValidation!  Maybe this is for something completely different from ModelState; I just didn't have time to read up on it this weekend.

