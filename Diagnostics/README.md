# Diagnostics

Your application doesn't always behave as you would expect, but .NET Core has tools and APIs that will help you diagnose these issues quickly and effectively.

[Diagnostics](https://docs.microsoft.com/en-us/dotnet/core/diagnostics/)

[Error Handling in ASP .NET Core Applications](https://blog.dudak.me/2017/error-handling-in-asp-net-core-applications/)

### DeveloperExceptionPage

By default, the ASP.NET Core application simple returns a status code for an exception that is not handled by the application. If you want your application to display a page that shows the detailed information about the unhandled exception, then you need to use the Developer Exception Page middleware.

[more information](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1)

### ExceptionHandlerMiddleware

Customise the `ExceptionHandlerMiddleware` to create custom responses when an error occurs in your middleware pipeline.

[more information](https://andrewlock.net/creating-a-custom-error-handler-middleware-function/)

### StatusCodePagesMiddleware

StatusCodePages Middleware middleware is similar to ExceptionHandler Middleware middleware, which uses an error processor to complete the final request processing and response tasks in case of “error” in subsequent request processing. The difference between them lies in the definition of “error”. For Exception Handler Middleware, the so-called error is throwing an exception, but for Status Code Pages Middleware, the response state code between 400 and 599 is regarded as an error. 