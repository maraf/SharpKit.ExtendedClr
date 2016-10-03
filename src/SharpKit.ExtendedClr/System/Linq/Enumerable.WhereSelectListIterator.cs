using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
namespace System.Linq
{
    partial class JsImplEnumerable
    {

        [JsType(Name = "System.Linq.Enumerable.WhereSelectListIterator")]
        private class WhereSelectListIterator<TSource, TResult> : Iterator<TResult>
        {
            private List<TSource> source;
            private Func<TSource, bool> predicate;
            private Func<TSource, TResult> selector;
            private IEnumerator<TSource> enumerator;
            public WhereSelectListIterator(List<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
            {
                this.source = source;
                this.predicate = predicate;
                this.selector = selector;
            }
            public override JsImplEnumerable.Iterator<TResult> Clone()
            {
                return new JsImplEnumerable.WhereSelectListIterator<TSource, TResult>(this.source, this.predicate, this.selector);
            }
            public override bool MoveNext()
            {
                switch (this.state)
                {
                    case 1:
                        this.enumerator = this.source.GetEnumerator();
                        this.state = 2;
                        break;
                    case 2:
                        break;
                    default:
                        return false;
                }
                while (this.enumerator.MoveNext())
                {
                    TSource current = this.enumerator.Current;
                    if (this.predicate == null || this.predicate(current))
                    {
                        this.current = this.selector(current);
                        return true;
                    }
                }
                this.Dispose();
                return false;
            }
            public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
            {
                return new JsImplEnumerable.WhereSelectListIterator<TSource, TResult2>(this.source, this.predicate, JsImplEnumerable.CombineSelectors<TSource, TResult, TResult2>(this.selector, selector));
            }
            public override IEnumerable<TResult> Where(Func<TResult, bool> predicate)
            {
                return new JsImplEnumerable.WhereEnumerableIterator<TResult>(this, predicate);
            }
        }
    }

}
