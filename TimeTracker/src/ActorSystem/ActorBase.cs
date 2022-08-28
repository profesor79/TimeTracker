using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Akka.Actor;
using Akka.Configuration;
using Autofac.Builder;
using Autofac.Core;
using Akka.DI.Core;
using Serilog;

namespace dictionaryService.ActorSystem
{
    /// <summary>The program.</summary>
    public class ActorBase
    {
        private const string SystemName = "dictionary-service-actor-system";

        private static Config GetConfig()
        {
            var hostname = Dns.GetHostName();
            Log.Information("hostname", hostname);

            var config = ConfigurationFactory.ParseString(
              @"
akka {
    log-config-on-start = on
    log-dead-letters = on
    loglevel=DEBUG,
    loggers=[""Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog""]}
    actor {
        throughput = 1
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
        }
    remote {
        dot-netty.tcp {
            port = 0
            hostname = localhost
            public-hostname = __publicHostName__
              # Sets the send buffer size of the Sockets,
              # set to 0b for platform default
              send-buffer-size = 33554432b
              # Sets the receive buffer size of the Sockets,
              # set to 0b for platform default
              receive-buffer-size = 33554432b
              # Maximum message size the transport will accept, but at least
              # 32000 bytes.
              # Please note that UDP does not support arbitrary large datagrams,
              # so this setting has to be chosen carefully when using UDP.
              # Both send-buffer-size and receive-buffer-size settings has to
              # be adjusted to be able to buffer messages of maximum size.
              maximum-frame-size = 16777216b
        }
    }
}
                    "
                .Replace("__publicHostName__", SystemName)
              );

            return config;
        }

        /// <summary>The main.</summary>
        /// <param name="args">The args.</param>
        private static void StartSystem(string[] args)
        {


            var config = GetConfig();
            var system = Akka.Actor.ActorSystem.Create(SystemName, config);

            // Create and build your container
            var builder = DependencyContainer.ContainerBuilder();
            var container = builder.Build();

            system.UseAutofac(container);
            system.WhenTerminated.Wait();
        }
    }


}

