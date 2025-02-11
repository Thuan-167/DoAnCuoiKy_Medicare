﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorApointment.Data.Enums;
using Microsoft.AspNetCore.Identity;

namespace DoctorApointment.Data.Entities
{
    public class AppUsers : IdentityUser<Guid>
    {

        public DateTime CreatedAt { get; set; }
        public Status Status { get; set; }
        public AppRoles AppRoles { get; set; }
        public Guid RoleId { get; set; }
        public Doctors Doctors { get; set; }
        public List<Patients> Patients { get; set; }
        public List<CommentsPost> CommentsPosts { get; set; }


    }
}
