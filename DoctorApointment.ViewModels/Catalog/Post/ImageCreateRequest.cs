﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApointment.ViewModels.Catalog.Post
{
    public class ImageCreateRequest
    {
        public IFormFile File { get; set; }
    }
}
