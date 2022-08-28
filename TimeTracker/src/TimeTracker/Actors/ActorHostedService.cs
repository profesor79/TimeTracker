using Akka.Actor;
using Akka.Configuration;
using Akka.DependencyInjection;

namespace TimeTracker.Actors
{
    public class ActorHostedService : IHostedService
    {


        protected ActorSystem ClusterSystem;

        private readonly IServiceProvider _serviceProvider;

        public ActorHostedService(IServiceProvider sp)
        {
            _serviceProvider = sp;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = HoconLoader.ParseConfig("crawler.hocon");
            var bootstrap = BootstrapSetup.Create()
               .WithConfig(config) // load HOCON and apply extension methods to inject environment variables
               .WithActorRefProvider(ProviderSelection.Cluster.Instance); // launch Akka.Cluster



            // enable DI support inside this ActorSystem, if needed
            var diSetup = ServiceProviderSetup.Create(_serviceProvider);

            // merge this setup (and any others) together into ActorSystemSetup
            var actorSystemSetup = bootstrap.And(diSetup);

            // start ActorSystem
            ClusterSystem = ActorSystem.Create("webcrawler", actorSystemSetup);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // strictly speaking this may not be necessary - terminating the ActorSystem would also work
            // but this call guarantees that the shutdown of the cluster is graceful regardless
            await CoordinatedShutdown.Get(ClusterSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }

    public static class HoconLoader
    {
        public static Akka.Configuration.Config ParseConfig(string hoconPath)
        {
            return ConfigurationFactory.ParseString(File.ReadAllText(hoconPath));
        }
    }
}

