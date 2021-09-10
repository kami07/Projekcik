namespace MemoryGame.Models
{
    public class PictureModel
    {
        public int Id { get; set; }
        public string ImageSource { get; set; }
        private bool isViewed { get; set; }
        private bool isMatched { get; set; }
        private bool isFailed { get; set; }

    }
}
