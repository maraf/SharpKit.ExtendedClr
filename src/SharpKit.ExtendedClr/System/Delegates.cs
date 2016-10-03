using SharpKit.JavaScript;
using System;
using System.Collections.Generic;

using System.Text;


namespace System
{

	[JsType(Name = "System.Action")]
	internal delegate void JsImplAction();

	[JsType(Name = "System.Action$1")]
	internal delegate void JsImplAction<T>(T obj);

	[JsType(Name = "System.Action$2")]
    internal delegate void JsImplAction<T1, T2>(T1 arg1, T2 arg2);

    [JsType(Name = "System.Action$3")]
    internal delegate void JsImplAction<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

    [JsType(Name = "System.Action$4")]
    internal delegate void JsImplAction<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

	[JsType(Name = "System.Func$1")]
	internal delegate TResult JsImplFunc<TResult>();

	[JsType(Name = "System.Func$2")]
	internal delegate TResult JsImplFunc<T, TResult>(T obj);

	[JsType(Name = "System.Func$3")]
	internal delegate TResult JsImplFunc<T1, T2, TResult>(T1 arg1, T2 arg2);

	[JsType(Name = "System.Func$4")]
	internal delegate TResult JsImplFunc<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);

	[JsType(Name = "System.EventHandler")]
	internal delegate void JsImplEventHandler(object sender, JsImplEventArgs e);

	[JsType(Name = "System.EventHandler$1")]
	internal delegate void JsImplEventHandler<TEventArgs>(object sender, TEventArgs e) where TEventArgs : EventArgs;

	[JsType(Name = "System.Predicate$1")]
	internal delegate bool JsImplPredicate<T>(T item);

}