using System;
using System.Diagnostics;

namespace BBT.Prism.Users;

public static class CurrentUserExtensions
{
    public static Guid GetId(this ICurrentUser currentUser)
    {
        Debug.Assert(currentUser.Id != null, "currentUser.Id != null");

        return currentUser!.Id!.Value;
    }
}