namespace JO.AutoMapper
{
    public interface IJOMapper
    {
        TDS? Map<TDS>(object? src);
    }
}
