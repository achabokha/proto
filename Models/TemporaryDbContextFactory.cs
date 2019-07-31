using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Embily.Models
{
    public class TemporaryDbContextFactory : IDesignTimeDbContextFactory<EmbilyDbContext>
    {
        public EmbilyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<EmbilyDbContext>();

            var connectionUnix = "Server=localhost,1433;Initial Catalog=EmbilyDBLocalMac;Persist Security Info=False;User ID=sa;Password=P@55w0rd;";
            var connectionWin = "Server=(localdb)\\MSSQLLocalDB; Database=EmbilyDBLocal; Trusted_Connection=True; MultipleActiveResultSets=true";

            var connection = Environment.OSVersion.Platform == PlatformID.Unix ? connectionUnix : connectionWin;

            builder.UseSqlServer(connection,
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(EmbilyDbContext).GetTypeInfo().Assembly.GetName().Name));

            builder.UseOpenIddict();

            return new EmbilyDbContext(builder.Options);
        }
    }
}
