using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DoctorApointment.Data.EF
{
    public class DoctorManageDbContextFactory : IDesignTimeDbContextFactory<DoctorManageDbContext>
    {
        public DoctorManageDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DoctorApointmentDb");

            var optionsBuilder = new DbContextOptionsBuilder<DoctorManageDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DoctorManageDbContext(optionsBuilder.Options);
        }
    }
}
