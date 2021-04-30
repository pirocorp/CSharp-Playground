namespace Abstractions.Functors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class Tree<T> : IReadOnlyCollection<Tree<T>>
    {
        private readonly IReadOnlyCollection<Tree<T>> children;

        public T Item { get; }

        public Tree(T item, IReadOnlyCollection<Tree<T>> children)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (children == null)
            {
                throw new ArgumentNullException(nameof(children));
            }

            this.Item = item;
            this.children = children;
        }

        public Tree<TResult> Map<TResult>(Func<T, TResult> mapping)
        {
            var mappedItem = mapping(this.Item);

            var mappedChildren = new List<Tree<TResult>>();

            foreach (var child in this.children)
            {
                mappedChildren.Add(child.Map(mapping));
            }

            return new Tree<TResult>(mappedItem, mappedChildren);
        }

        public int Count => this.children.Count;

        public IEnumerator<Tree<T>> GetEnumerator() 
            => this.children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => this.children.GetEnumerator();
    }
}
