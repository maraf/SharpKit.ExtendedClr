using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
namespace System.Linq
{
    partial class JsImplEnumerable
    {
        /*private static Func<TSource, TResult> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, TMiddle> selector1, Func<TMiddle, TResult> selector2)
        {
            return (TSource x) => selector2(selector1(x));
        }*/

        [JsType(Name = "System.Linq.Enumerable.SelectManyEnumerableIterator")]
        private class SelectManyEnumerableIterator<TSource, TResult> : JsImplEnumerable.Iterator<TResult>
        {
            private IEnumerable<TSource> source;
            private Func<TSource, IEnumerable<TResult>> selector;
            private IEnumerator<TSource> enumerator;
            private IEnumerator<TResult> innerEnumerator;
            public SelectManyEnumerableIterator(IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
            {
                this.source = source;
                this.selector = selector;
            }
            public override JsImplEnumerable.Iterator<TResult> Clone()
            {
                return new JsImplEnumerable.SelectManyEnumerableIterator<TSource, TResult>(this.source, this.selector);
            }
            public override void Dispose()
            {
                if (this.enumerator != null)
                {
                    this.enumerator.Dispose();
                }
                this.enumerator = null;
                base.Dispose();
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

                while (true)
                {
                    if (this.innerEnumerator == null)
                    {
                        if (this.enumerator.MoveNext())
                        {
                            this.innerEnumerator = selector(this.enumerator.Current).GetEnumerator();
                        }
                        else
                        {
                            this.Dispose();
                            return false;
                        }
                    }
                    else
                    {
                        if (this.innerEnumerator.MoveNext())
                        {
                            this.current = this.innerEnumerator.Current;
                            return true;
                        }
                        this.innerEnumerator = null;
                    }
                    
                }
            }
            public override IEnumerable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
            {
                return new JsImplEnumerable.WhereSelectEnumerableIterator<TResult, TResult2>(this, null, selector);
            }
            public override IEnumerable<TResult> Where(Func<TResult, bool> predicate)
            {
                return new JsImplEnumerable.WhereEnumerableIterator<TResult>(this, predicate);
            }
        }

    }
}