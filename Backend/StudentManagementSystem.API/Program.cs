using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManagementSystem.Application;
using StudentManagementSystem.Infrastructure;
using StudentManagementSystem.Infrastructure.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add Application and Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add API services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Student Management System API", Version = "v1" });
    
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "YourVeryLongSecretKeyThatIsAtLeast256BitsLong123456789";
var issuer = jwtSettings["Issuer"] ?? "StudentManagementSystem";
var audience = jwtSettings["Audience"] ?? "StudentManagementSystem";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management System API V1");
        c.RoutePrefix = string.Empty; // Makes Swagger UI available at root
    });
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

// Use Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Apply database migrations automatically in production
if (app.Environment.IsProduction())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try
        {
            context.Database.Migrate();
            app.Logger.LogInformation("Database migrations applied successfully.");
            
            // Seed initial data if needed
            await SeedData(context, app.Logger);
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while applying database migrations.");
        }
    }
}

app.Run();

// Seed data method
async Task SeedData(ApplicationDbContext context, ILogger logger)
{
    try
    {
        // Add missing Status column if it doesn't exist
        await context.Database.ExecuteSqlRawAsync(@"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                               WHERE table_name = 'Courses' AND column_name = 'Status') THEN
                    ALTER TABLE ""Courses"" ADD COLUMN ""Status"" integer NOT NULL DEFAULT 1;
                END IF;
            END $$;
        ");
        
        logger.LogInformation("Database schema updated.");
        
        // Check if we already have teachers
        if (!await context.Teachers.AnyAsync())
        {
            // Create a sample teacher
            var teacher = new StudentManagementSystem.Domain.Entities.Teacher
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@university.com",
                EmployeeId = "EMP001",
                Department = "Computer Science",
                Specialty = "Software Engineering",
                PhoneNumber = "123-456-7890",
                HireDate = new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            
            context.Teachers.Add(teacher);
            logger.LogInformation("Sample teacher created.");
        }
        
        // Check if we already have students
        if (!await context.Students.AnyAsync())
        {
            // Create a sample student
            var student = new StudentManagementSystem.Domain.Entities.Student
            {
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@student.com",
                StudentNumber = "STU001",
                DateOfBirth = new DateTime(2000, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                PhoneNumber = "987-654-3210",
                Address = "123 Student St",
                EnrollmentDate = new DateTime(2023, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            
            context.Students.Add(student);
            logger.LogInformation("Sample student created.");
        }
        await context.SaveChangesAsync();
        logger.LogInformation("Seed data created successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error occurred while seeding data.");
    }
}
