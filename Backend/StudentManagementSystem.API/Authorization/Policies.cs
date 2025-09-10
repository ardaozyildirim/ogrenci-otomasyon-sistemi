namespace StudentManagementSystem.API.Authorization;

public static class Policies
{
    public const string AdminOnly = "AdminOnly";
    public const string TeacherOrAdmin = "TeacherOrAdmin";
    public const string StudentOrAdmin = "StudentOrAdmin";
    public const string AllRoles = "AllRoles";
}
