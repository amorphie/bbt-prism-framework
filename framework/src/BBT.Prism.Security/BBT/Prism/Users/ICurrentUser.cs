using System;

namespace BBT.Prism.Users;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    Guid? Id { get; }
    string? UserName { get; }
    string? Name { get; }
    string? SurName { get; }
    public string? Email { get; }
    public string? Phone { get; }
    string[]? Roles { get; }
    bool IsInRole(string roleName);

    IDisposable Change(
        Guid? id,
        string? userName = null,
        string? name = null,
        string? surname = null,
        string? email = null,
        string? phone = null,
        string[]? roles = null);
}