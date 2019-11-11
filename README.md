[![NuGet](https://img.shields.io/nuget/dt/Marten.AspNetIdentity.svg)](https://www.nuget.org/packages/Marten.AspNetIdentity/)

# MartenAspNetIdentity

MartenAspNetIdentity is a [Marten DocumentDb](http://jasperfx.github.io/marten/) implementation of [Microsoft.AspNetCore.Identity](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity?view=aspnetcore-2.1).

## Example usage

```
// This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services)
{
    services.AddLogging();

    string connectionString = "server=localhost;database=aspnetidentity;uid=aspnetidentity;pwd=aspnetidentity;";

    services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddMartenStores<ApplicationUser, IdentityRole>(connectionString)
        .AddDefaultTokenProviders();

    // more service wireup...
}
```

* `ApplicationUser` is your user class to store.
* `IdentityRole` is the role to store.

This will create an `IDocumentStore` for you, create a database and using the connection string provided, and add it to your services. If you already have an `IDocumentStore` configured in your DI container, you can use the overload without the connection string, the MartenAspNetIdentity will use this for its managers:

```
services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddMartenStores<ApplicationUser, IdentityRole>()
        .AddDefaultTokenProviders();
```

There is a full example in this repository under the [Example project](https://github.com/roadkillwiki/Marten.AspNetIdentity/tree/master/src/Marten.AspNetIdentity.Example)
