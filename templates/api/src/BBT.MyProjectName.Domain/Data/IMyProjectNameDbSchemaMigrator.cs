using System.Threading.Tasks;

namespace BBT.MyProjectName.Data;

public interface IMyProjectNameDbSchemaMigrator
{
    Task MigrateAsync();
}