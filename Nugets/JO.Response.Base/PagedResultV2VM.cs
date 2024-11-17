using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JO.Response.Base
{
    public class PagedResultV2VM<TVM>
        where TVM : BaseViewModel
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<TVM>? Data { get; set; }
        public string OrderColumnName { get; set; }
    }
}
