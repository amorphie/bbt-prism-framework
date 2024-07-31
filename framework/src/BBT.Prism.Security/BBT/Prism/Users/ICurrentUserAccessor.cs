namespace BBT.Prism.Users;

public interface ICurrentUserAccessor
{
    BasicUserInfo? Current { get; set; }
}