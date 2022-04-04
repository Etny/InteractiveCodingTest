global using System;
using System.Linq;
using DynamicCheck;
using DynamicCheck.IO;
using DynamicCheck.Security;
using DynamicCheck.Testing;
using DynamicCheck.Tracking;
using DynamicCheck.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

await Host.CreateDefaultBuilder(args)
            .ConfigureServices(services => {
                services.AddHostedService<TestService>()
                    .AddSingleton<IStageProvider>(new JsonStageProvider("Tests"))
                    .AddScoped<TestingLifeCycle>()
                    // .AddScoped<TimeTracker>()
                    .AddScoped<StageTracker>()
                    .AddScoped<ProgressTracker>()
                    .AddTransient<IResultWriter, FileResultWriter>()
                    .AddScoped<ITestValidator, DefaultTestValidator>()
                    .AddScoped<ITestValidator, ProgressiveTestValidator>()
                    // .AddScoped<IPasswordProvider, ConsolePasswordProvider>()
                    .AddScoped<PasswordResultSigner>()
                    .AddScoped<PasswordResultValidator>()
                    .AddScoped<MessageWriter>()
                    .AddScoped<TestRunner>()
                    .AddLogging(logging => {
                        logging.ClearProviders();
                        logging.AddFile("test.log");
                        logging.SetMinimumLevel(LogLevel.Debug);
                    });
            })
            .RunConsoleAsync();
                    
// bool secured = args.Contains("-p") || args.Contains("-v");

// if(secured) {
//     services.AddScoped<PasswordResultSigner>();
//     services.AddScoped<PasswordResultValidator>();
// }

// using var prov = services.BuildServiceProvider();
// using var scope = prov.CreateScope();

// if(args.Contains("-v")) {
//     scope.ServiceProvider.GetRequiredService<IPasswordProvider>().ObtainPassword();
//     scope.ServiceProvider.GetRequiredService<PasswordResultValidator>().ShowValidation();
//     return;
// }


// if(secured)
//     scope.ServiceProvider.GetRequiredService<IPasswordProvider>().ObtainPassword();

// var init = scope.ServiceProvider.GetRequiredService<TestRunner>();
// init.Run();

// if(secured)
//     scope.ServiceProvider.GetRequiredService<PasswordResultSigner>().SignResult();





