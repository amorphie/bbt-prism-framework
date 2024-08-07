using System;
using System.Linq;

namespace BBT.Prism.Users;

public class CurrentUser(ICurrentUserAccessor currentUserAccessor) : ICurrentUser
{
    public bool IsAuthenticated => Id.HasValue;
    public Guid? Id => currentUserAccessor.Current?.Id;
    public string? UserName => currentUserAccessor.Current?.UserName;
    public string? Name => currentUserAccessor.Current?.Name;
    public string? SurName => currentUserAccessor.Current?.SurName;
    public string? Email => currentUserAccessor.Current?.Email;
    public string? Phone => currentUserAccessor.Current?.Phone;
    public string[]? Roles => currentUserAccessor.Current?.Roles;
    public Guid? ActorUserId => currentUserAccessor.Current?.ActorUserId; 
    public string? ActorUserName => currentUserAccessor.Current?.ActorUserName; 

    public bool IsInRole(string roleName)
    {
        return Roles?.Any(a => a == roleName) ?? false;
    }

    public IDisposable Change(
        Guid? id,
        string? userName = null,
        string? name = null,
        string? surname = null,
        string? email = null,
        string? phone = null,
        string[]? roles = null,
        Guid? actorUserId = null,
        string? actorUserName = null
    )
    {
        return SetCurrent(id, userName, name, surname, email, phone, roles, actorUserId, actorUserName);
    }

    private IDisposable SetCurrent(
        Guid? id,
        string? userName = null,
        string? name = null,
        string? surname = null,
        string? email = null,
        string? phone = null,
        string[]? roles = null,
        Guid? actorUserId = null,
        string? actorUserName = null
    )
    {
        var parentScope = currentUserAccessor.Current;
        currentUserAccessor.Current = new BasicUserInfo(id, userName, name, surname, email, phone, roles, actorUserId, actorUserName);
        return new DisposeAction(() => { currentUserAccessor.Current = parentScope; });
    }
}