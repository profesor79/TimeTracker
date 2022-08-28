using System;
using System.Linq;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Routing;

namespace profesor79.Common.ActorSystem
{
    public class ChildActorCreator : IChildActorCreator
    {
        public IActorRef Create<TActor>(IActorContext context, string name) where TActor : ActorBase
        {
            return context.ActorOf(GetProps<TActor>(context), name);
        }

        public IActorRef Create<TActor>(IActorContext context) where TActor : ActorBase
        {
            var name = typeof(TActor).ToString().Split(".").Last() + (DateTime.Now.Ticks.ToString()).Replace("]", string.Empty).Replace("[", string.Empty);
            Console.WriteLine(name);

            return Create<TActor>(context, $"{name}");
        }

        public IActorRef CreateWithRobinRouter<TActor>(IActorContext context, int numberOfInstances = 10)
          where TActor : ActorBase
        {
            return context.ActorOf(context.DI().Props<TActor>().WithRouter(new RoundRobinPool(numberOfInstances)));
        }
        public IActorRef CreateRemotely<TActor>(IActorContext context, RemoteScope remoteScope)
          where TActor : ActorBase
        {
            return context.ActorOf(context.DI().Props<TActor>().WithDeploy(Deploy.None.WithScope(remoteScope)));
        }

        public Props GetProps<TActor>(IActorContext context) where TActor : ActorBase
        {
            return context.DI().Props<TActor>();
        }
    }
}