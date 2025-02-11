﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorApointment.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DoctorApointment.Data.Configuration
{
    public class CommentsPostConfiguration : IEntityTypeConfiguration<CommentsPost>
    {
        public void Configure(EntityTypeBuilder<CommentsPost> builder)
        {
            builder.ToTable("CommentsPost");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(255);

            builder.HasOne(x => x.AppUsers).WithMany(x => x.CommentsPosts).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Posts).WithMany(x => x.CommentsPosts).HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
