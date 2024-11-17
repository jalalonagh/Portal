using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.Data.Base.ValueObjects.Types
{
    public class JOAddress : ValueObject<JOAddress>
    {
        [MaxLength(150)]
        public string? Street { get; protected set; }
        [MaxLength(50)]
        public string? City { get; protected set; }
        [MaxLength(50)]
        public string? Province { get; protected set; }
        [MaxLength(50)]
        public string? Country { get; protected set; }
        [MaxLength(20)]
        public string? PostCode { get; protected set; }
        [MaxLength(10)]
        public string? No { get; protected set; }

        protected JOAddress() { }

        public JOAddress(string? street, string? city, string? province, string? country, string? postCode, string? no)
        {
            Street = street;
            City = city;
            Province = province;
            Country = country;
            PostCode = postCode;
            No = no;
            Validate();
        }

        public static JOAddress Copy(JOAddress address)
        {
            return new JOAddress(address.Street, address.City, address.Province, address.Country, address.PostCode, address.No);
        }

        public JOAddress(string? postCode)
        {
            Street = "street";
            City = "city";
            Province = "province";
            Country = "country";
            PostCode = postCode;
            No = "no";
            Validate();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return Province;
            yield return City;
            yield return Street;
            yield return PostCode;
            yield return No;
        }

        protected override void Update(JOAddress other)
        {
            Street = other.Street;
            City = other.City;
            Province = other.Province;
            Country = other.Country;
            PostCode = other.PostCode;
            No = other.No;
            Validate();
        }

        protected override void Update(object parameter)
        {
            if (parameter != null)
            {
                string postCode = parameter.ToString();

                var isNumeric = long.TryParse(postCode, out long n);

                if (postCode == null || postCode.Length != 10 || !isNumeric)
                {
                    throw new Exception("مقدار ارسالی کدپستی نیست");
                }

                var address = InqueryAddressByPostCodeAsync(postCode).Result;

                Street = address.Street;
                City = address.City;
                Province = address.Province;
                Country = address.Country;
                PostCode = address.PostCode;
                No = address.No;
                Validate();
            }
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
            if (PostCode != null && (PostCode.Length > 10 || PostCode.Length < 10))
            {
                throw new Exception("کد پستی نا معتبر است");
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(Country))
            {
                stringBuilder.Append(Country);
            }
            if (string.IsNullOrEmpty(Province))
            {
                stringBuilder.Append("، " + Province);
            }
            if (string.IsNullOrEmpty(City))
            {
                stringBuilder.Append("، " + City);
            }
            if (string.IsNullOrEmpty(Street))
            {
                stringBuilder.Append("، " + Street);
            }
            if (string.IsNullOrEmpty(No))
            {
                stringBuilder.Append("، " + No);
            }
            if (string.IsNullOrEmpty(PostCode))
            {
                stringBuilder.Append("، " + PostCode);
            }

            return stringBuilder.ToString();
        }

        private async Task<JOAddress> InqueryAddressByPostCodeAsync(string postCode)
        {
            return default;
        }
    }
}
