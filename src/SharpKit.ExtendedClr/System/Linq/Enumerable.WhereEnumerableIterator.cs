using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
namespace System.Linq
{
    partial class JsImplEnumerable
    {

        [JsType(Name = "System.Linq.Enumerable.WhereEnumerableIterator")]
        private class WhereEnumerableIterator<TSource> : JsImplEnumerable.Iterator<TSource>
        {
            private IEnumerable<TSource> source;
            private Func<TSource, bool> predicate;
            private IEnumerator<TSource> enumerator;
            public WhereEnumerableIterator(IEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                this.source = source;
                this.predicate = predicate;
            }
            public override JsImplEnumerable.Iterator<TSource> Clone()
            {
                return new JsImplEnumerable.WhereEnumerableIterator<TSource>(this.source, this.predicate);
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
                while (this.enumerator.MoveNext())
                {
                    TSource current = this.enumerator.Current;
                    if (this.predicate(current))
                    {
                        this.current = current;
                        return true;
                    }
                }
                this.Dispose();
                return false;
            }
            public override IEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
            {
                return new JsImplEnumerable.WhereSelectEnumerableIterator<TSource, TResult>(this.source, this.predicate, selector);
            }
            public override IEnumerable<TSource> Where(Func<TSource, bool> predicate)
            {
                return new JsImplEnumerable.WhereEnumerableIterator<TSource>(this.source, JsImplEnumerable.CombinePredicates<TSource>(this.predicate, predicate));
            }
        }
    }
}