using System;

namespace BBT.Prism.AspNetCore.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class IgnorePrismSecurityHeaderAttribute: Attribute
{
    
}