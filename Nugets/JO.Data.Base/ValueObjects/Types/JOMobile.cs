using System.ComponentModel.DataAnnotations;

namespace JO.Data.Base.ValueObjects.Types
{
    public class JOMobile : ValueObject<JOMobile>
    {
        [MaxLength(15)]
        public string? Number { get; protected set; }

        protected JOMobile() { }

        public JOMobile(string no)
        {
            Number = no;

            Validate();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }

        protected override void Update(JOMobile other)
        {
            Number = other.Number;
        }

        protected override void Update(object parameter)
        {
            Number = parameter.ToString();

            Validate();
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

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Number ?? "";
        }
    }
}
