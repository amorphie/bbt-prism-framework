using System.Threading.Tasks;

namespace BBT.Prism.Data.Seeding;

public interface IDataSeeder
{
    Task SeedAsync(DataSeedContext context);
}