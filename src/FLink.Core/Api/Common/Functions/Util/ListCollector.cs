using System.Collections.Generic;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.Functions.Util
{
    /// <summary>
    /// A <see cref="ICollector{TRecord}"/> that puts the collected elements into a given list.
    /// </summary>
    /// <typeparam name="TElement">The type of the collected elements.</typeparam>
    public class ListCollector<TElement> : ICollector<TElement>
    {
        private readonly List<TElement> _list;

        public ListCollector(List<TElement> list) => _list = list;

        public void Collect(TElement element) => _list.Add(element);

        public void Close() { }
    }
}
