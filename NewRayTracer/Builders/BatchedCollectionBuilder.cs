using NewRayTracer.Models.Collections;

using System;
using System.Collections.Generic;
using System.Linq;

namespace NewRayTracer.Builders
{
    public class BatchedCollectionBuilder<T> : IBuilder<BatchedCollection<T>>
    {
        private List<BatchItem<T>> _collection;
        
        public BatchedCollectionBuilder()
        {
            _collection = new List<BatchItem<T>>();
        }

        public BatchItem<T> Add(T value)
        {
            BatchItem<T> item = new BatchItem<T>(value);
            _collection.Add(item);
            return item;
        }

        public BatchedCollection<T> Build()
        {
            List<List<BatchItem<T>>> batches = new List<List<BatchItem<T>>>
            {
                _collection
            };

            for (int step = 0; true; step++)
            {
                // elements in the propagation list will be moved to the next step because they cannot be in the same batch as one or more elements in the current step
                HashSet<BatchItem<T>> propagationList = new HashSet<BatchItem<T>>();
                for(int i = batches[step].Count - 1; i >= 0; i--)
                {
                    BatchItem<T> pivot = batches[step][i];
                    for(int j = i - 1; j >= 0; j--)
                    {
                        // compare all the elements in the current step and add elements to the propagation list if they have a restriction.
                        BatchItem<T> compareTo = batches[step][j];
                        
                        bool later = false;
                        bool earlier = false;

                        foreach(IComparer<BatchItem<T>> restriction in pivot.Restrictions)
                        {
                            int comparison = restriction.Compare(pivot, compareTo);
                            later = later || comparison > 0;
                            earlier = earlier || comparison < 0;
                        }

                        foreach (IComparer<BatchItem<T>> restriction in compareTo.Restrictions)
                        {
                            int comparison = restriction.Compare(compareTo, pivot);
                            later = later || comparison < 0;
                            earlier = earlier || comparison > 0;
                        }

                        // if the pivot element must be both earlier and later, then there are conflicting restrictions / circular restrictions
                        if (later && earlier) throw new Exception("Found conflicting restrictions on elements.");
                        if (later) propagationList.Add(pivot);
                        if (earlier) propagationList.Add(compareTo);
                    }
                }

                // if no elements have to be propagated, then all the batches are sorted
                if (propagationList.Count == 0) break;

                // if all the elements have to be propagated, then there is a circular restriction.
                if (propagationList.Count == batches[step].Count) throw new Exception("Found circular restrictions.");

                foreach(BatchItem<T> element in propagationList)
                {
                    batches[step].Remove(element);
                }

                batches.Add(propagationList.ToList());
            }

            return new BatchedCollection<T>(batches.Select(b => b.Select(i => i.Value).ToList()).ToList());
        }
    }

    public class BatchItem<T>
    {
        private List<IComparer<BatchItem<T>>> _restrictions;

        public BatchItem(T value)
        {
            Value = value;
            _restrictions = new List<IComparer<BatchItem<T>>>();
        }

        public T Value { get; }
        public IReadOnlyList<IComparer<BatchItem<T>>> Restrictions => _restrictions;
        public BatchItem<T> WithRestrictions(params IComparer<BatchItem<T>>[] restrictions)
        {
            _restrictions.AddRange(restrictions);
            return this;
        }

        public BatchItem<T> Before<TBefore>()
        {
            WithRestrictions(new BeforeComparer<TBefore>());
            return this;
        }

        public BatchItem<T> After<TAfter>()
        {
            WithRestrictions(new AfterComparer<TAfter>());
            return this;
        }

        private class BeforeComparer<TBefore> : IComparer<BatchItem<T>>
        {
            public int Compare(BatchItem<T> x, BatchItem<T> y)
                => y.Value.GetType().Equals(typeof(TBefore)) ? -1 : 0;
        }

        private class AfterComparer<TAfter> : IComparer<BatchItem<T>>
        {
            public int Compare(BatchItem<T> x, BatchItem<T> y)
                => y.Value.GetType().Equals(typeof(TAfter)) ? 1 : 0;
        }
    }
}
