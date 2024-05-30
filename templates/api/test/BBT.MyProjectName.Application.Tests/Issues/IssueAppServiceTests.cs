using BBT.Prism.Modularity;

namespace BBT.MyProjectName.Issues;

public abstract class IssueAppServiceTests<TStartupModule> : MyProjectNameApplicationTestBase<TStartupModule>
    where TStartupModule : IPrismModule
{
    private readonly IIssueAppService _issueAppService;

    public IssueAppServiceTests()
    {
        _issueAppService = GetRequiredService<IIssueAppService>();
    }
}