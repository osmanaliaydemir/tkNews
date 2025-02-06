using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using tkNews.Application.Common.Interfaces;
using tkNews.Application.Interfaces;
using tkNews.Application.Services;
using tkNews.Application.Services.Implementations;
using tkNews.Domain.Entities.Identity;
using tkNews.Domain.Enums;
using tkNews.Infrastructure.Authorization;
using tkNews.Infrastructure.Data;
using tkNews.Infrastructure.Data.Identity;
using tkNews.Infrastructure.Repositories;
using tkNews.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity DbContext
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    
    // User settings
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})
.AddEntityFrameworkStores<ApplicationIdentityDbContext>()
.AddDefaultTokenProviders();

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"] ?? throw new ArgumentNullException("JWT:Issuer configuration is missing"),
        ValidAudience = builder.Configuration["JWT:Audience"] ?? throw new ArgumentNullException("JWT:Audience configuration is missing"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["JWT:Key"] ?? throw new ArgumentNullException("JWT:Key configuration is missing"))),
        ClockSkew = TimeSpan.Zero
    };
});

// Add Token Service
builder.Services.AddScoped<ITokenService, TokenService>();

// Add Repositories and UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Services
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ITagService, TagService>();

// Add Permission Service
builder.Services.AddScoped<IPermissionService, PermissionService>();

// Add Authorization Handlers
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

// Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
    // Article Policies
    options.AddPolicy("ViewArticles", policy => 
        policy.Requirements.Add(new PermissionRequirement(Permissions.ViewArticles)));
    options.AddPolicy("CreateArticle", policy => 
        policy.Requirements.Add(new PermissionRequirement(Permissions.CreateArticle)));
    options.AddPolicy("EditArticle", policy => 
        policy.Requirements.Add(new PermissionRequirement(Permissions.EditArticle)));
    options.AddPolicy("DeleteArticle", policy => 
        policy.Requirements.Add(new PermissionRequirement(Permissions.DeleteArticle)));
    options.AddPolicy("PublishArticle", policy => 
        policy.Requirements.Add(new PermissionRequirement(Permissions.PublishArticle)));
        
    // Category Policies
    options.AddPolicy("ManageCategories", policy => 
        policy.Requirements.Add(new PermissionRequirement(Permissions.ManageCategories)));
        
    // Comment Policies
    options.AddPolicy("ModerateComments", policy => 
        policy.Requirements.Add(new PermissionRequirement(Permissions.ModerateComments)));
        
    // User Management Policies
    options.AddPolicy("ManageUsers", policy => 
        policy.Requirements.Add(new PermissionRequirement(Permissions.ManageUsers)));
    options.AddPolicy("ManageRoles", policy => 
        policy.Requirements.Add(new PermissionRequirement(Permissions.ManageRoles)));
        
    // System Policies
    options.AddPolicy("ManageSettings", policy => 
        policy.Requirements.Add(new PermissionRequirement(Permissions.ManageSettings)));
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Add Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed identity data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    
    var seeder = new IdentityDataSeeder(roleManager, userManager);
    await seeder.SeedAsync();
}

app.Run();
