namespace JO.Response.Base
{
    [Serializable]
    public class PagedResult<TEntity>
        where TEntity : class
    {
        public int Count { get; protected set; }
        public List<TEntity> Items { get; protected set; } = new List<TEntity>();

        public PagedResult(int count, List<TEntity> entities)
        {
            Count = count;
            Items = entities;
        }
    }
}
