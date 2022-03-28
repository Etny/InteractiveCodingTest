global using System;
using System.Linq;
using DynamicCheck;
using DynamicCheck.IO;
using DynamicCheck.Security;
using DynamicCheck.Testing;
using DynamicCheck.Tracking;
using DynamicCheck.Validation;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
                    .AddSingleton<IStageProvider>(new JsonStageProvider("Tests"))
                    .AddScoped<TestingLifeCycle>()
                    .AddScoped<TrackingManager>()
                    .AddScoped<IResultWriter, FileResultWriter>()
                    .AddScoped<ITestValidator, DefaultTestValidator>()
                    // .AddScoped<ITestValidator, ProgressiveTestValidator>()
                    .AddScoped<IPasswordProvider, ConsolePasswordProvider>()
                    .AddScoped<MessageWriter>()
                    .AddScoped<TestRunner>();

bool secured = args.Contains("-p") || args.Contains("-v");

if(secured) {
    services.AddScoped<PasswordResultSigner>();
    services.AddScoped<PasswordResultValidator>();
}

using var prov = services.BuildServiceProvider();
using var scope = prov.CreateScope();

if(args.Contains("-v")) {
    scope.ServiceProvider.GetRequiredService<IPasswordProvider>().ObtainPassword();
    scope.ServiceProvider.GetRequiredService<PasswordResultValidator>().ShowValidation();
    return;
}


if(secured)
    scope.ServiceProvider.GetRequiredService<IPasswordProvider>().ObtainPassword();

var init = scope.ServiceProvider.GetRequiredService<TestRunner>();
init.Run();

if(secured)
    scope.ServiceProvider.GetRequiredService<PasswordResultSigner>().SignResult();





