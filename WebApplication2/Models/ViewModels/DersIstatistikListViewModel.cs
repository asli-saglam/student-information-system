namespace WebApplication2.Models.ViewModels
{
    public class DersIstatistikListViewModel
    {
        public string Yil { get; set; }
        public string Yariyil { get; set; }
        public List<string> Yillar { get; set; }
        public List<string> Yariyillar { get; set; }
        public List<DersIstatistikViewModel> Istatistikler { get; set; }
    }
}