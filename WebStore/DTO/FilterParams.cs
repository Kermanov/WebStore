using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public enum SortParameter
    {
        CheapFirst,
        ExpensiveFirst
    }

    public class FilterParams
    {
        public string NameFilter { get; set; }
        public int CategoryFilter { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public SortParameter SortParameter { get; set; }
    }
}
