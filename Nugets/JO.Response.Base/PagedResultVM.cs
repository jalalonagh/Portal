namespace JO.Response.Base
{
    [Serializable]
    public class PagedResultVM<TVM>
        where TVM : BaseViewModel
    {
        public int Count { get; protected set; }
        public List<TVM> Items { get; protected set; } = new List<TVM>();

        public PagedResultVM(int count, List<TVM> entities)
        {
            Count = count;
            Items = entities;
        }
    }
}
