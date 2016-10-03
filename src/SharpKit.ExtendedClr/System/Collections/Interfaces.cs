using SharpKit.JavaScript;
using System.Collections;

namespace System.Collections
{
	[JsType(Name = "System.Collections.IDictionary")]
	internal interface IDictionary : ICollection, IEnumerable
	{
		// Methods
		void Add(object key, object value);
		void Clear();
		bool Contains(object key);
		void Remove(object key);

		// Properties
		bool IsFixedSize { get; }
		bool IsReadOnly { get; }
		object this[object key] { get; set; }
		ICollection Keys { get; }
		ICollection Values { get; }
	}

	[JsType(Name = "System.Collections.IEnumerable")]
    public interface JsImplIEnumerable
	{
	}

	[JsType(Name = "System.Collections.IEnumerator")]
	public interface JsImplIEnumerator
	{
	}

	[JsType(Name = "System.Collections.ICollection")]
    public interface JsImplICollection : JsImplIEnumerable
	{
	}

	[JsType(Name = "System.Collections.IList")]
	internal interface JsImplIList : JsImplICollection
	{
	}

    [JsType(Name = "System.Collections.IComparer")]
    internal interface JsImplIComparer
    {
        int Compare(object x, object y);
    }
}
