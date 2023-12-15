namespace MyAds.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int AdId { get; set; }
        public string Com { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
