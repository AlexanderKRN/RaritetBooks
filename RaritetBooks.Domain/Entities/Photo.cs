using RaritetBooks.Domain.Common;

namespace RaritetBooks.Domain.Entities
{
    public abstract class Photo : Entity
    {
        public const string JPEG = "jpeg";
        public const string JPG = "jpg";
        public const string BMP = "bmp";
        public const string PNG = "png";
        
        public string Path { get; private set; }
        public bool IsMain { get; private set; }

        protected Photo(string path)
        {
            Path = path;
        }
    }
}