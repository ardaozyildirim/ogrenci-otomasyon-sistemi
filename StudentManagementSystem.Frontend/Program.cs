using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using StudentManagementSystem.Frontend.Services.Api;
using StudentManagementSystem.Frontend.Services.Auth;
using Blazored.LocalStorage;
using Blazored.Toast;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add Blazored services
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add HTTP Client
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Add Authentication
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

// Add API Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<GradeService>();
builder.Services.AddScoped<AttendanceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
