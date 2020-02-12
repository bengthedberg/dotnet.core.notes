# ASP.NET Core Middleware

Middleware is software that's assembled into an app pipeline to handle requests and responses. 

In ASP.NET Core, middleware are C# classes that can handle an HTTP request or response. Middleware can either:

* Handle an incoming HTTP request by generating an HTTP response.
* Process an incoming HTTP request, modify it, and pass it on to another piece of middleware.
* Process an outgoing HTTP response, modify it, and pass it on to either another piece of middleware, or the ASP.NET Core web server.

As you can see, middleware consists of small components that execute in sequence when the application receives an HTTP request. They can perform a whole host of functions, such as logging, identifying the current user for a request, serving static files, and handling errors.

The `IApplicationBuilder` that’s passed to the Configure method is used to define the order in which middleware executes. The order of the calls in this method is important, as the order they’re added to the builder in is the order they’ll execute in the final pipeline. 

Middleware can only use objects created by previous middleware
in the pipeline—it can’t access objects created by later middleware. If you’re performing authorization in middleware to restrict the users that may access your application, you must ensure it comes after the authentication middleware that identifies the current
user.

You define the middleware pipeline in code as part of your initial application configuration in Startup. You can tailor the middleware pipeline specifically to your needs –simple apps may need only a short pipeline, and large apps with a variety of features may use many more middleware. Middleware is the fundamental source of behavior in your application – ultimately the middleware pipeline is responsible for responding to any HTTP request it receives.

The request is passed to the middleware pipeline as an [HttpContext](./HttpContext.md) object. The ASP.NET Core web server builds an HttpContext object from the incoming request, which passes up and down the middleware pipeline. When you're using existing middleware to build a pipeline, this is a detail you'll rarely deal with. Its presence behind the scenes provides a route to exerting extra control over your middleware pipeline.

## Built-in middleware

ASP.NET Core ships with the following middleware components. The Order column provides notes on middleware placement in the request processing pipeline and under what conditions the middleware may terminate request processing. When a middleware short-circuits the request processing pipeline and prevents further downstream middleware from processing a request, it's called a terminal middleware. 

