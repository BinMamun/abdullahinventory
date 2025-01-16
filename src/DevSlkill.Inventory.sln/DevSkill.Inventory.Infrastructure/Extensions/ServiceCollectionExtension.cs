using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services
               .AddIdentity<ApplicationUser, ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddUserManager<ApplicationUserManager>()
               .AddRoleManager<ApplicationRoleManager>()
               .AddSignInManager<ApplicationSignInManager>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministrationPolicy", policy =>
                    policy.RequireRole("Super Admin"));

                options.AddPolicy("ViewIndexPolicy", policy =>
                    policy.RequireClaim("Can View List?", "true"));

                options.AddPolicy("CreatePolicy", policy =>
                    policy.RequireClaim("Can Create?", "true"));

                options.AddPolicy("EditPolicy", policy =>
                    policy.RequireClaim("Can Edit?", "true"));

                options.AddPolicy("DeletePolicy", policy =>
                    policy.RequireClaim("Can Delete?", "true"));

                options.AddPolicy("DeleteAllPolicy", policy =>
                    policy.RequireClaim("Can DeleteAll?", "true"));

            });
        }
    }
}
