using System.Diagnostics;
using Akka.Actor;
using Akka.Cluster.Hosting;
using Akka.Hosting;
using Akka.Remote.Hosting;
using TimeTracker;
using TimeTracker.Actors.TimeSignature;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json");

builder.Logging.ClearProviders().AddConsole();

var akkaConfig = builder.Configuration.GetRequiredSection(nameof(AkkaClusterConfig))
    .Get<AkkaClusterConfig>();

builder.Services.AddControllers();
builder.Services.AddAkka(akkaConfig.ActorSystemName, (builder, provider) =>
{
    Debug.Assert(akkaConfig.Port != null, "akkaConfig.Port != null");
    builder.AddHoconFile("app.conf")
        .WithRemoting(akkaConfig.Hostname, akkaConfig.Port.Value)
        .WithClustering(new ClusterOptions()
        {
            Roles = akkaConfig.Roles?.ToArray() ?? Array.Empty<string>(),
            SeedNodes = akkaConfig.SeedNodes?.Select(Address.Parse).ToArray() ?? Array.Empty<Address>()
        })
        .WithActors((system, registry) =>
        {
            ActorSystemRefernce.System = system;
            var consoleActor = system.ActorOf(Props.Create(() => new ConsoleActor()), "console");
            registry.Register<ConsoleActor>(consoleActor);

        });
});

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapGet("/", async (HttpContext context, ActorRegistry registry) =>
    {
        var reporter = registry.Get<ConsoleActor>();
        var resp = await reporter.Ask<string>($"hit from {context.TraceIdentifier}", context.RequestAborted); // calls Akka.NET under the covers
        await context.Response.WriteAsync(resp);
    });
});


await app.RunAsync();