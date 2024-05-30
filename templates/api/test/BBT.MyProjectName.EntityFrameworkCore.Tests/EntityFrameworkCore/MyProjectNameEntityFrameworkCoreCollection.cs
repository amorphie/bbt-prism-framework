using Xunit;

namespace BBT.MyProjectName.EntityFrameworkCore;

[CollectionDefinition(MyProjectNameTestConsts.CollectionDefinitionName)]
public class MyProjectNameEntityFrameworkCoreCollection: ICollectionFixture<MyProjectNameEntityFrameworkCoreFixture>
{
    
}