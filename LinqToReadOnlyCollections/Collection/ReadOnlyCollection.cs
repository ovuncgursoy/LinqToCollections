﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace LinqToCollections.Collection {
    ///<summary>Utility class for implementing a readable collection via delegates.</summary>
    [DebuggerDisplay("{ToString()}")]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class ReadOnlyCollection<T> : IReadOnlyCollection<T> {
        private readonly Func<int> _counter;
        private readonly Func<IEnumerator<T>> _iterator;

        [ContractInvariantMethod()]
        private void ObjectInvariant() {
            Contract.Invariant(_counter != null);
            Contract.Invariant(_iterator != null);
        }

        ///<summary>Constructs a readable collection implementation.</summary>
        ///<param name="counter">Gets the number of Collection items.</param>
        ///<param name="efficientIterator">Iterates the items in the collection.</param>
        public ReadOnlyCollection(Func<int> counter, Func<IEnumerator<T>> iterator) {
            Contract.Requires<ArgumentException>(counter != null);
            Contract.Requires<ArgumentException>(iterator != null);
            this._counter = counter;
            this._iterator = iterator;
        }

        public int Count {
            get {
                var r = _counter();
                if (r < 0) throw new InvalidOperationException("Invalid counter delegate.");
                return r;
            }
        }
        public IEnumerator<T> GetEnumerator() { return this._iterator(); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        public override string ToString() {
            const int MaxPreviewItemCount = 10;
            var initialItems = String.Join(", ", System.Linq.Enumerable.Take(this, 10));
            var suffix = Count > MaxPreviewItemCount ? "..." : "]";
            return "Count: " + Count + ", Items: [" + initialItems + suffix;
        }
    }
}
