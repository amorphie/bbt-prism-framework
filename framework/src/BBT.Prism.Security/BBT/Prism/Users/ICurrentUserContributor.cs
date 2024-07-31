namespace BBT.Prism.Users;

public interface ICurrentUserContributor
{
    BasicUserInfo? GetCurrentUser();
}