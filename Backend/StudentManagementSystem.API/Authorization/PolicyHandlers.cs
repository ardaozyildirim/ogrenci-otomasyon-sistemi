using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StudentManagementSystem.API.Authorization;

public class AdminOnlyRequirement : IAuthorizationRequirement { }

public class AdminOnlyHandler : AuthorizationHandler<AdminOnlyRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminOnlyRequirement requirement)
    {
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

public class TeacherOrAdminRequirement : IAuthorizationRequirement { }

public class TeacherOrAdminHandler : AuthorizationHandler<TeacherOrAdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TeacherOrAdminRequirement requirement)
    {
        if (context.User.IsInRole("Admin") || context.User.IsInRole("Teacher"))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

public class StudentOrAdminRequirement : IAuthorizationRequirement { }

public class StudentOrAdminHandler : AuthorizationHandler<StudentOrAdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StudentOrAdminRequirement requirement)
    {
        if (context.User.IsInRole("Admin") || context.User.IsInRole("Student"))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
