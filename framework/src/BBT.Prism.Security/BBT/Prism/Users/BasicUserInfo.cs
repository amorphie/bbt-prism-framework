using System;

namespace BBT.Prism.Users;

public class BasicUserInfo(
    Guid? id,
    string? userName = null,
    string? name = null,
    string? surname = null,
    string? email = null,
    string? phone = null,
    string[]? roles = null,
    Guid? actorUserId = null,
    string? actorUserName = null)
{
    public Guid? Id { get; set; } = id;
    public string? UserName { get; set; } = userName;
    public string? Name { get; set; } = name;
    public string? SurName { get; set; } = surname;
    public string? Email { get; set; } = email;
    public string? Phone { get; set; } = phone;
    public string[]? Roles { get; set; } = roles;
    public Guid? ActorUserId { get; set; } = actorUserId;
    public string? ActorUserName { get; set; } = actorUserName;
    
}