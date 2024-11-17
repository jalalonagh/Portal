using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class PagedRequestV2DTO
    {
        public int? Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string? Columns { get; set; }
        public PagedRequestV2OrderDTO[]? Order { get; set; }
        public PagedRequestV2Search? Search { get; set; }

        public bool HasOrder()
        {
            if(Order == null || !Order.Any())
                return false;

            return true;
        }

        public bool HasText2Search()
        {
            if (Search == null || string.IsNullOrEmpty(Search.Value) || string.IsNullOrWhiteSpace(Search.Value))
                return false;

            return true;
        }
    }

    public class PagedRequestV2Search
    {
        public string Value { get; set; }
        public bool? Regex { get; set; } = false;
        public string[]? Fixed { get; set; }
    }

    public class PagedRequestV2OrderDTO
    {
        public int? Column { get; set; }
        public string Dir { get; set; }
        public string Name { get; set; }
    }
}
