using System.Threading;
using BBT.Prism.Threading;
using Microsoft.Data.Sqlite;

namespace BBT.Prism.EntityFrameworkCore.Sqlite;

public class PrismUnitTestSqliteConnection(string connectionString) : SqliteConnection(connectionString)
{
    public override SqliteCommand CreateCommand()
    {
        return new PrismSqliteCommand
        {
            Connection = this,
            CommandTimeout = DefaultTimeout,
            Transaction = Transaction
        };
    }
}

internal class PrismSqliteCommand : SqliteCommand
{
    private readonly static SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

    public override SqliteConnection? Connection
    {
        get => base.Connection;
        set
        {
            using (Semaphore.Lock())
            {
                base.Connection = value;
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        using (Semaphore.Lock())
        {
            base.Dispose(disposing);
        }
    }
}
