
using Div.Link.Project01.DAL.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

//https://localhost:7028/swagger/index.html
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Div.Link.Project01.BLL;
using Div.Link.Project01.DAL.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddServices(builder.Configuration);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = Microsoft.AspNetCore.Identity.IdentityConstants.ExternalScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                };
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/signin-google"; // Match your Google Console exactly
                
                // Force HTTPS for the redirect URI manually to solve Railway proxy issues
                options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
                {
                    OnRedirectToAuthorizationEndpoint = context =>
                    {
                        var redirectUri = context.RedirectUri;
                        // Replace the internal redirect_uri parameter to follow HTTPS
                        if (redirectUri.Contains("redirect_uri=http%3A%2F%2F"))
                        {
                            redirectUri = redirectUri.Replace("redirect_uri=http%3A%2F%2F", "redirect_uri=https%3A%2F%2F");
                        }
                        context.Response.Redirect(redirectUri);
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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

// Enable detailed error pages for debugging the 500 error on Railway
app.UseDeveloperExceptionPage();

// Essential for Railway and Docker to handle HTTPS correctly
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto,
    KnownNetworks = { },
    KnownProxies = { }
});

// Enable Swagger always for easy testing on Railway
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    // Development specific settings if any
}

app.UseDefaultFiles(); // Allow index.html to be the default page
app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Seed Roles and Admin User
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    
                    // Create Roles
                    string[] roleNames = { "Admin", "User" };
                    foreach (var roleName in roleNames)
                    {
                        if (!roleManager.RoleExistsAsync(roleName).Result)
                        {
                            roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
                        }
                    }

                    // Create Default Admin
                    var adminUser = userManager.FindByEmailAsync("admin@authpro.com").Result;
                    if (adminUser == null)
                    {
                        var newAdmin = new ApplicationUser { UserName = "admin", Email = "admin@authpro.com" };
                        var result = userManager.CreateAsync(newAdmin, "Admin@123").Result;
                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(newAdmin, "Admin").Wait();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error if needed
                }
            }

            app.Run();
       
    
