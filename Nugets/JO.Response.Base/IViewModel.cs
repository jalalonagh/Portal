namespace JO.Response.Base
{
    public interface IViewModel
    {
        long Id { get; }
        long? Editor { get; }
        DateTime ModifiedTime { get; }
        string ModifiedTimePersian { get; }
        Guid ConcurrencyStatus { get; }
        string? ModifiedDescription { get; }
        bool? IsDeleted { get; }
        bool? Enabled { get; }
    }
}
