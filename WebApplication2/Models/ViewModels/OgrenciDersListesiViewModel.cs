using WebApplication2.Controllers;

namespace WebApplication2.Models.ViewModels
{
    public class OgrenciDersListesiViewModel
    {
        public int? OgrenciID { get; set; }
        public string AdSoyad { get; set; }
        public List<OgrenciDersDto> Dersler { get; set; } = new List<OgrenciDersDto>();
    }
}
