using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.Data.Base.ValueObjects.Types
{
    public class JOFullName : ValueObject<JOFullName>
    {
        [MaxLength(50)]
        public string? FirstName { get; protected set; }
        [MaxLength(50)]
        public string? MiddleName { get; protected set; }
        [MaxLength(50)]
        public string? LastName { get; protected set; }

        protected JOFullName() { }

        public JOFullName(string? first, string? middle, string last)
        {
            FirstName = first;
            MiddleName = middle;
            LastName = last;

            Validate();
        }

        public JOFullName(string? first, string? last)
        {
            FirstName = first;
            LastName = last;

            Validate();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return MiddleName;
            yield return LastName;
        }

        protected override void Update(JOFullName other)
        {
            FirstName = other.FirstName;
            MiddleName = other.MiddleName;
            LastName = other.LastName;

            Validate();
        }

        [Obsolete]
        protected override void Update(object parameter)
        {

        }

        [Obsolete]
        protected override void Update(object parameter, object parameter2)
        {

        }

        [Obsolete]
        protected override void Update(object parameter, object parameter2, object parameter3)
        {

        }

        public override void Validate()
        {

        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(FirstName);

            if (!string.IsNullOrEmpty(MiddleName))
            {
                stringBuilder.Append(" " + MiddleName + " ");
            }

            stringBuilder.Append(LastName);

            return stringBuilder.ToString();
        }
    }
}
