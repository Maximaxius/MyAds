using MyAds.Enums;

namespace MyAds.ViewModels
{
    public class AddViewModel
    {

        public string? Description { get; set; }

        public string? Name { get; set; }
        public string? UserId { get; set; }

        public DateTime CreationTime { get; set; }

        public int Id { get; set; }
        public AdType Type { get; set; }

    }
}
