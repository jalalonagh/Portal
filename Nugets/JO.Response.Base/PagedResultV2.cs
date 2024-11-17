using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JO.Response.Base
{
    public class PagedResultV2<TEntity>
        where TEntity : class
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<TEntity> Data { get; set; }
        public string OrderColumnName { get; set; }

        public PagedResultV2(int draw, int total, List<TEntity> data, List<KeyValuePair<string, bool>>? orderKeys = null)
        {
            Draw = draw;
            RecordsTotal = total;
            RecordsFiltered = data.Count;
            Data = data;
            OrderColumnName = GetOrderColumnName(orderKeys);
        }

        private string GetOrderedColumnDir(bool dir)
        {
            return dir ? "ASC" : "DESC";
        }

        private string GetOrderColumnFullName(KeyValuePair<string, bool> key)
        {
            return $"{key.Key} {GetOrderedColumnDir(key.Value)}";
        }

        private string GetOrderColumnName(List<KeyValuePair<string, bool>>? keys)
        {
            if (keys == null || !keys.Any())
                return "";

            var names = keys.Select(k => GetOrderColumnFullName(k)).ToList();

            return string.Join(",", names);
        }
    }
}
