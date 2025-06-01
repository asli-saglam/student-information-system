using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Ogrenci
    {
        public int OgrenciID { get; set; }

        [Required]
        public string Ad { get; set; }

        [Required]
        public string Soyad { get; set; }

        public int BolumID { get; set; }
        public Bolum? Bolum { get; set; }

        public ICollection<OgrenciDers>? OgrenciDersler { get; set; }
    }
}
