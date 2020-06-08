using System;
using System.Collections.Generic;

namespace TransactionStore.Api.Models.OData
{
    public class ODataResult<T> where T : class
    {
        public int Count { get; set; }

        public IEnumerable<T> Items { get; set; } = (IEnumerable<T>) Array.Empty<T>();
    }
}