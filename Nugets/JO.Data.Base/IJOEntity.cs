namespace JO.Data.Base
{
    public interface IJOEntity
    {
        long Id { get; }
        long? Editor { get; }
        DateTime ModifiedTime { get; }
        string ModifiedTimePersian { get; }
        Guid ConcurrencyStatus { get; }
        string? ModifiedDescription { get; }
        bool? IsDeleted { get; }
        bool? Enabled { get; }

        void Validate();
        void SetId(long id);
        void SetEditor(long? id);
        void SetDeleted();
        void SetEnabled(bool? enabled);
        void SetTimeNow();
        void SetModifiedTime(DateTime time);
        void ReNewConcurrency();
        void SetIsSystemAction();
        bool GetIsSystemAction();
    }
}