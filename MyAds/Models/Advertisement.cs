using MyAds.Enums;

namespace MyAds.Models
{
    public class Advertisement
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        public DateTime CreationTime { get; set; }
        public AdType Type { get; set; }
    }
}
