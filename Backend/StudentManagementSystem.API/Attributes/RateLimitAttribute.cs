using Microsoft.AspNetCore.RateLimiting;

namespace StudentManagementSystem.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthRateLimitAttribute : Attribute
{
    public string PolicyName => "AuthPolicy";
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiRateLimitAttribute : Attribute
{
    public string PolicyName => "ApiPolicy";
}
