using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace JO.Data.Base.ValueObjects.Types
{
    public class JODateTime : ValueObject<JODateTime>
    {
        private readonly string pattern1 = "^\\d{4}-((0\\d)|(1[012]))-(([012]\\d)|3[01])$";
        private readonly string pattern2 = "^\\d{4}/((0\\d)|(1[012]))/(([012]\\d)|3[01])$";

        public DateTime? Date { get; protected set; }
        public TimeSpan? Time { get; protected set; }
        [MaxLength(15)]
        public string? PersianDate { get; protected set; }

        protected JODateTime() { }

        public JODateTime(DateTime? dateTime)
        {
            Date = dateTime;
            Time = dateTime?.TimeOfDay;
            PersianDate = dateTime?.ToLocalTime().ToString("yyyy-MM-dd") ?? null;

            Validate();
        }

        public JODateTime(string? dateTime)
        {
            if (string.IsNullOrEmpty(dateTime))
            {
                throw new Exception($"مقدار ارسالی برای {nameof(dateTime)} خالی است.");
            }

            if (!string.IsNullOrEmpty(dateTime))
            {
                dateTime = dateTime.Replace("-", "/");
                var splited = dateTime.Split("/");
                string date = "";
                for (int idx = 0; idx < splited.Length; idx++)
                {
                    if (idx > 0)
                    {
                        date += "/";
                    }
                    if (splited[idx].Length == 1)
                    {
                        date += "0" + splited[idx];
                    }
                    else
                    {
                        date += splited[idx];
                    }
                }
                dateTime = date;
            }

            if (!Regex.IsMatch(dateTime, pattern1, RegexOptions.IgnoreCase) && !Regex.IsMatch(dateTime, pattern2, RegexOptions.IgnoreCase))
            {
                throw new Exception($"مقدار ارسالی برای {nameof(dateTime)} تاریخ نیست. تاریخ را به شمسی و صحیح ارسال کنید.");
            }

            PersianCalendar pc = new PersianCalendar();

            var p = System.DateTime.Parse(dateTime);

            Date = new DateTime(p.Year, p.Month, p.Day, pc);
            PersianDate = dateTime.Replace("/", "-");

            Validate();
        }

        public bool IsExpired()
        {
            if (Date == null)
            {
                throw new Exception("تاریخ مورد مقایسه مقدار ندارد");
            }

            if (Date < DateTime.Now)
            {
                return true;
            }

            return false;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Date;
            yield return Time;
            yield return PersianDate;
        }

        public override void Validate()
        {
            if (Date != null && Date.Value.Year < 630)
            {
                throw new Exception("تاریخ مورد استفاده غیر مجاز است");
            }
        }

        protected override void Update(JODateTime other)
        {
            Date = other.Date;
            Time = other.Time;
            PersianDate = other.PersianDate;

            Validate();
        }

        protected override void Update(object parameter)
        {
            if (parameter != null && parameter is DateTime)
            {
                Date = (DateTime)parameter;
                Time = ((DateTime)parameter).TimeOfDay;
                PersianDate = Date.Value.ToLocalTime().ToString("yyyy-MM-dd");

                Validate();
            }
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
            return PersianDate ?? Date?.ToLocalTime().ToString("yyyy-MM-dd") ?? "";
        }
    }
}
