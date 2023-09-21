namespace Player.Models
{
    public class TagInfo
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string AlbumArtist { get; set; }
        public int? Year { get; set; }
        public string Composer { get; set; }
        public string Publisher { get; set; }
        public string Comment { get; set; }
        public string Lyrics { get; set; }
        public string Genre { get; set; }
        public int? BitDepth { get; set; }
        public int? Bitrate { get; set; }
        public int? BPM { get; set; }
        public int? TrackNumber { get; set; }
        public int? DiscNumber { get; set; }
        public int Duration { get; set; }
        public string Group { get; set; }
        public double SampleRate { get; set; }

        public TagInfo(string fileName)
        {
            var tags = new ATL.Track(fileName);
            Title = tags.Title;
            Artist = tags.Artist;
            AlbumArtist = tags.AlbumArtist;
            Year = tags.Year;
            Composer = tags.Composer;
            Publisher = tags.Publisher;
            Album = tags.Album;
            BitDepth = tags.BitDepth;
            Bitrate = tags.Bitrate;
            Lyrics = tags.Lyrics.UnsynchronizedLyrics;
            Genre = tags.Genre;
            BPM = tags.BPM;
            TrackNumber = tags.TrackNumber;
            Comment = tags.Comment;
            DiscNumber = tags.DiscNumber;
            Duration = tags.Duration;
            Group = tags.Group;
            SampleRate = tags.SampleRate;
        }
    }
}
