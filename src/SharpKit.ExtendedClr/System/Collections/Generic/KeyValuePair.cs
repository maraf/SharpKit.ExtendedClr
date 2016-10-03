using SharpKit.JavaScript;
using System;
using System.Collections.Generic;

using System.Text;

namespace System.Collections.Generic
{
    [JsType(Name = "System.Collections.Generic.KeyValuePair$2")]
    public struct JsImplKeyValuePair<TKey, TValue>
    {

        TKey _Key;
        TValue _Value;

        JsImplKeyValuePair(TKey key, TValue value)
        {
            this._Key = key;
            this._Value = value;
        }

        public TKey Key
        {
            get
            {
                return this._Key;
            }
        }

        public TValue Value
        {
            get { return _Value; }
        }

    }
}
