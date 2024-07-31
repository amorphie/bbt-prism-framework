using BBT.Prism.Collections;

namespace BBT.Prism.Users;

public class CurrentUserContributorOptions
{
    public ITypeList<ICurrentUserContributor> Contributors { get; } = new TypeList<ICurrentUserContributor>();
}