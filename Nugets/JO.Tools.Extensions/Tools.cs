using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace JO.Tools.Extensions
{
    public static class Tools
    {
        private static JsonSerializerOptions jsonSerializerOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
        };

        public static JsonSerializerOptions JsonSerializerOptions { get => jsonSerializerOptions; set => jsonSerializerOptions = value; }

        public static readonly Dictionary<int, string> PersianMonthName = new()
        {
            { 1, "فروردین" },
            { 2, "اردیبهشت" },
            { 3, "خرداد" },
            { 4, "تیر" },
            { 5, "مرداد" },
            { 6, "شهریور" },
            { 7, "مهر" },
            { 8, "آبان" },
            { 9, "آذر" },
            { 10, "دی" },
            { 11, "بهمن" },
            { 12, "اسفند" }
        };

        public static bool IsNumeric(string expresion)
        {
            return decimal.TryParse(expresion, out _);
        }

        public static string ReverseString(string expresion)
        {
            return new string(expresion.ToCharArray().Reverse().ToArray());
        }

        public static string EnglishDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public static string CurrentPersianDateTime()
        {
            return PerisanDate(DateTime.Now) + " " + CurrentTime();
        }

        public static string PerisanDate(DateTime dateTime)
        {
            var persianCalendar = new PersianCalendar();
            return persianCalendar.GetYear(dateTime).ToString("0000") + "/" + persianCalendar.GetMonth(dateTime).ToString("00") + "/" + persianCalendar.GetDayOfMonth(dateTime).ToString("00");
        }

        public static string PerisanDate(string date, bool isBancsRX = false)
        {
            if (date == "")
            {
                return date;
            }

            try
            {
                if (!isBancsRX)
                {
                    var dateTime = new DateTime(int.Parse(date[..4]), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)));
                    var persianCalendar = new PersianCalendar();
                    return persianCalendar.GetYear(dateTime).ToString("0000") + "/" + persianCalendar.GetMonth(dateTime).ToString("00") + "/" + persianCalendar.GetDayOfMonth(dateTime).ToString("00");
                }
                else
                {
                    return date.Substring(4, 4) + "/" + date.Substring(2, 2) + "/" + date[..2];
                }
            }
            catch
            {
                return date;
            }
        }

        public static DateTime PersianToGregorian(string persianDate)
        {
            var datePart = persianDate.Split('/');
            var persianCalendar = new PersianCalendar();

            return new DateTime(int.Parse(datePart[0]), int.Parse(datePart[1]), int.Parse(datePart[2]), persianCalendar);
        }

        public static string CurrentTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        public static string FotmatTime(string time)
        {
            return time[..2] + ":" + time.Substring(2, 2) + ":" + time.Substring(4, 2);
        }

        public static string Sheba(string accountNumber, int accountType = 0)
        {
            string sheba = "016" + accountType.ToString() + accountNumber.PadLeft(0x12, '0');

            return "IR" + (98M - decimal.Parse(sheba + "182700") % 97M).ToString().PadLeft(2, '0') + sheba;
        }

        public static string FixArabicString(string input)
        {
            return input.Replace('ی', 'ي').Replace('ك', 'ک');
        }

        public static string FixMobileNumberStartWith98(string mobileNumber)
        {
            mobileNumber = mobileNumber.Trim().TrimStart('0');
            mobileNumber = mobileNumber.Length == 10 ? $"98{mobileNumber}" : mobileNumber;

            return mobileNumber;
        }

        public static string FixMobileNumberStartWith0(string mobileNumber)
        {
            mobileNumber = mobileNumber.Trim().TrimStart('0');
            mobileNumber = mobileNumber.Length == 10 ? $"0{mobileNumber}" : mobileNumber.Length == 12 ? $"0{mobileNumber[2..]}" : mobileNumber;

            return mobileNumber;
        }

        public static List<T> ConvertDataTable<T>(DataTable dataTable)
        {
            List<T> data = new();

            foreach (DataRow row in dataTable.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }

            return data;
        }

        private static T GetItem<T>(DataRow dataRow)
        {
            Type type = typeof(T);
            T instance = Activator.CreateInstance<T>();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    if (propertyInfo.Name == column.ColumnName)
                    {
                        propertyInfo.SetValue(instance, dataRow[column.ColumnName], null);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return instance;
        }

        public static DateTime EndOfDay(this DateTime @this)
        {
            return new DateTime(@this.Year, @this.Month, @this.Day).AddDays(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
        }

        public static string GetRouteController(this string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return value.Split("/").Count() > 0 ? value.Split("/")[0] : string.Empty;
        }

        public static string GetRouteAction(this string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return value.Split("/").Count() > 1 ? value.Split("/")[1] : string.Empty;
        }
    }
}
