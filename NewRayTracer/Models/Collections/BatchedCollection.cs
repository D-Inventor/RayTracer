using System.Collections;
using System.Collections.Generic;

namespace NewRayTracer.Models.Collections
{
    public class BatchedCollection<T> : IReadOnlyList<IEnumerable<T>>
    {
        private readonly IReadOnlyList<IEnumerable<T>> _batches;

        public BatchedCollection(IReadOnlyList<IEnumerable<T>> batches)
        {
            _batches = batches;
        }

        public IEnumerable<T> this[int index] => _batches[index];

        public int Count => _batches.Count;

        public IEnumerator<IEnumerable<T>> GetEnumerator()
        {
            return _batches.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_batches).GetEnumerator();
        }
    }
}
