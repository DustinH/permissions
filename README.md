# RimDev Abilities

**Abilities** is a permissions library built to work with [ASP.NET]() [MVC 5]() and [WebAPI](), although it can certainly be used in other situations. It works with a `ClaimPrincipal` to allow developers to write logic to *authorize* actions within an application. 

## Main Idea

`ClaimsPrincipal` comes with a collection of claims associated with the user. This library determines if a Claim *exists* or *does not exist*. If a claim *exists* then a user is allowed to perform that action, otherwise they are not allowed to.

**Note: while you could create more complex logic in your abilities, try to distinguish between an ability and business rule. Business rules should be implemented in your application, and not as an `Ability`.** 

## Getting Started

> PS> Install-Package RimDev.Abilities

## Ability

```csharp
public abstract class Ability
{
    public virtual string Name => GetType().Name;

    public abstract Task<bool> ExecuteAsync(ClaimsPrincipal user, params IPolicyContext[] args);
}
```

An `Ability` has a property and a method: `Name` and `ExecuteAsync`. The `ExecuteAsync` method takes in a user and zero or many `IPolicyContext` as arguments. The result is a `true` or `false`.

**All policies are resolved via AutoFac, so they can accept any dependency registered in your application.**

## MVC

The MVC component comes with three attributes out of the box, with the ability to create many more attributes to decorate MVC actions: `Roles`, `Ability`, and `Custom`. 

**We recommend you use Claim, as it allows for more granular permission controls.**

Additionally, we have provided a *Global* attribute of `Authenticate` that will return a `401` status code and challenge the user for credentials. The attribute can also be used at the `Controller` and Action level.

```csharp
public class FilterConfig
{
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        // will insure authentication is required
        // across the application, you may also
        // decorate a Controller or Action     
        filters.Add(new AuthenticateAttribute());
    }
}
```

Abilities will return a `403` Forbidden response, which will allow you to handle authentication and authorization violations differently.

```csharp
// backward compatibility
[Roles("Administrator")]
public ActionResult Index() {
  return View();   
}

// recommended
[Ability("User Detail")]
public ActionResult Show(int id) {
  return View();   
}

// a complex ability
[Custom(typeof(ComplexAbility)]
public ActionResult Edit(int id) {
    return View();
}
```

## WebAPI

The WebAPI component comes with three attributes out of the box, with the ability to create many more attributes to decorate MVC actions: `Roles`, `Ability`, and `Custom`. 

**We recommend you use Claim, as it allows for more granular permission controls.**

Additionally, we have provided a *Global* attribute of `Authenticate` that will return a `401` status code and challenge the user for credentials. The attribute can also be used at the `Controller` and Action level.

Abilities will return a `403` Forbidden response, which will allow you to handle authentication and authorization violations differently.

```csharp
[Authenticate]
public class HomeController : ApiController
{
    [HttpGet]
    [AllowAnonymous]
    [Route("")]
    public IHttpActionResult Root()
    {
        return Ok("Welcome to the root path.");
    }

    [HttpGet]
    [Route("claim")]
    [Ability("Awesome")]
    public IHttpActionResult ClaimPolicy()
    {
        return Ok("/claim");
    }

    [HttpGet]
    [Route("role")]
    [Roles("Administrator")]
    public IHttpActionResult InRolePolicy()
    {
        return Ok("/role");
    }

    [HttpGet]
    [Route("custom")]
    [Custom(typeof(ComplexAbility))]
    public IHttpActionResult CustomPolicy()
    {
        return Ok("/custom");
    }
}
```

## Technology Dependencies

- [AutoFac](http://docs.autofac.org/en/latest/)
- [ASP.NET MVC](http://www.asp.net/mvc)
- [ASP.NET WebAPI](http://www.asp.net/web-api)

