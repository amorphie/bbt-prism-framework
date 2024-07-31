using System.Security.Claims;

namespace BBT.Prism.Security.Claims;

public static class PrismClaimTypes
{
    /// <summary>
    /// Default: sub
    /// Identity No
    /// </summary>
    public static string UserName { get; set; } = "sub";
    
    /// <summary>
    /// Default: uppercase_name
    /// </summary>
    public static string Name { get; set; } = "uppercase_name";

    /// <summary>
    /// Default: uppercase_surname
    /// </summary>
    public static string SurName { get; set; } = "uppercase_surname";

    /// <summary>
    /// Default: userid
    /// </summary>
    public static string UserId { get; set; } = "userid";

    /// <summary>
    /// Default: role
    /// </summary>
    public static string Role { get; set; } = "role";

    /// <summary>
    /// Default: email
    /// </summary>
    public static string Email { get; set; } = "email";
    
    /// <summary>
    /// Default: phone_number
    /// </summary>
    public static string Phone { get; set; } = "phone_number";

    /// <summary>
    /// Default: act
    /// Delegation
    /// </summary>
    public static string Actor { get; set; } = "act";
    
    /// <summary>
    /// Default: "client_id".
    /// </summary>
    public static string ClientId { get; set; } = "client_id";
}