using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JO.Data.Base
{
    public class PagedRequestV2
    {
        public int? Draw { get; protected set; }
        public int Start { get; protected set; }
        public int Length { get; protected set; }
        public string? Columns { get; protected set; }
        public string[] AllowedColumns { get; protected set; }
        public PagedRequestV2Order[]? Order { get; protected set; }
        public PagedRequestV2Search? Search { get; protected set; }

        protected PagedRequestV2() { }

        public PagedRequestV2(int draw,
            int start,
            int length,
            string columns,
            string[] allowedColumns,
            PagedRequestV2Order[]? order = null,
            PagedRequestV2Search? search = null)
        {
            Draw = draw;
            Start = start;
            Length = length;
            Columns = columns.Replace(" ", "").Trim();
            AllowedColumns = allowedColumns.Select(s => s.Trim()).ToArray();
            Order = order;
            Search = search;

            if (Start <= 0)
            {
                Start = 0;
            }

            if (Length > 500)
            {
                throw new ArgumentException("در این سرویس امکان درخواست بیشتر از 500 رکورد مقدور نمی باشد .");
            }

            if (AllowedColumns == null ||
                !AllowedColumns.Any() ||
                !AllowedColumns.Where(w => !string.IsNullOrWhiteSpace(w) && !string.IsNullOrEmpty(w)).Any())
            {
                throw new ArgumentException("ستون های مجاز تعریف نشده است");
            }

            if (string.IsNullOrEmpty(Columns) && string.IsNullOrWhiteSpace(Columns))
            {
                Columns = "*";
            }

            if (Columns != "*")
            {
                if (!Regex.IsMatch(Columns, "^[a-zA-Z0-9_]+(,[a-zA-Z0-9_]+)*$"))
                {
                    throw new ArgumentException("معرفی ستون ها معتبر نیست . موارد مجاز در معرفی ستون ها حروف لاتین و عدد و زیر خط می باشد و برای جداسازی ستون ها از ویرگول لاتین استفاده شود");
                }

                var cList = Columns.Split(",").Select(s => s.Trim());
                var aList = cList.Intersect(AllowedColumns);

                Columns = string.Join(",", aList);
            }
            else
            {
                Columns = string.Join(",", AllowedColumns);
            }
        }

        public bool HasOrder()
        {
            if (Order == null || !Order.Any())
            {
                return false;
            }

            return true;
        }

        public bool HasText2Search()
        {
            if (Search == null || string.IsNullOrEmpty(Search.Value) || string.IsNullOrWhiteSpace(Search.Value))
            {
                return false;
            }

            return true;
        }

        public List<KeyValuePair<string, bool>> GetOrderKeys()
        {
            if(Order == null || !Order.Any())
            {
                return new List<KeyValuePair<string, bool>>();
            }

            return Order.Select(s => new KeyValuePair<string, bool>(s.Name, s.Dir.Trim().ToLower() == "asc")).ToList();
        }

        public string[] GetSelectedColumns()
        {
            return Columns.Split(",");
        }
    }

    public class PagedRequestV2Search
    {
        public string Value { get; set; }
        public bool Regex { get; set; } = false;
        public string[] Fixed { get; set; }
    }

    public class PagedRequestV2Order
    {
        public int Column { get; set; }
        public string Dir { get; set; }
        public string Name { get; set; }
    }
}
