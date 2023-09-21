namespace Player.Models
{
    public class Track
    {
        public TagInfo Tags { get; private set; }
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public Track(string filePath)
        {
            FilePath = filePath;
            FileName = System.IO.Path.GetFileName(filePath);
            Tags = new TagInfo(filePath);
        }
    }
}
