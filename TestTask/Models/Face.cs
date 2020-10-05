using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class Face
    {
        public int id { get; set; }
        public int person_id { get; set; }
        public string imageName { get; set; }
        public byte[] imageBytes { get; set; }
       
    }
}
