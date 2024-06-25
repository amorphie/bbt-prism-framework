# Modularity

## Introduction

BBT.Prism SDK provides support for a modular architecture inspired by ABP.io. This guide outlines how to implement and configure modules within your application using BBT.Prism.

## Module Class

Every module needs to define a module class. To do this, create a class that inherits from `PrismModule`:

```csharp
public class AppModule : PrismModule
{
}
```

### Configuring Dependency Injection & Other Modules

#### ConfigureServices Method

`ConfigureServices` is the main method for adding your services to the dependency injection system and configuring other modules. Example:

> Asynchronous versions of these methods are available. To make asynchronous calls within these methods, override the asynchronous versions instead of the synchronous ones.

```csharp
public class AppModule : PrismModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //...
    }
}
```

#### Pre-Configure Services

The `PrismModule` class also provides the `PreConfigureServices` method, which can be overridden to execute code just before and after `ConfigureServices`. Code in these methods will run before/after the `ConfigureServices` methods of all other modules.

### Application Initialization

After configuring the services of all modules, the application initializes all modules. At this point, you can resolve services from `IServiceProvider` since it is ready and available.

#### OnApplicationInitialization Method

Override the `OnApplicationInitialization` method to execute code during application startup.

**Example:**

```csharp
public class AppModule : PrismModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var myService = context.ServiceProvider.GetService<MyService>();
        myService.DoSomething();
    }
}
```

An asynchronous version of the `OnApplicationInitialization` method is also available. If you need to make asynchronous calls within this method, override the asynchronous version.

**Example:**

```csharp
public class AppModule : PrismModule
{
    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var myService = context.ServiceProvider.GetService<MyService>();
        await myService.DoSomethingAsync();
    }
}
```

> When both asynchronous and synchronous versions are overridden, only the asynchronous version will be executed.

Typically, `OnApplicationInitialization` is used by the startup module to build the middleware pipeline for ASP.NET Core applications.

**Example:**

```csharp
[Modules(typeof(PrismAspNetCoreMvcModule))]
public class AppModule : PrismModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
    }
}
```

Startup logic specific to your module can also be included.

#### Post Application Initialization

The `PrismModule` class also defines the `OnPostApplicationInitialization` method, which can be overridden to execute code just before and just after `OnApplicationInitialization`. Code in these methods will be executed before/after the `OnApplicationInitialization` methods of all other modules.

> Asynchronous versions of these methods are also available. If you need to make asynchronous calls within these methods, override the asynchronous versions instead of the synchronous ones.

### Application Shutdown

To execute some code during application shutdown, override the `OnApplicationShutdown` method.

> An asynchronous version of this method is also available. If you need to make asynchronous calls within this method, override the asynchronous version instead of the synchronous one.

## Module Dependencies

In a modular application, one module often depends on other modules. If a Prism module depends on another module, it must declare a `[Modules]` attribute, as shown below:

```csharp
[Modules(typeof(AppAOtherModule), typeof(AppBOtherModule))]
public class AppModule : PrismModule
{
    //...
}
```

You can use multiple `Modules` attributes or pass multiple module types to a single `Modules` attribute, based on your preference.

A dependent module might rely on another module, but you only need to define your direct dependencies. Prism analyzes the dependency graph during application startup and initializes/shuts down modules in the correct order.