using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;

namespace JO.Data.Base.ValueObjects.Types
{
    public class JOAvatar : ValueObject<JOAvatar>
    {
        [MaxLength(300)]
        public string? Root { get; protected set; }
        [MaxLength(300)]
        public string? Path { get; protected set; }
        [MaxLength(100)]
        public string? Name { get; protected set; }
        [MaxLength(5)]
        public string? FileType { get; protected set; }
        [MaxLength(300)]
        public string? ThumbnailPath { get; protected set; }
        [MaxLength(100)]
        public string? ThumbnailName { get; protected set; }
        [MaxLength(5)]
        public string? ThumbnailFileType { get; protected set; }

        protected JOAvatar() { }

        public JOAvatar(string? path, string? root)
        {
            Root = root;
            Path = path;

            Name = !string.IsNullOrEmpty(path) ? System.IO.Path.GetFileNameWithoutExtension(path) : null;
            FileType = !string.IsNullOrEmpty(path) ? System.IO.Path.GetExtension(path) : null;

            ThumbnailPath = null;
            ThumbnailName = null;
            ThumbnailFileType = null;

            Validate();
        }

        public JOAvatar(string? path, string? thumb, string? root)
        {
            Root = root;
            Path = path;

            ThumbnailPath = thumb;

            Name = !string.IsNullOrEmpty(path) ? System.IO.Path.GetFileNameWithoutExtension(path) : null;
            FileType = !string.IsNullOrEmpty(path) ? System.IO.Path.GetExtension(path) : null;

            ThumbnailName = !string.IsNullOrEmpty(thumb) ? System.IO.Path.GetFileNameWithoutExtension(thumb) : null;
            ThumbnailFileType = !string.IsNullOrEmpty(thumb) ? System.IO.Path.GetExtension(thumb) : null;

            Validate();
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Path;
            yield return Name;
            yield return FileType;
            yield return ThumbnailPath;
            yield return ThumbnailName;
            yield return ThumbnailFileType;
        }

        protected override void Update(JOAvatar other)
        {
            Path = other.Path;
            Name = other.Name;
            FileType = other.FileType;
            ThumbnailPath = other.ThumbnailPath;
            ThumbnailName = other.ThumbnailName;
            ThumbnailFileType = other.ThumbnailFileType;

            Validate();
        }

        protected override void Update(object parameter)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        protected override void Update(object parameter, object parameter2)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        protected override void Update(object parameter, object parameter2, object parameter3)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(ThumbnailPath) ? ThumbnailPath : Path ?? "";
        }
    }
}
