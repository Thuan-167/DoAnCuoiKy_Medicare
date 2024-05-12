using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorApointment.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DoctorApointment.Data.Configuration
{
    public class ServiceDetailtConfiguration : IEntityTypeConfiguration<ServiceDetailts>
    {
        public void Configure(EntityTypeBuilder<ServiceDetailts> builder)
        {
            builder.ToTable("ServiceDetailts");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Services).WithMany(x => x.ServiceDetailts).HasForeignKey(x => x.ServicesId);
            builder.HasOne(x => x.MedicalRecord).WithMany(x => x.ServiceDetailts).HasForeignKey(x => x.MedicalRecordId);
        }
    }
}
