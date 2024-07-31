using System.Threading;

namespace BBT.Prism.Users;

public class AsyncLocalCurrentUserAccessor : ICurrentUserAccessor
{
    public static AsyncLocalCurrentUserAccessor Instance { get; } = new();

    public BasicUserInfo? Current {
        get => _currentScope.Value;
        set => _currentScope.Value = value;
    }

    private readonly AsyncLocal<BasicUserInfo?> _currentScope;

    private AsyncLocalCurrentUserAccessor()
    {
        _currentScope = new AsyncLocal<BasicUserInfo?>();
    }
}