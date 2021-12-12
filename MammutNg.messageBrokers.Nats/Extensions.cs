using System;
using System.Collections.Generic;
using System.Reflection;
using MammutNg.MessageBrokers.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using STAN.Client;

namespace MammutNg.MessageBrokers.Nats
{
    public static class Extensions
    {

 

        public static IMessageProperties Map<T>(this MessageContext<T> context) where T : class
        {
            return new MessageProperties
            {
                CorrelationId = context.CorrelationId,
                MessageId = context.MessageId,
                Timestamp = context.Timestamp,
                Headers = new Dictionary<string, object>(context.Headers)
            };
        }


        public static NatsBus AddNats(this IServiceCollection services,string clientId,string natsSectionName="nats")
        {
            var provider = services.BuildServiceProvider();
            var natsOption = provider.GetOptions<NatsOption>(natsSectionName);
          //  var appOption = provider.GetService<IServiceId>();

            var cf = new StanConnectionFactory();
            var option = StanOptions.GetDefaultOptions();
            option.NatsURL = natsOption.Url;
          //  var id = clientId ??$"{appOption.Id}_{Guid.NewGuid():N}" ;
            var connection = cf.CreateConnection(natsOption.ClusterId, clientId, option);
            
            services.AddSingleton(natsOption);
            services.AddSingleton(connection);
            services.AddTransient<INatsMessageSerializer, MessageSerializer>();
            services.AddScoped<IMessagePropertiesAccessor, MessagePropertiesAccessor>();

            services.AddSingleton<IMessageDispatcher,MessageDispatcher>();
            services.AddSingleton<IAdvancedMessageDispatcher,MessageDispatcher>();

            services.AddSingleton<IEventDispatcher,EventDispatcher>();
            services.AddSingleton<IAdvancedEventDispatcher,EventDispatcher>();

            services.AddSingleton<ICommandDispatcher,CommandDispatcher>();
            services.AddSingleton<IAdvancedCommandDispatcher,CommandDispatcher>();

            services.AddSingleton<IQueryDispatcher,QueryDispatcher>();
            services.AddSingleton<IAdvancedQueryDispatcher,QueryDispatcher>();

            services.AddSingleton<IBusDispatcher, BusDispatcher>();
            services.AddSingleton<INatsBusDispatcher, BusDispatcher>();

            services.AddSingleton<IMessageSubscriber, MessageSubscriber>();

            services.AddSingleton<IEventSubscriber, EventSubscriber>();
            services.AddSingleton<IAdvancedEventSubscriber, EventSubscriber>();

            services.AddSingleton<ICommandSubscriber, CommandSubscriber>();
            services.AddSingleton<IAdvancedCommandSubscriber, CommandSubscriber>();

            services.AddSingleton<IQuerySubscriber, QuerySubscriber>();

            services.AddSingleton<IBusSubscriber, BusSubscriber>();
            services.AddSingleton<INatsBusSubscriber, BusSubscriber>();
           // services.AddSingleton<IFaultSubscriber, BusSubscriber>();

            return new NatsBus(services);
        }

        public static void AddHandlers(this NatsBus natsBus, params Assembly[] assemblies)
        {
            natsBus.ServiceCollection.Scan(s => s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>))).AsImplementedInterfaces().WithTransientLifetime()
                .AddClasses(c => c.AssignableTo(typeof(IMessageHandler<>))).AsImplementedInterfaces().WithTransientLifetime()
                .AddClasses(c=>c.AssignableTo(typeof(ICommandHandler<,>))).AsImplementedInterfaces().WithTransientLifetime()
                .AddClasses(c=>c.AssignableTo(typeof(IQueryHandler<,>))).AsImplementedInterfaces().WithTransientLifetime()
                .AddClasses(c=>c.AssignableTo(typeof(IEventHandler<>))).AsImplementedInterfaces().WithTransientLifetime());
        }

        public static IApplicationBuilder UseNats(this IApplicationBuilder app,Action<IBusSubscriber> config)
        {
            var subscriber=  app.ApplicationServices.GetService<IBusSubscriber>();
            config(subscriber);
            return app;
        }

        public static IApplicationBuilder UseNats(this IApplicationBuilder app,Action<INatsBusSubscriber> config)
        {
            var subscriber=  app.ApplicationServices.GetService<INatsBusSubscriber>();
            config(subscriber);
            return app;
        }

        public static IBusSubscriber UseAdvancedNats(this IApplicationBuilder app)
        {
            return app.ApplicationServices.GetService<INatsBusSubscriber>();
           
        }

        public static IBusSubscriber UseNats(this IApplicationBuilder app)
        {
            return app.ApplicationServices.GetService<IBusSubscriber>();
           
        }

        public static IHostBuilder UseNats(this IHostBuilder builder,Action<IBusSubscriber> config)
        {
            builder.ConfigureServices((context, services) =>
            {
                var provider = services.BuildServiceProvider();
                var subscriber=  provider.GetService<IBusSubscriber>();
                config(subscriber);
            });
           
            return builder;
        }

        public static IHostBuilder UseNats(this IHostBuilder builder,Action<INatsBusSubscriber> config)
        {
            builder.ConfigureServices((context, services) =>
            {
                var provider = services.BuildServiceProvider();
                var subscriber=  provider.GetService<INatsBusSubscriber>();
                config(subscriber);
            });
           
            return builder;
        }
    }
}
