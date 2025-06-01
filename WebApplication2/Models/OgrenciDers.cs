using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class OgrenciDers
    {
        public int OgrenciID { get; set; }
        public Ogrenci Ogrenci { get; set; }

        public int DersID { get; set; }
        public Ders Ders { get; set; }

        [Required]
        public string Yil { get; set; }

        [Required]
        public string Yariyil { get; set; }

        public int? Vize { get; set; }
        public int? Final { get; set; }
    }
}