<table class="table">
<thead>
<tr>
<th>Middleware</th>
<th>Description</th>
<th>Order</th>
</tr>
</thead>
<tbody>
<tr>
<td><a href="../../security/authentication/identity?view=aspnetcore-3.1" data-linktype="relative-path">Authentication</a></td>
<td>Provides authentication support.</td>
<td>Before <code>HttpContext.User</code> is needed. Terminal for OAuth callbacks.</td>
</tr>
<tr>
<td><a href="/en-us/dotnet/api/microsoft.aspnetcore.builder.authorizationappbuilderextensions.useauthorization" data-linktype="absolute-path">Authorization</a></td>
<td>Provides authorization support.</td>
<td>Immediately after the Authentication Middleware.</td>
</tr>
<tr>
<td><a href="../../security/gdpr?view=aspnetcore-3.1" data-linktype="relative-path">Cookie Policy</a></td>
<td>Tracks consent from users for storing personal information and enforces minimum standards for cookie fields, such as <code>secure</code> and <code>SameSite</code>.</td>
<td>Before middleware that issues cookies. Examples: Authentication, Session, MVC (TempData).</td>
</tr>
<tr>
<td><a href="../../security/cors?view=aspnetcore-3.1" data-linktype="relative-path">CORS</a></td>
<td>Configures Cross-Origin Resource Sharing.</td>
<td>Before components that use CORS.</td>
</tr>
<tr>
<td><a href="../error-handling?view=aspnetcore-3.1" data-linktype="relative-path">Diagnostics</a></td>
<td>Several separate middlewares that provide a developer exception page, exception handling, status code pages, and the default web page for new apps.</td>
<td>Before components that generate errors. Terminal for exceptions or serving the default web page for new apps.</td>
</tr>
<tr>
<td><a href="../../host-and-deploy/proxy-load-balancer?view=aspnetcore-3.1" data-linktype="relative-path">Forwarded Headers</a></td>
<td>Forwards proxied headers onto the current request.</td>
<td>Before components that consume the updated fields. Examples: scheme, host, client IP, method.</td>
</tr>
<tr>
<td><a href="../../host-and-deploy/health-checks?view=aspnetcore-3.1" data-linktype="relative-path">Health Check</a></td>
<td>Checks the health of an ASP.NET Core app and its dependencies, such as checking database availability.</td>
<td>Terminal if a request matches a health check endpoint.</td>
</tr>
<tr>
<td><a href="../http-requests?view=aspnetcore-3.1#header-propagation-middleware" data-linktype="relative-path">Header Propagation</a></td>
<td>Propagates HTTP headers from the incoming request to the outgoing HTTP Client requests.</td>
<td aria-label="No value"></td>
</tr>
<tr>
<td><a href="/en-us/dotnet/api/microsoft.aspnetcore.builder.httpmethodoverrideextensions" data-linktype="absolute-path">HTTP Method Override</a></td>
<td>Allows an incoming POST request to override the method.</td>
<td>Before components that consume the updated method.</td>
</tr>
<tr>
<td><a href="../../security/enforcing-ssl?view=aspnetcore-3.1#require-https" data-linktype="relative-path">HTTPS Redirection</a></td>
<td>Redirect all HTTP requests to HTTPS.</td>
<td>Before components that consume the URL.</td>
</tr>
<tr>
<td><a href="../../security/enforcing-ssl?view=aspnetcore-3.1#http-strict-transport-security-protocol-hsts" data-linktype="relative-path">HTTP Strict Transport Security (HSTS)</a></td>
<td>Security enhancement middleware that adds a special response header.</td>
<td>Before responses are sent and after components that modify requests. Examples: Forwarded Headers, URL Rewriting.</td>
</tr>
<tr>
<td><a href="../../mvc/overview?view=aspnetcore-3.1" data-linktype="relative-path">MVC</a></td>
<td>Processes requests with MVC/Razor Pages.</td>
<td>Terminal if a request matches a route.</td>
</tr>
<tr>
<td><a href="../owin?view=aspnetcore-3.1" data-linktype="relative-path">OWIN</a></td>
<td>Interop with OWIN-based apps, servers, and middleware.</td>
<td>Terminal if the OWIN Middleware fully processes the request.</td>
</tr>
<tr>
<td><a href="../../performance/caching/middleware?view=aspnetcore-3.1" data-linktype="relative-path">Response Caching</a></td>
<td>Provides support for caching responses.</td>
<td>Before components that require caching.</td>
</tr>
<tr>
<td><a href="../../performance/response-compression?view=aspnetcore-3.1" data-linktype="relative-path">Response Compression</a></td>
<td>Provides support for compressing responses.</td>
<td>Before components that require compression.</td>
</tr>
<tr>
<td><a href="../localization?view=aspnetcore-3.1" data-linktype="relative-path">Request Localization</a></td>
<td>Provides localization support.</td>
<td>Before localization sensitive components.</td>
</tr>
<tr>
<td><a href="../routing?view=aspnetcore-3.1" data-linktype="relative-path">Endpoint Routing</a></td>
<td>Defines and constrains request routes.</td>
<td>Terminal for matching routes.</td>
</tr>
<tr>
<td><a href="/en-us/dotnet/api/microsoft.aspnetcore.builder.spaapplicationbuilderextensions.usespa" data-linktype="absolute-path">SPA</a></td>
<td>Handles all requests from this point in the middleware chain by returning the default page for the Single Page Application (SPA)</td>
<td>Late in the chain, so that other middleware for serving static files, MVC actions, etc., takes precedence.</td>
</tr>
<tr>
<td><a href="../app-state?view=aspnetcore-3.1" data-linktype="relative-path">Session</a></td>
<td>Provides support for managing user sessions.</td>
<td>Before components that require Session.</td>
</tr>
<tr>
<td><a href="../static-files?view=aspnetcore-3.1" data-linktype="relative-path">Static Files</a></td>
<td>Provides support for serving static files and directory browsing.</td>
<td>Terminal if a request matches a file.</td>
</tr>
<tr>
<td><a href="../url-rewriting?view=aspnetcore-3.1" data-linktype="relative-path">URL Rewrite</a></td>
<td>Provides support for rewriting URLs and redirecting requests.</td>
<td>Before components that consume the URL.</td>
</tr>
<tr>
<td><a href="../websockets?view=aspnetcore-3.1" data-linktype="relative-path">WebSockets</a></td>
<td>Enables the WebSockets protocol.</td>
<td>Before components that are required to accept WebSocket requests.</td>
</tr>
</tbody>
</table>


## Configure the Middleware

Its the `Configure` method in the Startup class where you define the middleware pipeline for the application, which is what defines the app’s behavior. 

> It’s important that you consider the order of middleware when
adding it to the pipeline. Middleware can only use objects created by earlier middleware in the pipeline.

The `IApplicationBuilder` that’s passed to the Configure method is used to define the order in which middleware executes.


Example of a middleware pipeline created in the `Configure` method:
```csharp
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }
}
```

## Create your own middleware pipeline with IApplicationBuilder

The middleware pipeline is one of the fundamental building blocks of ASP.NET Core
apps. 

Every request passes through the middleware pipeline, and each middleware component in turn gets an opportunity to modify
the request, or to handle it and return a response.

ASP.NET Core includes middleware out of the box for handling common scenarios.
You’ll find middleware for 
* serving static files, 
* for handling errors, 
* for authentication, etc.

The largest of these components is the MVC middleware. The MVC middleware is a particularly large component and is commonly where
you’ll spend most of your time during development. It serves as the entry point for
most of your app’s business logic, as it routes incoming requests to action methods,
which call methods on your app’s various business services and models.


Sometimes, however, you don’t need all of the power (and associated complexity)
that comes with the MVC middleware. You might want to *create a simple service* that,
when called, returns the current time. Or you might want to add a *health-check URL
to an existing app, where calling the URL doesn’t do any significant processing*, but
checks the app is running. Although you could use the MVC middleware for these,
you could also create small, dedicated middleware components that handle these
requirements.

Other times, you might have *requirements that lie outside the remit of the MVC
middleware*. You might want to ensure *all responses generated by your app include a
specific header*. This sort of **cross-cutting concern is a perfect fit for custom middleware**.


You could add the custom middleware early in your middleware pipeline to ensure
every response from your app includes the required header, whether it comes from the
static file middleware, the error-handling middleware, or the MVC middleware.

### Three ways to create custom middleware components

**Creating simple endpoints with the Run extension**



### How to create branches in your middleware pipeline

