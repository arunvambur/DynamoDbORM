using Autofac;
using MediatR;
using Microsoft.Extensions.Configuration;
using DynamoDbORM.Persistence.DynamoDb;
using System.Reflection;
using DynamoDb;

namespace DynamoDbORM.Persistence
{   
    public class PersistenceServiceRegistration : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public PersistenceServiceRegistration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            var dynamoDbSettings = _configuration.GetSection("DynamoDbSettings").Get<DynamoDbSettings>();
            builder.RegisterGeneric(typeof(DynamoDbContext<>));
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                  .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterGeneric(typeof(CommonRepository<>)).As(typeof(ICommonRepository<>));
             builder.RegisterInstance<DynamoDbSettings>(dynamoDbSettings)
                .SingleInstance();
        }

    }
}
