using Microsoft.AspNetCore.Identity;

namespace MyAds.Models
{
    public class User : IdentityUser
    {
        public List<Advertisement> Ads { get; set; }
    }
}
