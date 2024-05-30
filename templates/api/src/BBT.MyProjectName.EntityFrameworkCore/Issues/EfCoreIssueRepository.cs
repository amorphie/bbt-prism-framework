using System;
using System.Linq;
using System.Threading.Tasks;
using BBT.MyProjectName.EntityFrameworkCore;
using BBT.Prism.Domain.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BBT.MyProjectName.Issues;

public class EfCoreIssueRepository(
    MyProjectNameDbContext dbContext,
    IServiceProvider serviceProvider)
    : EfCoreRepository<MyProjectNameDbContext, Issue, Guid>(dbContext, serviceProvider), IIssueRepository
{
    public async override Task<IQueryable<Issue>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .Include(p => p.Comments)
            .Include(p => p.Labels);
    }
}