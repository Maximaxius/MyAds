using MyAds.Models;

namespace MyAds.ViewModels
{
    public class AllAdvertisementViewModel
    {
        public List<User> User { get; set; }
        public List<Comment> Comments { get; set; }

        public IEnumerable<AddViewModel> Ads { get; set; }

    }
}
