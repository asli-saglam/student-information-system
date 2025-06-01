using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Ders
    {
        public int DersID { get; set; }

        [Required]
        public string DersAd { get; set; }

        public ICollection<OgrenciDers>? OgrenciDersler { get; set; }
    }
}
