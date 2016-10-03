using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
namespace System.Linq
{
    partial class JsImplEnumerable
    {

        [JsType(Name = "System.Linq.Enumerable.SelectManyListIterator")]
        private class SelectManyListIterator<TSource, TResult> : Iterator<TResult>
        {
            private List<TSource> source;
            private Func<TSource, IEnumerable<TResult>> selector;
            private int index;
            private IEnumerator<TResult> innerEnumerator;
            public SelectManyListIterator(List<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
            {
                this.source = source;
                this.selector = selector;
            }
            public override JsImplEnumerable.Iterator<TResult> Clone()
            {
                return new JsImplEnumerable.SelectManyListIterator<TSource, TResult>(this.source, this.selector);
            }
            public override bool MoveNext()
            {
                if (this.state == 1)
                {
                    while (this.index < this.source.Count || innerEnumerator != null)
                    {
                        if (innerEnumerator == null)
                        {
                            TSource arg = this.source[this.index];
                            this.index++;

                            var innerEnumerable = this.selector(arg);

                            innerEnumerator = innerEnumerable.GetEnumerator();
                        }


                        var hadNext = innerEnumerator.MoveNext();

                        if (!hadNext)
                        {
                            innerEnumerator = null;
                            continue;
                        }

                        this.current = innerEnumerator.Current;
                        return true;

                    }
                    this.Dispose();
                }
                return false;
            }
            public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
            {
                //return new Enumerable.WhereSelectListIterator<TSource, TResult2>(this.source, this.predicate, Enumerable.CombineSelectors<TSource, TResult, TResult2>(this.selector, selector));
                return new JsImplEnumerable.WhereSelectEnumerableIterator<TResult, TResult2>(this, null, selector);
            }
            public override IEnumerable<TResult> Where(Func<TResult, bool> predicate)
            {
                return new JsImplEnumerable.WhereEnumerableIterator<TResult>(this, predicate);
            }
        }
    }

}
