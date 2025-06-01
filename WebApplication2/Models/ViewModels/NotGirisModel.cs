using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.ViewModels
{
    public class NotGirisModel
    {
        [Range(0, 100, ErrorMessage = "Not 0-100 arasında olmalıdır")]
        public int? Vize { get; set; }

        [Range(0, 100, ErrorMessage = "Not 0-100 arasında olmalıdır")]
        public int? Final { get; set; }
    }
}
