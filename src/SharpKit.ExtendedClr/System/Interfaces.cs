using SharpKit.JavaScript;

namespace System
{
    [JsType(Name = "System.IComparable")]
    public interface JsImplIComparable
    {
        int CompareTo(object obj);
    }

    [JsType(Name = "System.IComparable$1")]
    public interface JsImplIComparable<T>
    {
        int CompareTo(T other);
    }

    [JsType(Name = "System.IEquatable$1")]
    public interface JsImplIEquatable<T>
    {
        bool Equals(T other);
    }


    [JsType(Name = "System.IFormattable")]
    public interface JsImplIFormattable
    {
        string ToString(string format, System.IFormatProvider formatProvider);
    }
    
    [JsType(Name = "System.IComparar$1")]
    public interface JsImplIComparar<T>
    {
        int Compare(T x, T y);
    }

}
