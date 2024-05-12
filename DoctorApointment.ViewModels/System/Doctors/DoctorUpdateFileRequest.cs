using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DoctorApointment.ViewModels.System.Doctors
{
    public class DoctorUpdateFileRequest
    {
        public Guid Id { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public IFormFile? ThumbnailImage { get; set; }
        [Display(Name = "Hình ảnh")]
        public IFormFileCollection? Galleries { get; set; }
    }
}
