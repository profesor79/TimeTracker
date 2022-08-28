using System.Reflection;
using Autofac;
using profesor79.Common.ActorSystem;

namespace dictionaryService.ActorSystem
{
    public static class DependencyContainer
    {
        /// <summary>The assign actors.</summary>
        /// <param name="builder">The builder.</param>
        private static void AssignActors(ContainerBuilder builder)
        {
            //  builder.RegisterType<RootActor>().AsSelf();

            var assembly = Assembly.GetExecutingAssembly();


            builder.RegisterAssemblyTypes(assembly)
              .Where(t => t.Name.EndsWith("Actor"))
              .AsSelf();
        }

        /// <summary>The assign storage.</summary>
        /// <param name="builder">The builder.</param>
        private static void AssignStorage(ContainerBuilder builder)
        {
            // builder.RegisterType<>().As<>();
            // builder.RegisterType<>().As<>();
            // builder.RegisterType<>().As<>();
        }

        /// <summary>The container builder.</summary>
        /// <returns>The <see cref="ContainerBuilder()" />.</returns>
        public static ContainerBuilder ContainerBuilder()
        {
            var builder = new ContainerBuilder();


            builder.RegisterType<ChildActorCreator>().As<IChildActorCreator>();
            AssignActors(builder);
            AssignStorage(builder);

            // builder.RegisterType<>().AsSelf();
            return builder;
        }
    }


}

