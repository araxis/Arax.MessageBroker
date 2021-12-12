using System.Reflection;
using MammutNg.MessageBrokers.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace MammutNg.MessageBrokers.Nats.MediatR
{
    public static class Extensions
    {
        public static void AddMediatRHandlers(this NatsBus bus, params Assembly[] assemblies)
        {
            bus.ServiceCollection.AddTransient(typeof(IMessageHandler<>), typeof(MessageHandler<>));
            bus.ServiceCollection.AddTransient(typeof(IEventHandler<>), typeof(EventHandler<>));
            bus.ServiceCollection.AddTransient(typeof(ICommandHandler<>), typeof(CommandHandler<>));
            bus.ServiceCollection.AddTransient(typeof(ICommandHandler<,>), typeof(CommandHandler<,>));
            bus.ServiceCollection.AddTransient(typeof(IQueryHandler<,>), typeof(QueryHandler<,>));

           // bus.ServiceCollection.RegisterMappers(assemblies);
        
         
        }

        //private static void RegisterMappers(this IServiceCollection services, params Assembly[] assemblies)
        //{
           
        //    services.RegisterCommandMappers(assemblies);
        //    services.RegisterEventMappers(assemblies);
        //    services.RegisterMessageMappers(assemblies);
        //    services.RegisterQueryMappers(assemblies);
           
        //}

        //private static void RegisterEventMappers(this IServiceCollection services, params Assembly[] assemblies)
        //{
        //    services.Scan(s => s.FromAssemblies(assemblies)
        //        .AddClasses(classes => 
        //            classes.AssignableTo(typeof(IEventMapper<>)))
        //        .AsImplementedInterfaces()
        //        .WithTransientLifetime());
           
        //}

        //private static void RegisterCommandMappers(this IServiceCollection services, params Assembly[] assemblies)
        //{
        //    services.Scan(s => s.FromAssemblies(assemblies)
        //        .AddClasses(classes =>
        //            classes.AssignableTo(typeof(ICommandMapper<>)))
        //        .AsImplementedInterfaces().WithTransientLifetime()
        //        .AddClasses(classes =>
        //            classes.AssignableTo(typeof(ICommandMapper<,>)))
        //        .AsImplementedInterfaces().WithTransientLifetime());
            
        //}

        //private static void RegisterQueryMappers(this IServiceCollection services, params Assembly[] assemblies)
        //{
        //    services.Scan(s => s.FromAssemblies(assemblies)
        //        .AddClasses(classes =>
        //            classes.AssignableTo(typeof(IQueryMapper<,>)))
        //        .AsImplementedInterfaces().WithTransientLifetime());
            
        //}

        //private static void RegisterMessageMappers(this IServiceCollection services, params Assembly[] assemblies)
        //{
        //   services.Scan(s => s.FromAssemblies(assemblies).AddClasses(classes =>
        //            classes.AssignableTo(typeof(IMessageMapper<>)))
        //        .AsImplementedInterfaces().WithTransientLifetime());
           
        //}
    }
}
