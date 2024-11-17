namespace JO.Response.Base
{
    public class BaseViewModel : IViewModel
    {
        public long Id { get; set; }
        public long? Editor { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string ModifiedTimePersian { get; set; }
        public Guid ConcurrencyStatus { get; set; }
        public string? ModifiedDescription { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Enabled { get; set; }
    }
}
