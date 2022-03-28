global using System;

using DynamicCheck;
using DynamicCheck.IO;
using DynamicCheck.Testing;
using DynamicCheck.Tracking;
using Microsoft.Extensions.DependencyInjection;

using var prov = new ServiceCollection()
                    .AddSingleton<IStageProvider>(new JsonStageProvider("./Tests.json"))
                    .AddScoped<TestingLifeCycle>()
                    .AddScoped<TrackingManager>()
                    .AddScoped<IResultWriter, FileResultWriter>()
                    .AddScoped<MessageWriter>()
                    .AddScoped<TestRunner>()
                    .BuildServiceProvider();

using(var scope = prov.CreateScope()) {
    var init = scope.ServiceProvider.GetRequiredService<TestRunner>();
    init.Run();
}




