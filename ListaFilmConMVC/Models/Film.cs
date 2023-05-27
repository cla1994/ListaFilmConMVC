namespace ListaFilmConMVC.Models
{
    public class Film
    {
        public int FilmID { get; set; }

        public string Title { get; set; } = "";

        public string Year { get; set; } = "";

        public string imdbIdentifier { get; set; } = "";

        public string Type { get; set; } = "";

        public string Poster { get; set; } = "";

        public Picture? Picture { get; set; }

    }

    public class Picture
    {
        public int PictureID { get; set; }
        public string PictureName { get; set; } = "";
        public byte[]? RawData { get; set; }
    }
}
