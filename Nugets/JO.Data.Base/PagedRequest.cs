using System.Linq.Expressions;

namespace JO.Data.Base
{
    public class PagedRequest
    {
        public int? SkipCount { get; set; } = 0;
        public int? MaxResultCount { get; set; } = int.MaxValue;
        public string? Sorting { get; set; } = "Id";
        public bool? IsAsc { get; set; } = false;

        public PagedRequest()
        {

        }

        public PagedRequest(int? skip, int? max, string? sorting, bool? isAsc)
        {
            SkipCount = skip;
            MaxResultCount = max;
            Sorting = sorting ?? "Id";
            IsAsc = isAsc;
        }
    }
}
