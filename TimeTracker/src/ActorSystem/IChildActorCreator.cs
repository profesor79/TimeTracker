using Akka.Actor;

namespace profesor79.Common.ActorSystem
{
    public interface IChildActorCreator
    {
        IActorRef Create<TActor>(IActorContext context) where TActor : ActorBase;
        IActorRef Create<TActor>(IActorContext context, string name) where TActor : ActorBase;
        IActorRef CreateRemotely<TActor>(IActorContext context, RemoteScope remoteScope) where TActor : ActorBase;
        IActorRef CreateWithRobinRouter<TActor>(IActorContext context, int numberOfInstances = 10) where TActor : ActorBase;
        Props GetProps<TActor>(IActorContext context) where TActor : ActorBase;
    }
}