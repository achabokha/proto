using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models
{
    public interface IEmbilyDbInitializer
    {
        Task SeedAsync();
    }

    public struct WellKnowIds
    {
        public const string userId = "1E9198F8-A351-4A1F-8C4C-E99C17B916D6";
        public const string adminId = "21478290-36D3-4C65-8FD0-C9CDD94633BA";
    }

    public class EmbilyDbInitializer : IEmbilyDbInitializer
    {
        private readonly DbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManage;

        public EmbilyDbInitializer(DbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManage = roleManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Roles.AnyAsync())
            {
                await _roleManage.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
                await _roleManage.CreateAsync(new IdentityRole { Name = Roles.User.ToString() });

                // H Account - User
                {
                    var email = "dwight.schrute@office.com";
                    var password = "Pa$$w0rd!";
                    var user = new ApplicationUser
                    {
                        Id = WellKnowIds.userId,
                        Email = email,
                        UserName = email,
                        FirstName = "Dwight",
                        LastName = "Schrute",
                        PhoneNumber = "+1 666-999-6969",
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true,
                    };

                    await _userManager.CreateAsync(user, password);
                    await _userManager.AddToRoleAsync(user, Roles.User.ToString());
                }

                // G account - Admin
                {
                    var email = "michael.scott@office.com";
                    var password = "Pa$$w0rd!";
                    var user = new ApplicationUser
                    {
                        Id = WellKnowIds.adminId,
                        Email = email,
                        UserName = email,
                        FirstName = "Michael",
                        LastName = "Scott",
                        PhoneNumber = "+1 666-999-6969",
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true,
                    };

                    await _userManager.CreateAsync(user, password);
                    await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
            }
        }
    }
}
