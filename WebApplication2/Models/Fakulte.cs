using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Fakulte
    {
        public int FakulteID { get; set; }

        [Required]
        public string FakulteAd { get; set; }

        public ICollection<Bolum>? Bolumler { get; set; }
    }
}