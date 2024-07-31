namespace BBT.Prism.Users;

public interface ICurrentUserResolver
{
    BasicUserInfo? Resolve();
}