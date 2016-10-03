using SharpKit.JavaScript;
using System.Collections.Specialized;

namespace System.Collections.Specialized
{
	[JsType(Name = "System.Collections.Specialized.NotifyCollectionChangedAction")]
	public enum JSImplNotifyCollectionChangedAction
	{
		Add,
		Remove,
		Replace,
		Move,
		Reset,
	}

	[JsType(Name = "System.Collections.Specialized.INotifyCollectionChanged")]
	public interface JsImplINotifyCollectionChanged
	{
		event NotifyCollectionChangedEventHandler CollectionChanged;
	}

}
