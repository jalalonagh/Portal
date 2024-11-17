using System.ComponentModel.DataAnnotations;

namespace JO.Data.Base.ValueObjects.Types
{
    public class JONationalCode : ValueObject<JONationalCode>
    {
        [MaxLength(12)]
        public string? Identity { get; set; }

        protected JONationalCode() { }

        public JONationalCode(string code)
        {
            Identity = code;

            Validate();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Identity;
        }

        protected override void Update(JONationalCode other)
        {
            Identity = other.Identity;
        }

        protected override void Update(object parameter)
        {
            Identity = parameter.ToString();

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

        }

        public override string ToString()
        {
            return Identity ?? "";
        }
    }
}
