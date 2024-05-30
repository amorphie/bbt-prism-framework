using System.Threading.Tasks;
using BBT.Prism.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BBT.Prism.Data.Seeding;

public sealed class DataSeeder(
    IOptions<PrismDataSeedOptions> options,
    IServiceScopeFactory serviceScopeFactory)
    : IDataSeeder
{
    private IServiceScopeFactory ServiceScopeFactory { get; } = serviceScopeFactory;
    private PrismDataSeedOptions Options { get; } = options.Value;

    public async Task SeedAsync(DataSeedContext context)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        if (context.Properties.ContainsKey(DataSeederExtensions.SeedInSeparateUow))
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            foreach (var contributorType in Options.Contributors)
            {
                using var transaction = await unitOfWork.BeginTransactionAsync();
                var contributor = (IDataSeedContributor)scope.ServiceProvider.GetRequiredService(contributorType);
                await contributor.SeedAsync(context);
                await transaction.CommitAsync();
            }
        }
        else
        {
            foreach (var contributorType in Options.Contributors)
            {
                var contributor = (IDataSeedContributor)scope.ServiceProvider.GetRequiredService(contributorType);
                await contributor.SeedAsync(context);
            }
        }
    }
}
