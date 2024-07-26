using Saas.Repository.Interfaces;
using Saas.Repository.Concretes;
using Saas.Api.Services;
using Saas.Repository.Services;

namespace Saas.Api.Extensions
{
    public static class IocContainer
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISecretRepository, SecretRepository>();
            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<ICollectionSecretRepository, CollectionSecretRepository>();
            services.AddScoped<IOneTimeShareRepository, OneTimeShareRepository>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISecretService, SecretService>();
        }
    }
}