using Microsoft.AspNetCore.Http;
using System;

namespace EronNew.Models
{
    public class ImageUploader
    {
        public Guid qquuid { get; set; }
        public string qqfilename { get; set; }
        public int qqtotalfilesize { get; set; }
        public IFormFile qqfile { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }



}
