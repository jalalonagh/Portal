using System.ComponentModel.DataAnnotations;

namespace JO.DataTransferObject
{
    public class PagedRequestDTO
    {
        [Range(0, int.MaxValue - 10, ErrorMessage = "مقدار رکورد های بارگزاری شده در بازه تعریف شده نیست")]
        public int? Page { get; set; } = 0;
        [Range(10, int.MaxValue, ErrorMessage = "مقدار رکورد های قابل بارگزاری در بازه تعریف شده نیست")]
        public int? Size { get; set; } = int.MaxValue;
        [MaxLength(100, ErrorMessage = "مقدار نام ستون برای مرتب سازی مناسب نیست")]
        public string? SortField { get; set; } = "Id";
        public string? SortOrder { get; set; } = "DESC";
        public string? Filter { get; set; }

        public int GetSkip()
        {
            if(Page <= 0)
            {
                return 0;
            }

            int skip = (Page ?? 0) * (Size ?? 0);
            skip -= (Size ?? 0);

            return skip;
        }

        public bool GetSortOrder()
        {
            return SortOrder == "DESC" ? false : SortOrder == "ASC" ? true : false;
        }

        public bool HasFilter()
        {
            return string.IsNullOrEmpty(Filter) || string.IsNullOrWhiteSpace(Filter) ? false : true;
        }
    }
}
