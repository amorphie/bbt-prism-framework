using System.Threading.Tasks;
using BBT.MyProjectName.Data;
using Microsoft.EntityFrameworkCore;

namespace BBT.MyProjectName.EntityFrameworkCore;

public class MyProjectNameDbSchemaMigrator(MyProjectNameDbContext dbContext) : IMyProjectNameDbSchemaMigrator
{
    public async Task MigrateAsync()
    {
        await dbContext
            .Database.MigrateAsync();
    }
}