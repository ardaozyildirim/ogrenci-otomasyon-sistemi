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
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add API Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver")
    );
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Add Security Services
builder.Services.AddScoped<StudentManagementSystem.Infrastructure.Services.ISecurityService, StudentManagementSystem.Infrastructure.Services.SecurityService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Student Management System API", 
        Version = "v1",
        Description = "A comprehensive student management system API with authentication, authorization, and CRUD operations for students, teachers, courses, and grades.",
        Contact = new OpenApiContact
        {
            Name = "Student Management System Team",
            Email = "support@studentmanagement.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
    
    // Include XML comments for better documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    
    // Add JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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
    
    // Enable annotations
    c.EnableAnnotations();
    
    // Customize operation IDs
    c.CustomOperationIds(apiDesc =>
    {
        return apiDesc.ActionDescriptor.EndpointMetadata
            .OfType<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>()
            .FirstOrDefault()?.ActionName;
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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management System API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
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

// Make Program class accessible for testing
public partial class Program { }
