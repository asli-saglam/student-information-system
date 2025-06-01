using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Bolum
    {
        public int BolumID { get; set; }

        [Required]
        public string BolumAd { get; set; }

        public int FakulteID { get; set; }
        public Fakulte? Fakulte { get; set; }

        public ICollection<Ogrenci>? Ogrenciler { get; set; }
    }
}
