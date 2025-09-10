using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Interfaces;
using StudentManagementSystem.Application.Mappings;
using StudentManagementSystem.Infrastructure.Data;
using StudentManagementSystem.Infrastructure;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentManagementSystem.API.Authorization;
using StudentManagementSystem.API.Middleware;
using StudentManagementSystem.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Security Services
builder.Services.AddScoped<StudentManagementSystem.Infrastructure.Services.ISecurityService, StudentManagementSystem.Infrastructure.Services.SecurityService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Management System API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add Infrastructure Layer
builder.Services.AddInfrastructure(builder.Configuration);

// Override DbContext with In-Memory for testing
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(StudentManagementSystem.Application.AssemblyReference).Assembly));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(StudentManagementSystem.Application.AssemblyReference).Assembly));

       builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = builder.Configuration["Jwt:Issuer"],
                   ValidAudience = builder.Configuration["Jwt:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
               };
           });

       // Authorization Policies
       builder.Services.AddAuthorization(options =>
       {
           options.AddPolicy(Policies.AdminOnly, policy =>
               policy.Requirements.Add(new AdminOnlyRequirement()));
           
           options.AddPolicy(Policies.TeacherOrAdmin, policy =>
               policy.Requirements.Add(new TeacherOrAdminRequirement()));
           
           options.AddPolicy(Policies.StudentOrAdmin, policy =>
               policy.Requirements.Add(new StudentOrAdminRequirement()));
       });

       builder.Services.AddScoped<IAuthorizationHandler, AdminOnlyHandler>();
       builder.Services.AddScoped<IAuthorizationHandler, TeacherOrAdminHandler>();
       builder.Services.AddScoped<IAuthorizationHandler, StudentOrAdminHandler>();

// Add CORS Configuration
builder.Services.AddCorsConfiguration(builder.Configuration);

// Add Rate Limiting
builder.Services.AddRateLimitingConfiguration();

var app = builder.Build();

// Add Global Exception Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS Configuration
app.UseCorsConfiguration(app.Environment);

// Use Rate Limiting
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
