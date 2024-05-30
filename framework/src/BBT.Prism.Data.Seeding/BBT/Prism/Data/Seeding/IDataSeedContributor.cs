using System.Threading.Tasks;

namespace BBT.Prism.Data.Seeding;

public interface IDataSeedContributor
{
    Task SeedAsync(DataSeedContext context);
}