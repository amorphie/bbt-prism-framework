using System.Threading.Tasks;
using BBT.MyProjectName.Issues;
using BBT.Prism.Data.Seeding;
using BBT.Prism.Domain.Repositories;
using BBT.Prism.Guids;
using BBT.Prism.Uow;

namespace BBT.MyProjectName.Data;

public class MyProjectNameDataSeedContributor(
    IUnitOfWork unitOfWork,
    IGuidGenerator guidGenerator,
    IIssueRepository issueRepository,
    IGitRepository gitRepository)
    : IDataSeedContributor
{
    public async Task SeedAsync(DataSeedContext context)
    {
        const string repoName = "MyProjectName";
        var repository = await gitRepository.FirstOrDefaultAsync(p => p.Name == repoName);

        if (repository == null)
        {
            repository = new GitRepository(
                guidGenerator.Create(),
                repoName
            );
            await gitRepository.InsertAsync(repository);
            await unitOfWork.SaveChangesAsync();
        }

        if (!await issueRepository.AnyAsync())
        {
            var issue = new Issue(
                guidGenerator.Create(),
                repository.Id,
                "Cli solution renamer",
                "Cli solution does not change the project name in renamer.");

            await issueRepository.InsertAsync(issue);
            await unitOfWork.SaveChangesAsync();
        }
    }
}