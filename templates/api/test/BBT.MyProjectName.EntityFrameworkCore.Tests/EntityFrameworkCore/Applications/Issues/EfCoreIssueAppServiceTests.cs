using BBT.MyProjectName.Issues;
using Xunit;

namespace BBT.MyProjectName.EntityFrameworkCore.Applications.Issues;

[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]
public class EfCoreIssueAppServiceTests : IssueAppServiceTests<MyProjectNameEntityFrameworkCoreTestModule>
{
    
}